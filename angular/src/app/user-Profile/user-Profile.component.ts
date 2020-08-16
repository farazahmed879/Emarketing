import { Component, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UserServiceProxy, UserWithdrawDetailServiceProxy, UserDto, UserWithdrawDetailDto, UserPersonalDetailServiceProxy, UserPersonalDetailDto, AdminServiceProxy, CreateWithdrawRequestDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { PrimefacesDropDownObject } from '@app/app.component';
import * as moment from 'moment';

@Component({
  templateUrl: './user-profile.component.html',
  animations: [appModuleAnimation()],
})
export class UserProfileComponent extends AppComponentBase {
  user = new UserDto();
  id: number;
  saving = false;
  withDrawTypeId: number;
  accountTitle: string;
  accountNumber: string;
  jazzCashNumber: string;
  easyPaisaNumber: string;
  withdrawTypeArrayObj: PrimefacesDropDownObject[];
  selectedWithdrawType: PrimefacesDropDownObject;
  userWithdrawDetail: UserWithdrawDetailDto;
  gender = [
    { value: 1, label: 'Male' },
    { value: 2, label: 'Female' }
  ];
  selectedGender: PrimefacesDropDownObject;

  //Personal Details
  cnic: string;
  dob: moment.Moment;
  phoneNumber: string;
  address: string;
  city: string;
  country: string;
  postalCode: string;
  state: string;


  constructor(injector: Injector,
    public _adminService: AdminServiceProxy,
    public _withdrawDetailAppService: UserWithdrawDetailServiceProxy,
    public _userPersonalDetailsService: UserPersonalDetailServiceProxy,
  ) {
    super(injector);


  }

  ngOnInit(): void {
    this.getUserInofrmation();
    this.getWithdrawType();
    this.getPaymentDetaiwlByUserId();
    this.getPersonalInformation();
  }
  getUserInofrmation() {
    this._adminService.getUserDetailId(this.appSession.userId).subscribe((result) => {
      this.user.emailAddress = result.emailAddress;
      this.user.fullName = result.fullName;
      this.user.name = result.name;
      this.user.surname = result.surname;
      this.user.isActive = result.isActive;
      this.user.userName = result.userName;
      this.user.id = result.id;
    });
  }

  getWithdrawType() {
    this._adminService.getWithdrawTypes().subscribe((result) => {
      if (result) {
        this.withdrawTypeArrayObj = result.map(item =>
          ({
            label: item.Name,
            value: item.WithdrawTypeId
          }));
        console.log("withdrawTypeArrayObj", this.withdrawTypeArrayObj);
      }
    })
  }

  getPersonalInformation() {
    this._userPersonalDetailsService.getByUserId().subscribe((result) => {
      if (result) {
        this.cnic = result.nicNumber;
        this.dob = moment(result.birthday);
        this.phoneNumber = result.phoneNumber;
        this.address = result.address;
        this.city = result.city;
        this.postalCode = result.postalCode;
        this.state = result.state;
        this.country = result.country;
        this.selectedGender.value = result.gender;
        console.log("getPersonalInformation", result);
      }
    })
  }

  getPaymentDetaiwlByUserId() {
    this._adminService.getByUserId(this.appSession.userId).subscribe((result) => {
      if (result) {
        this.withDrawTypeId = result.withdrawTypeId;
        this.accountNumber = result.accountIBAN;
        this.accountTitle = result.accountTitle;
        this.easyPaisaNumber = result.easyPaisaNumber;
        this.jazzCashNumber = result.jazzCashNumber;
        console.log("getPaymentDetaiwlByUserId", result);
      }
    })
  }

  saveUserProfile() {
    this.saving = true;
    this._adminService
      .update(this.user)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
      });
  }

  SavePaymentDetails() {
    var userWithdrawDetail = new UserWithdrawDetailDto();
    userWithdrawDetail.userId = this.appSession.userId;
    userWithdrawDetail.jazzCashNumber = this.jazzCashNumber;
    userWithdrawDetail.withdrawTypeId = this.withDrawTypeId;
    userWithdrawDetail.easyPaisaNumber = this.easyPaisaNumber;
    userWithdrawDetail.accountIBAN = this.accountNumber;
    userWithdrawDetail.accountTitle = this.accountTitle;
    this._withdrawDetailAppService.createOrEdit(userWithdrawDetail).pipe(
      finalize(() => {
        this.saving = false;
      })
    )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
      });

  }

  savePersonalDetails() {
    var userPersonalDetailDto = new UserPersonalDetailDto();
    userPersonalDetailDto.userId = this.appSession.userId;
    userPersonalDetailDto.userName = this.appSession.user.name;
    userPersonalDetailDto.nicNumber = this.cnic;
    userPersonalDetailDto.gender = this.selectedGender.value;
    userPersonalDetailDto.phoneNumber = this.phoneNumber;
    userPersonalDetailDto.address = this.address;
    userPersonalDetailDto.birthday = moment();
    userPersonalDetailDto.city = this.city;
    userPersonalDetailDto.postalCode = this.postalCode;
    userPersonalDetailDto.state = this.state;
    userPersonalDetailDto.country = this.country;

    this._userPersonalDetailsService.createOrEdit(userPersonalDetailDto).pipe(
      finalize(() => {
        this.saving = false;
      })
    )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
      });
  }
}
