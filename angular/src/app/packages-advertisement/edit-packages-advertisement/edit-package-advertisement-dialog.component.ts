import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
  Optional,
  Inject
} from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import * as _ from 'lodash';
import { AppComponentBase } from '@shared/app-component-base';
import {
  PackageDto,
  PackageServiceProxy,
  PackageAdDto,
  PackageAdServiceProxy
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './edit-package-advertisement-dialog.component.html'
})
export class EditPackagesAdvertisementComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  packageAd = new PackageAdDto();
  id: number;
  packageId: number;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _packageAdService: PackageAdServiceProxy,
    public bsModalRef: BsModalRef,
    private _modalService: BsModalService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.show();
  }

  show() {
    this.packageId;
    debugger;
    this._packageAdService.getById(this.id).subscribe((result) => {
      this.packageAd = result;
    }
    )
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
