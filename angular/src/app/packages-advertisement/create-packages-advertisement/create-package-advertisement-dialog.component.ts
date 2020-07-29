import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output
} from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import * as _ from 'lodash';
import { AppComponentBase } from '@shared/app-component-base';
import {
  PackageDto,
  PackageServiceProxy,
  PackageAdServiceProxy,
  PackageAdDto
} from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';

@Component({
  templateUrl: './create-package-advertisement-dialog.component.html'
})
export class CreatePackagesAdvertisementComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  packageAd = new PackageAdDto();
  packageId: number;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _packageAdService: PackageAdServiceProxy,
    public bsModalRef: BsModalRef,
    private _modalService: BsModalService,
    private activatedRoute: ActivatedRoute
    
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.packageId = parseInt(this.activatedRoute.snapshot.paramMap.get('packageId'));
  }
  show(packageId){
    debugger;
  }


  save(): void {
    this.saving = true;
    debugger;
    this.packageAd.packageId = this.packageId;
    this._packageAdService
      .createOrEdit(this.packageAd)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit();
      });
  }
  onIsActivceChange(event){

  }

}
