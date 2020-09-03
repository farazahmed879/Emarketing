import { Component, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { WithdrawRequestServiceProxy, AdminServiceProxy,  WithdrawRequestDto, WithdrawRequestDtoPagedResultDto, CreateWithdrawRequestDto } from '@shared/service-proxies/service-proxies';
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
    private _adminService: AdminServiceProxy,
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
        request.keyword,
        
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

  markAsPaidRequest(event: WithdrawRequestDto) {
    debugger;
    var createWithdrawRequestDto = new CreateWithdrawRequestDto();
    createWithdrawRequestDto.id = event.id;
    this._adminService.createOrEditWithdrawRequest(createWithdrawRequestDto).subscribe((result) => {
      if (result)
        this.notify.info(this.l('SavedSuccessfully'));
    })
  }


}
