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

@Component({
  templateUrl: './create-package-advertisement-dialog.component.html'
})
export class CreatePackagesAdvertisementComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  packageAd = new PackageAdDto();

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _packageAdService: PackageAdServiceProxy,
    public bsModalRef: BsModalRef,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  ngOnInit(): void {

  }


  save(): void {
    this.saving = true;

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


}
