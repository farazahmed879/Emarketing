import { Component, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UserPackageAdDetailDto, UserPackageAdDetailServiceProxy, UserPackageAdDetailDtoPagedResultDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { PagedRequestDto, PagedListingComponentBase } from '@shared/paged-listing-component-base';
import { EditUserPackageAdsDetailComponent } from './edit-user-package-ads-detail/edit-user-package-ads-detail-dialog.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Route } from '@angular/compiler/src/core';
import { Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';


class PagedWithdrawHistoryDto extends PagedRequestDto {
  keyword: string;
}
@Component({
  templateUrl: './user-package-ads-detail.component.html',
  animations: [appModuleAnimation()]
})
export class UserPackageAdsDetailComponent extends PagedListingComponentBase<UserPackageAdDetailDto> {
  constructor(injector: Injector,
    private _userPackageDetailService: UserPackageAdDetailServiceProxy,
    private _modalService: BsModalService,
    private router: Router,
    public sanitizer: DomSanitizer
  ) {
    super(injector);
  }
  keyword: string;
  userPackageDetail: UserPackageAdDetailDtoPagedResultDto;
  ngOnInIt() {

  }
  protected list(
    request: PagedWithdrawHistoryDto,
    pageNumber: number
  ): void {
    request.keyword = this.keyword;

    this._userPackageDetailService
      .getPaginatedAll(
        this.keyword, 
        request.skipCount,
        request.maxResultCount
      )
      .subscribe((result: UserPackageAdDetailDtoPagedResultDto) => {
        this.isTableLoading = false;
        this.userPackageDetail = result;
        console.log("Ads", result);
        this.showPaging(result, pageNumber);
      });
  }

  delete() {

  }

  viewUserPackageAdDetail(id: number){
    //this.showCreateOrEditUserPackageAdDetailDialog(id);
    this.router.navigate(['/app/ads-detail',id]);
  }

  private showCreateOrEditUserPackageAdDetailDialog(id?: number): void {
    let createOrEditUserPackageAdDetailDialog: BsModalRef;
      createOrEditUserPackageAdDetailDialog = this._modalService.show(
        EditUserPackageAdsDetailComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      );
    

    createOrEditUserPackageAdDetailDialog.content.onSave.subscribe(() => {
     // this.refresh();
      var pagedHistory = new PagedWithdrawHistoryDto();
      this.list(pagedHistory,1);
    });
  }


}
