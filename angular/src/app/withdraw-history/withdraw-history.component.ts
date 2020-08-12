import { Component, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { WithdrawRequestServiceProxy, WithdrawRequestDto, WithdrawRequestDtoPagedResultDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { PagedRequestDto, PagedListingComponentBase } from '@shared/paged-listing-component-base';


class PagedWithdrawHistoryDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}
@Component({
  templateUrl: './withdraw-history.component.html',
  animations: [appModuleAnimation()]
})
export class WithdrawHistoryComponent extends PagedListingComponentBase<WithdrawRequestDto> {
  constructor(injector: Injector,
    private _withdrawRequestService: WithdrawRequestServiceProxy,
    ) {
    super(injector);
    
  }
 
  keyword: string;
  withdrawRequestHistory: WithdrawRequestDtoPagedResultDto;
  ngOnInIt(){

  }
  list(
    request: PagedWithdrawHistoryDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;
    request.isActive = false;

    this._withdrawRequestService
      .getPaginatedAll(
        undefined,
        request.isActive,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: WithdrawRequestDtoPagedResultDto) => {
        this.withdrawRequestHistory = result;
        console.log(result)
        this.isTableLoading = false;
        this.showPaging(result, pageNumber);
      });
  }
  delete(){

  }


}
