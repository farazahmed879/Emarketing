import { Component,
  Injector,
  OnInit,
  EventEmitter,
  Output } from '@angular/core';

  import { finalize } from 'rxjs/operators';
  import { BsModalRef } from 'ngx-bootstrap/modal';
  import * as _ from 'lodash';
  import { AppComponentBase } from '@shared/app-component-base';
  import {
    UserReferralServiceProxy,
    UserReferralDto,
    
  } from '@shared/service-proxies/service-proxies';
  
@Component({
 // selector: 'app-view-user-referral',
  templateUrl: './view-user-referral.component.html',
 // styleUrls: ['./view-user-referral.component.css']
})
export class ViewUserReferralComponent extends AppComponentBase 
implements OnInit {
  saving = false;
  userReferral = new UserReferralDto();
   
  id: number;

  @Output() onSave = new EventEmitter<any>();
  constructor(injector: Injector,
    public _userReferralService: UserReferralServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._userReferralService.getById(this.id).subscribe((result) => {
      this.userReferral = result;

      
    });
  }

  save(): void {
    this.saving = true;

    // this.user.roleNames = this.getCheckedRoles();

    // this._userService
    //   .update(this.user)
    //   .pipe(
    //     finalize(() => {
    //       this.saving = false;
    //     })
    //   )
    //   .subscribe(() => {
    //     this.notify.info(this.l('SavedSuccessfully'));
    //     this.bsModalRef.hide();
    //     this.onSave.emit();
    //   });
  }
}
