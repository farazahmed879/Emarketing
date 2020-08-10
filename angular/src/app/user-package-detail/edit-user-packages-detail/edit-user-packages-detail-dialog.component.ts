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
  UserPackageAdDetailServiceProxy,
  UserPackageAdDetailDto
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './edit-user-packages-detail-dialog.component.html'
})
export class EditUserPackagesDetailComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  userPackageAdDetail = new UserPackageAdDetailDto();
  id: number;
  packageId: number;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _userPackageAdDetailService: UserPackageAdDetailServiceProxy,
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
    this._userPackageAdDetailService.getById(this.id).subscribe((result) => {
      this.userPackageAdDetail = result;
    }
    )
  }


  save(): void {
    this.saving = true;

    this._userPackageAdDetailService
      .createOrEdit(this.userPackageAdDetail)
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
