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
  PackageServiceProxy
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './edit-package-dialog.component.html'
})
export class EditPackageDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  package = new PackageDto();
  id: number;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _packageService: PackageServiceProxy,
    public bsModalRef: BsModalRef,
    private _modalService: BsModalService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._packageService.getById(this.id).subscribe((result) => {
      this.package = result;
    }
    )
  }


  save(): void {
    this.saving = true;

    this._packageService
      .createOrEdit(this.package)
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
