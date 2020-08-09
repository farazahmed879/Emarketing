import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { WithdrawRequestServiceProxy, WithdrawRequestDto, WithdrawRequestDtoPagedResultDto, UserRequestServiceProxy, UserRequestDtoPagedResultDto, UserReferralServiceProxy, UserReferralDtoPagedResultDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { PagedRequestDto, PagedListingComponentBase } from '@shared/paged-listing-component-base';


class PagedWithdrawHistoryDto extends PagedRequestDto {
  keyword: string;
}
@Component({
  templateUrl: './user-referal.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserReferalComponent extends PagedListingComponentBase<WithdrawRequestDto> {
  constructor(injector: Injector,
    private _userReferalService: UserReferralServiceProxy,
  ) {
    super(injector);

  }

  keyword: string;
  userReferal: UserReferralDtoPagedResultDto;
  ngOnInIt() {

  }
  protected list(
    request: PagedWithdrawHistoryDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._userReferalService
      .getPaginatedAll(
        undefined,
        undefined,
        undefined,
        undefined,
        undefined,
        undefined,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: UserReferralDtoPagedResultDto) => {
        this.userReferal = result;
        console.log("userReferal", result);
        this.showPaging(result, pageNumber);
      });
  }

  delete() {

  }


}
