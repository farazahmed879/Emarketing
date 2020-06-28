using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Emarketing.BusinessModels.WithdrawRequest.Dto;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.WithdrawRequest
{
    public interface IWithdrawRequestAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(CreateWithdrawRequestDto withdrawRequestDto);

        Task<WithdrawRequestDto> GetById(long withdrawRequestId);

        Task<ResponseMessageDto> DeleteAsync(long withdrawRequestId);

        Task<List<WithdrawRequestDto>> GetAll(long? userId);

        Task<PagedResultDto<WithdrawRequestDto>> GetPaginatedAllAsync(PagedCreateWithdrawRequestResultRequestDto input);
    }


    public class WithdrawRequestAppService : AbpServiceBase, IWithdrawRequestAppService
    {
        private readonly IRepository<BusinessObjects.WithdrawRequest, long> _withdrawRequestRepository;


        public WithdrawRequestAppService(
            IRepository<BusinessObjects.WithdrawRequest, long> withdrawRequestRepository)

        {
            _withdrawRequestRepository = withdrawRequestRepository;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(CreateWithdrawRequestDto withdrawRequestDto)
        {
            ResponseMessageDto result;
            if (withdrawRequestDto.Id == 0)
            {
                result = await CreateWithdrawRequestAsync(withdrawRequestDto);
            }
            else
            {
                result = await UpdateWithdrawRequestAsync(withdrawRequestDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateWithdrawRequestAsync(CreateWithdrawRequestDto withdrawRequestDto)
        {
            var result = await _withdrawRequestRepository.InsertAsync(new BusinessObjects.WithdrawRequest()
            {
                Amount = withdrawRequestDto.Amount,
                Status = false,
                WithdrawTypeId =withdrawRequestDto.WithdrawTypeId,
                UserId = withdrawRequestDto.UserId,
            });

            await UnitOfWorkManager.Current.SaveChangesAsync();

            if (result.Id != 0)
            {
                return new ResponseMessageDto()
                {
                    Id = result.Id,
                    SuccessMessage = AppConsts.SuccessfullyInserted,
                    Success = true,
                    Error = false,
                };
            }

            return new ResponseMessageDto()
            {
                Id = 0,
                ErrorMessage = AppConsts.InsertFailure,
                Success = false,
                Error = true,
            };
        }

        private async Task<ResponseMessageDto> UpdateWithdrawRequestAsync(CreateWithdrawRequestDto withdrawRequestDto)
        {
            var result = await _withdrawRequestRepository.UpdateAsync(new BusinessObjects.WithdrawRequest()
            {
                Id = withdrawRequestDto.Id,
                Amount = withdrawRequestDto.Amount,
                Status = withdrawRequestDto.Status,
                WithdrawTypeId = withdrawRequestDto.WithdrawTypeId,
                UserId = withdrawRequestDto.UserId,
            });

            if (result != null)
            {
                return new ResponseMessageDto()
                {
                    Id = result.Id,
                    SuccessMessage = AppConsts.SuccessfullyUpdated,
                    Success = true,
                    Error = false,
                };
            }

            return new ResponseMessageDto()
            {
                Id = 0,
                ErrorMessage = AppConsts.UpdateFailure,
                Success = false,
                Error = true,
            };
        }

        public async Task<WithdrawRequestDto> GetById(long withdrawRequestId)
        {
            var result = await _withdrawRequestRepository.GetAll()
                .Where(i => i.Id == withdrawRequestId)
                .Select(i =>
                    new WithdrawRequestDto()
                    {
                        Id = i.Id,
                        //Name = i.Name,
                        //Description = i.Description
                    })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<ResponseMessageDto> DeleteAsync(long withdrawRequestId)
        {
            var model = await _withdrawRequestRepository.GetAll().Where(i => i.Id == withdrawRequestId).FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _withdrawRequestRepository.UpdateAsync(model);

            return new ResponseMessageDto()
            {
                Id = withdrawRequestId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<WithdrawRequestDto>> GetAll(long? userId)
        {
            var result = await _withdrawRequestRepository.GetAll().Where(i => i.IsDeleted == false && i.UserId == userId)
                .Select(i => new WithdrawRequestDto()
                {
                    Id = i.Id,
                    //Name = i.Name,
                    //Description = i.Description,
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime
                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<WithdrawRequestDto>> GetPaginatedAllAsync(
            PagedCreateWithdrawRequestResultRequestDto input)
        {
            var filteredWithdrawRequests = _withdrawRequestRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserId== input.UserId);
                //.Where(i => i.IsDeleted == false && (input.TenantId == null || i.TenantId == input.TenantId))
                //.WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserName.Contains(input.Name));

            var pagedAndFilteredWithdrawRequests = filteredWithdrawRequests
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredWithdrawRequests.Count();

            return new PagedResultDto<WithdrawRequestDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredWithdrawRequests.Where(i => i.IsDeleted == false).Select(i =>
                        new WithdrawRequestDto()
                        {
                            Id = i.Id,
                            //Name = i.Name,
                            //Description = i.Description,
                            //TenantId = i.TenantId
                        })
                    .ToListAsync());
        }
    }
}