import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';`1  `
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PackageServiceProxy, PackageDtoPagedResultDto, PackageDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { PagedRequestDto, PagedListingComponentBase } from '@shared/paged-listing-component-base';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreatePackageDialogComponent } from './create-package/create-package-dialog.component';


class PagedWithdrawHistoryDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}
@Component({
  templateUrl: './packages.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PackagesComponent extends PagedListingComponentBase<PackageDto> {
  constructor(injector: Injector,
    private _packageService: PackageServiceProxy,
    private _modalService: BsModalService
    ) {
    super(injector);
    
  }
 
  keyword: string;
  packages: PackageDtoPagedResultDto;
  ngOnInIt(){

  }
  protected list(
    request: PagedWithdrawHistoryDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;
    request.isActive = false;

    this._packageService
      .getPaginatedAll(
        undefined,
        this.keyword,
        undefined,
        undefined,
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
      .subscribe((result: PackageDtoPagedResultDto) => {
        this.packages = result;
        console.log("packages",result);
        this.showPaging(result, pageNumber);
      });
  }
  delete(){

  }


  createPackage(): void {
    this.showCreateOrEditPackageDialog();
  }

  editUser(user: PackageDto): void {
    this.showCreateOrEditPackageDialog(user.id);
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
      // createOrEditPackageDialog = this._modalService.show(
      //   EditUserDialogComponent,
      //   {
      //     class: 'modal-lg',
      //     initialState: {
      //       id: id,
      //     },
      //   }
      // );
    }

    createOrEditPackageDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }


}
