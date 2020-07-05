using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Emarketing.BusinessModels.WithdrawRequest.Dto;
using Emarketing.Sessions;
using Emarketing.Sessions.Dto;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.WithdrawRequest
{
    public interface IWithdrawRequestAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(CreateWithdrawRequestDto withdrawRequestDto);

        Task<WithdrawRequestDto> GetById(long withdrawRequestId);

        Task<ResponseMessageDto> DeleteAsync(long withdrawRequestId);

        Task<List<WithdrawRequestDto>> GetAll();

        Task<PagedResultDto<WithdrawRequestDto>> GetPaginatedAllAsync(WithdrawRequestInputDto input);
    }


    public class WithdrawRequestAppService : AbpServiceBase, IWithdrawRequestAppService
    {
        private readonly IRepository<BusinessObjects.WithdrawRequest, long> _withdrawRequestRepository;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAbpSession _abpSession;


        public WithdrawRequestAppService(
            IRepository<BusinessObjects.WithdrawRequest, long> withdrawRequestRepository,
            ISessionAppService sessionAppService,
            IAbpSession abpSession)

        {
            _withdrawRequestRepository = withdrawRequestRepository;
            _sessionAppService = sessionAppService;
            _abpSession = abpSession;
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
                WithdrawTypeId = withdrawRequestDto.WithdrawTypeId,
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
                        Amount = i.Amount,
                        WithdrawTypeId = i.WithdrawTypeId,
                        UserId = i.UserId,
                        UserName = $"{i.User.FullName}",
                        CreatorUserId = i.CreatorUserId,
                        CreationTime = i.CreationTime,
                        LastModificationTime = i.LastModificationTime,
                        LastModifierUserId = i.LastModifierUserId
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

        public async Task<List<WithdrawRequestDto>> GetAll()
        {
            var userId = _abpSession.UserId;

            var result = await _withdrawRequestRepository.GetAll().Where(i => i.IsDeleted == false && i.UserId == userId)
                .Select(i => new WithdrawRequestDto()
                {
                    Id = i.Id,
                    Amount = i.Amount,
                    WithdrawTypeId = i.WithdrawTypeId,
                    UserId = i.UserId,
                    UserName = $"{i.User.FullName}",
                    CreatorUserId = i.CreatorUserId,
                    CreationTime = i.CreationTime,
                    LastModificationTime = i.LastModificationTime,
                    LastModifierUserId = i.LastModifierUserId

                }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<WithdrawRequestDto>> GetPaginatedAllAsync(
            WithdrawRequestInputDto input)
        {
            var userId = _abpSession.UserId;
            var filteredWithdrawRequests = _withdrawRequestRepository.GetAll()
                .Where(x => x.UserId == userId)
                .WhereIf(input.Status.HasValue, x => x.Status == input.Status);
            //.Where(i => i.IsDeleted == false && (input.TenantId == null || i.TenantId == input.TenantId))
            //.WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserName.Contains(input.Name));

            var pagedAndFilteredWithdrawRequests = filteredWithdrawRequests
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredWithdrawRequests.Count();

            var result =  new PagedResultDto<WithdrawRequestDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredWithdrawRequests.Where(i => i.IsDeleted == false).Select(i =>
                        new WithdrawRequestDto()
                        {
                            Id = i.Id,
                            Amount = i.Amount,
                            WithdrawTypeId = i.WithdrawTypeId,
                            UserId = i.UserId,
                            UserName = $"{i.User.FullName}",
                            CreatorUserId = i.CreatorUserId,
                            CreationTime = i.CreationTime,
                            LastModificationTime = i.LastModificationTime,
                            LastModifierUserId = i.LastModifierUserId
                        })
                    .ToListAsync());
            return result;
        }


    }
}