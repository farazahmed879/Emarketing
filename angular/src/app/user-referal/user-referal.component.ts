import { Component, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { WithdrawRequestServiceProxy, WithdrawRequestDto, WithdrawRequestDtoPagedResultDto, UserRequestServiceProxy, UserRequestDtoPagedResultDto, UserReferralServiceProxy, UserReferralDtoPagedResultDto, UserReferralDto, UpdateUserReferralDto ,AdminServiceProxy} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { PagedRequestDto, PagedListingComponentBase } from '@shared/paged-listing-component-base';


class PagedWithdrawHistoryDto extends PagedRequestDto {
  keyword: string;
}
@Component({
  templateUrl: './user-referal.component.html',
  animations: [appModuleAnimation()]
})
export class UserReferalComponent extends PagedListingComponentBase<WithdrawRequestDto> {
  constructor(injector: Injector,
    private _adminService: AdminServiceProxy,
    private _userReferalService: UserReferralServiceProxy,
  ) {
    super(injector);

  }

  keyword: string;
  userReferal: UserReferralDtoPagedResultDto;
  ngOnInIt() {

  }
  list(
    request: PagedWithdrawHistoryDto,
    pageNumber: number
  ): void {
    request.keyword = this.keyword;

    this._userReferalService
      .getPaginatedAll(
        this.keyword,
        request.skipCount,
        request.maxResultCount
      )
      .subscribe((result: UserReferralDtoPagedResultDto) => {
        this.userReferal = result;
        this.isTableLoading = false;
        console.log("userReferal", result);
        this.showPaging(result, pageNumber);
      });
  }

  delete() {

  }

  acceptRequest(event: UserReferralDtoPagedResultDto){

  }

  markAsPaidReferral(event: UserReferralDto) {
    debugger;
    var updateUserReferralDto = new UpdateUserReferralDto();
    updateUserReferralDto.userReferralId = event.id;
    this._adminService.updateUserReferral(updateUserReferralDto).subscribe((result) => {
      if (result)
        this.notify.info(this.l('SavedSuccessfully'));
    })
  }

}
