import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PackageServiceProxy, PackageDtoPagedResultDto, PackageDto, UserReferralRequestServiceProxy, UserReferralRequestDtoPagedResultDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { PagedRequestDto, PagedListingComponentBase } from '@shared/paged-listing-component-base';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateUserReferalRequestDialogComponent } from './create-user-referal-request/create-user-referal-request-dialog.component';
import { EditUserReferalRequestDialogComponent } from './edit-user-referal-request/edit-user-referal-request-dialog.component';


class PagedWithdrawHistoryDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}
@Component({
  templateUrl: './user-referal-request.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserReferalRequestComponent extends PagedListingComponentBase<PackageDto> {
  constructor(injector: Injector,
    private _userReferalRequestService: UserReferralRequestServiceProxy,
    private _modalService: BsModalService
    ) {
    super(injector);
    
  }
 
  keyword: string;
  userReferalRequest: UserReferralRequestDtoPagedResultDto;
  
  ngOnInit(): void {
    var pagedHistory = new PagedWithdrawHistoryDto();
    this.list(pagedHistory,1);
  }

  protected list(
    request: PagedWithdrawHistoryDto,
    pageNumber: number
  ): void {
    request.keyword = this.keyword;
    request.isActive = false;
    this._userReferalRequestService
      .getPaginatedAll(
        undefined,
        undefined,
        undefined,
        undefined,
        undefined,
        undefined,
        undefined,
        request.skipCount,
        request.maxResultCount
      )
      .subscribe((result: UserReferralRequestDtoPagedResultDto) => {
        this.userReferalRequest = result;
        console.log("userReferalRequest",result);
        this.showPaging(result, pageNumber);
      });
  }
  delete(event: PackageDto){
    abp.message.confirm(
      this.l('UserDeleteWarningMessage', event.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._userReferalRequestService.delete(event.id).subscribe(() => {
            abp.notify.success(this.l('SuccessfullyDeleted'));
            this.refresh();
          });
        }
      }
    );
  }


  createUserReferalRequest(): void {
    this.showCreateOrEditUserReferalRequestDialog();
  }

  editUserReferalRequest(event: PackageDto): void {
    this.showCreateOrEditUserReferalRequestDialog(event.id);
  }


  private showCreateOrEditUserReferalRequestDialog(id?: number): void {
    debugger;
    let createOrEditUserReferalRequestDialog: BsModalRef;
    if (!id) {
      createOrEditUserReferalRequestDialog = this._modalService.show(
        CreateUserReferalRequestDialogComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditUserReferalRequestDialog = this._modalService.show(
        EditUserReferalRequestDialogComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      );
    }

    createOrEditUserReferalRequestDialog.content.onSave.subscribe(() => {
     // this.refresh();
      var pagedHistory = new PagedWithdrawHistoryDto();
      this.list(pagedHistory,1);
    });
  }


}
