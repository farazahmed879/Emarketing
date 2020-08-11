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
  UserReferralRequestDto
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './create-user-referal-request-dialog.component.html'
})
export class CreateUserReferalRequestDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  userReferalRequest = new UserReferralRequestDto();

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _userReferalRequestService: UserReferralRequestServiceProxy,
    public bsModalRef: BsModalRef,
    private _modalService: BsModalService,
  ) {
    super(injector);
  }

  ngOnInit(): void {

  }


  save(): void {
    this.saving = true;
    this.userReferalRequest.userId = this.appSession.userId;
    
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
