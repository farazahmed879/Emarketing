import { Component, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PackageServiceProxy, PackageDtoPagedResultDto, PackageDto, AdminServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { PagedRequestDto, PagedListingComponentBase } from '@shared/paged-listing-component-base';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreatePackageDialogComponent } from './create-package/create-package-dialog.component';
import { EditPackageDialogComponent } from './edit-package/edit-package-dialog.component';


class PagedWithdrawHistoryDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}
@Component({
  templateUrl: './packages.component.html',
  animations: [appModuleAnimation()]
})
export class PackagesComponent extends PagedListingComponentBase<PackageDto> {
  constructor(injector: Injector,
    private _packageService: PackageServiceProxy,
    private _adminService: AdminServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);

  }

  keyword: string;
  packages: PackageDtoPagedResultDto;

  ngOnInit(): void {
    var pagedHistory = new PagedWithdrawHistoryDto();
    this.list(pagedHistory, 1);
  }

  protected list(
    request: PagedWithdrawHistoryDto,
    pageNumber: number
  ): void {
    request.keyword = this.keyword;
    request.isActive = false;
    this._packageService
      .getPaginatedAll(
        undefined,
        this.keyword,

        request.isActive,
        request.skipCount,
        request.maxResultCount
      )
      .subscribe((result: PackageDtoPagedResultDto) => {
        this.packages = result;
        console.log("packages", result);
        this.showPaging(result, pageNumber);
      });
  }
  delete(event: PackageDto) {
    abp.message.confirm(
      this.l('UserDeleteWarningMessage', event.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._packageService.delete(event.id).subscribe(() => {
            abp.notify.success(this.l('SuccessfullyDeleted'));
            this.refresh();
          });
        }
      }
    );
  }


  createPackage(): void {
    this.showCreateOrEditPackageDialog();
  }

  editPackage(event: PackageDto): void {
    this.showCreateOrEditPackageDialog(event.id);
  }

  renewPackage() {
    this._adminService.renewPackageAdForUsers().subscribe(result => {
      if (result) {
        abp.notify.success(this.l('SuccessfullyRenewPackage'));
      }
    })
  }


  private showCreateOrEditPackageDialog(id?: number): void {
    let createOrEditPackageDialog: BsModalRef;
    if (!id) {
      createOrEditPackageDialog = this._modalService.show(
        CreatePackageDialogComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditPackageDialog = this._modalService.show(
        EditPackageDialogComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      );
    }

    createOrEditPackageDialog.content.onSave.subscribe(() => {
      // this.refresh();
      var pagedHistory = new PagedWithdrawHistoryDto();
      this.list(pagedHistory, 1);
    });
  }


}
