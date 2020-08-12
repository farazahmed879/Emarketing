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
  UserReferralRequestServiceProxy,
  UserReferralRequestDto,
  PackageServiceProxy,
  CreateUserReferralRequestDto
} from '@shared/service-proxies/service-proxies';
import { PrimefacesDropDownObject } from '@app/app.component';

@Component({
  templateUrl: './create-user-referal-request-dialog.component.html'
})
export class CreateUserReferalRequestDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  userReferalRequest = new CreateUserReferralRequestDto();
  packages: PrimefacesDropDownObject[];
  selectedPackageId: number;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _userReferalRequestService: UserReferralRequestServiceProxy,
    public _packageService: PackageServiceProxy,
    public bsModalRef: BsModalRef,
    private _modalService: BsModalService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getAllPackages();
  }


  getAllPackages() {
    this._packageService.getAll().subscribe((result) => {
      if (result) {
        console.log("packages", result);
        this.packages = result.map(item =>
          ({
            label: item.name,
            value: item.id
          }));
      }
    })
  }

  save(): void {
    debugger;
    this.saving = true;
    this.userReferalRequest.userId = this.appSession.userId;
    this.userReferalRequest.packageId = this.selectedPackageId;
    this._userReferalRequestService
      .createOrEdit(this.userReferalRequest)
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
