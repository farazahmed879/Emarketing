import { Component, Injector } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PackageServiceProxy, PackageDtoPagedResultDto, PackageDto, PackageAdServiceProxy, PackageAdDto, PackageAdDtoPagedResultDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { PagedRequestDto, PagedListingComponentBase } from '@shared/paged-listing-component-base';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreatePackagesAdvertisementComponent } from './create-packages-advertisement/create-package-advertisement-dialog.component';
import { EditPackagesAdvertisementComponent } from './edit-packages-advertisement/edit-package-advertisement-dialog.component';
import { ActivatedRoute } from '@angular/router';


class PagedPackageAdDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}
@Component({
  templateUrl: './packages-advertisement.component.html',
  animations: [appModuleAnimation()],
})
export class PackagesAdvertisementComponent extends PagedListingComponentBase<PackageAdDto> {
  constructor(injector: Injector,
    private _packageAdsService: PackageAdServiceProxy,
    public _packageService: PackageServiceProxy,
    private _modalService: BsModalService,
    private activatedRoute: ActivatedRoute
    ) {
    super(injector);
    
  }
 
  packageId: number;
  keyword: string;
  packagesAds: PackageAdDtoPagedResultDto;
  package = new PackageDto();
  
  ngOnInit(): void {
    this.packageId = parseInt(this.activatedRoute.snapshot.paramMap.get('packageId'));    
    var pagedHistory = new PagedPackageAdDto(); 
    this.list(pagedHistory,1,undefined);
    this.getPackageById(this.packageId);
  }
  
  protected list(
    request: PagedPackageAdDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;
    request.isActive = false;
    this._packageAdsService
      .getPaginatedAll(
        this.packageId,
        undefined,
        undefined,
        undefined,      
        request.isActive,
        request.skipCount,
        request.maxResultCount
      )
      .subscribe((result: PackageAdDtoPagedResultDto) => {
        this.packagesAds = result;
        console.log("packages",result);
        this.showPaging(result, pageNumber);
      });
  }

  getPackageById(packageId: number) {
    this._packageService.getById(packageId).subscribe((result) => {
      this.package = result;
      console.log("Package",result);
    }
    )
  }
  delete(event: PackageAdDto){
    abp.message.confirm(
      this.l('UserDeleteWarningMessage', event.title),
      undefined,
      (result: boolean) => {
        if (result) {
          this._packageAdsService.delete(event.id).subscribe(() => {
            abp.notify.success(this.l('SuccessfullyDeleted'));
            this.refresh();
          });
        }
      }
    );
  }


  createPackageAds(): void {
    this.showCreateOrEditPackageDialog();
  }

  editPackage(event: PackageDto): void {
    this.showCreateOrEditPackageDialog(event.id);
  }


  private showCreateOrEditPackageDialog(id?: number): void {
    let createOrEditPackageDialog: BsModalRef;
    if (!id) {
      createOrEditPackageDialog = this._modalService.show(
        CreatePackagesAdvertisementComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: this.packageId
          },
        },
        
      );
    } else {
      createOrEditPackageDialog = this._modalService.show(
        EditPackagesAdvertisementComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
            packageId: this.packageId
          },
        }
      );
    }

    createOrEditPackageDialog.content.onSave.subscribe(() => {
      this.refresh();
      // var pagedHistory = new PagedPackageAdDto();
      // this.list(pagedHistory,1,undefined);
    });
  }


}
