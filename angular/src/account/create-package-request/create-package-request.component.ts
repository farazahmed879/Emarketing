import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { WithdrawRequestDto, WithdrawRequestServiceProxy, CreateWithdrawRequestDto, CreateUserRequestDto, UserRequestServiceProxy, AdminServiceProxy } from '@shared/service-proxies/service-proxies';
import { SelectItem } from 'primeng/api';
import { Router, ActivatedRoute } from '@angular/router';
import { PrimefacesDropDownObject } from '@app/app.component';

interface City {
  name: string;
  id: number;
}

@Component({
  templateUrl: './create-package-request.component.html',
  animations: [appModuleAnimation()]
})
export class CreatePackageRequestComponent extends AppComponentBase implements OnInit {
  //cities1: SelectItem[];
  createUserRequestDto: CreateUserRequestDto;
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  phoneNumber: string;
  password: string;
  passwordConfirm: string;
  packageId: number;
  passwordMatch = false;
  passwordValidationMessage: string;
  passordMessage: string;
  emailValidationMessage: string;
  packages: PrimefacesDropDownObject[];

  customPackages = [
    {
      id: 1, name: 'Package 1',
      isUnlimited: false,
      durationInDays: 60,
      price: 2000,
      totalEarning: 3900,
      profitValue: 0,
      dailyAdCount: 5,
      pricePerAd: 13,
      referralAmount: 300,
      minimumWithdraw: 500,
      maximumWithdraw: 1000
    },
    {
      id: 2,
      name: 'Package 2',
      isUnlimited: false,
      durationInDays: 60,
      price: 3000,
      totalEarning: 3900,
      profitValue: 0,
      dailyAdCount: 5,
      pricePerAd: 20,
      referralAmount: 300,
      minimumWithdraw: 1000,
      maximumWithdraw: 1000
    },
    {
      id: 3,
      name: 'Package 3',
      isUnlimited: true,
      durationInDays: 30,
      price: 5000,
      totalEarning: 11040,
      profitValue: 0,
      dailyAdCount: 5,
      pricePerAd: 73.6,
      referralAmount: 300,
      minimumWithdraw: 500,
      maximumWithdraw: 1000
    },
    {
      id: 4,
      name: 'Package 4',
      isUnlimited: false,
      durationInDays: 60,
      price: 5000,
      totalEarning: 11040,
      profitValue: 0,
      dailyAdCount: 5,
      pricePerAd: 36.8,
      referralAmount: 700,
      minimumWithdraw: 1500,
      maximumWithdraw: 2000
    },
    {
      id: 5,
      name: 'Package 5',
      isUnlimited: false,
      durationInDays: 90,
      price: 10000,
      totalEarning: 26010,
      profitValue: 0,
      dailyAdCount: 5,
      pricePerAd: 57.8,
      referralAmount: 1200,
      minimumWithdraw: 2500,
      maximumWithdraw: 3500
    },
    {
      id: 6,
      name: 'Package 6',
      isUnlimited: false,
      durationInDays: 90,
      price: 20000,
      totalEarning: 37980,
      profitValue: 0,
      dailyAdCount: 5,
      pricePerAd: 84.4,
      referralAmount: 700,
      minimumWithdraw: 5000,
      maximumWithdraw: 6000
    },
    {
      id: 7,
      name: 'Package 7',
      isUnlimited: false,
      durationInDays: 90,
      price: 25000,
      totalEarning: 47700,
      profitValue: 0,
      dailyAdCount: 5,
      pricePerAd: 106,
      referralAmount: 2500,
      minimumWithdraw: 4500,
      maximumWithdraw: 6000
    }
  ];

  constructor(injector: Injector,
    private _adminService: AdminServiceProxy,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {
    super(injector);

  }


  ngOnInit() {
    this.packageId = parseInt(this.activatedRoute.snapshot.paramMap.get('packageId'));
    this.packages = this.customPackages.map(item =>
      ({
        label: item.name,
        value: item.id
      }));
  }

  checkPassword(str) {
    // at least one number, one lowercase and one uppercase letter
    // at least six characters
    var re = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}/;
    return re.test(str);
  }

  create() {
    if (this.firstName == null || this.lastName == null ||
      this.userName == null || this.email == null
      || this.password == null || this.packageId == null) {
      return;
    }
    var createUserRequestDto = new CreateUserRequestDto;
    createUserRequestDto.firstName = this.firstName;
    createUserRequestDto.lastName = this.lastName;
    createUserRequestDto.userName = this.userName;
    createUserRequestDto.email = this.email;
    createUserRequestDto.phoneNumber = this.phoneNumber;
    createUserRequestDto.password = this.password;
    createUserRequestDto.packageId = this.packageId;


    this._adminService.createOrEdit(createUserRequestDto).subscribe((result) => {
      if (result) {
        this.notify.info(this.l('SavedSuccessfully'));
        this.router.navigateByUrl('/account/userPackage');
      }

    })

  }
  back() {

  }

  confirmPassword() {
    if (this.checkPassword(this.password)) {
      if (this.passwordConfirm == this.password) {
        this.passordMessage = "";
        this.passwordValidationMessage = "Password match";
        this.passwordMatch = true;
      }
      else {
        this.passordMessage = "";
        this.passwordValidationMessage = "Password does not match";
        this.passwordMatch = false;
      }
    } else {
      this.passordMessage = "at least one number, one lowercase and one uppercase letter, at least six characters";
    }
  }


  ValidateEmail() {
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email)) {
      this.emailValidationMessage = ""
    }
    this.emailValidationMessage = "Please prove correct email address";
  }
}