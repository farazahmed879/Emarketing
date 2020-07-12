using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Emarketing.BusinessModels.UserReferral.Dto;
using Microsoft.EntityFrameworkCore;

namespace Emarketing.BusinessModels.UserReferral
{
    public interface IUserReferralAppService : IApplicationService
    {
        Task<ResponseMessageDto> CreateOrEditAsync(CreateUserReferralDto userReferralDto);

        Task<UserReferralDto> GetById(long userReferralId);

        Task<ResponseMessageDto> DeleteAsync(long userReferralId);

        Task<List<UserReferralDto>> GetAll(long? userId);

        Task<PagedResultDto<UserReferralDto>> GetPaginatedAllAsync(UserRefferalInputDto input);
    }


    public class UserReferralAppService : AbpServiceBase, IUserReferralAppService
    {
        private readonly IRepository<BusinessObjects.UserReferral, long> _userReferralRepository;


        public UserReferralAppService(
            IRepository<BusinessObjects.UserReferral, long> userReferralRepository)

        {
            _userReferralRepository = userReferralRepository;
        }

        public async Task<ResponseMessageDto> CreateOrEditAsync(CreateUserReferralDto userReferralDto)
        {
            ResponseMessageDto result;
            if (userReferralDto.Id == 0)
            {
                result = await CreateUserReferralAsync(userReferralDto);
            }
            else
            {
                result = await UpdateUserReferralAsync(userReferralDto);
            }

            return result;
        }

        private async Task<ResponseMessageDto> CreateUserReferralAsync(CreateUserReferralDto userReferralDto)
        {
            var result = await _userReferralRepository.InsertAsync(new BusinessObjects.UserReferral()
            {
                UserId = userReferralDto.UserId,
                ReferralUserId = userReferralDto.ReferralUserId,
                ReferralBonusStatusId = ReferralBonusStatus.Inactive,
                ReferralAccountStatusId = userReferralDto.ReferralAccountStatusId,
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

        private async Task<ResponseMessageDto> UpdateUserReferralAsync(CreateUserReferralDto userReferralDto)
        {
            var result = await _userReferralRepository.UpdateAsync(new BusinessObjects.UserReferral()
            {
                Id = userReferralDto.Id,

                ReferralUserId = userReferralDto.ReferralUserId,
                ReferralBonusStatusId = userReferralDto.ReferralBonusStatusId,
                ReferralAccountStatusId = userReferralDto.ReferralAccountStatusId,
                UserId = userReferralDto.UserId,
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

        public async Task<UserReferralDto> GetById(long userReferralId)
        {
            var result = await _userReferralRepository.GetAll()
                .Where(i => i.Id == userReferralId)
                .Select(i =>
                    new UserReferralDto()
                    {
                        Id = i.Id,
                        //Name = i.Name,
                        //Description = i.Description
                    })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<ResponseMessageDto> DeleteAsync(long userReferralId)
        {
            var model = await _userReferralRepository.GetAll().Where(i => i.Id == userReferralId).FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _userReferralRepository.UpdateAsync(model);

            return new ResponseMessageDto()
            {
                Id = userReferralId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<UserReferralDto>> GetAll(long? userId)
        {
            var result = await _userReferralRepository.GetAll().Where(i => i.IsDeleted == false && i.UserId == userId)
                .Select(i => new UserReferralDto()
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

        public async Task<PagedResultDto<UserReferralDto>> GetPaginatedAllAsync(
            UserRefferalInputDto input)
        {
            var filteredUserReferrals = _userReferralRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserId == input.UserId);
            //.Where(i => i.IsDeleted == false && (input.TenantId == null || i.TenantId == input.TenantId))
            //.WhereIf(!string.IsNullOrWhiteSpace(input.UserName), x => x.UserName.Contains(input.Name));

            var pagedAndFilteredUserReferrals = filteredUserReferrals
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = filteredUserReferrals.Count();

            return new PagedResultDto<UserReferralDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredUserReferrals.Where(i => i.IsDeleted == false).Select(i =>
                        new UserReferralDto()
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