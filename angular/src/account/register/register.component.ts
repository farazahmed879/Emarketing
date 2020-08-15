import { Component, Injector, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
  AccountServiceProxy,
  RegisterInput,
  RegisterOutput,
  CreateUserRequestDto,
  AdminServiceProxy
} from '@shared/service-proxies/service-proxies';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { PrimefacesDropDownObject } from '@app/app.component';

@Component({
  templateUrl: './register.component.html',
  animations: [accountModuleAnimation()]
})
export class RegisterComponent extends AppComponentBase implements OnInit {
   model: RegisterInput = new RegisterInput();
  createUserRequestDto: CreateUserRequestDto = new CreateUserRequestDto();
  saving = false;
  packageId: number;
  packages: PrimefacesDropDownObject[];
  passwordConfirm: string;
  passwordMatch = false;
  passwordValidationMessage: string;
  passordMessage: string;

  constructor(
    injector: Injector,
    private _accountService: AccountServiceProxy,
    private _router: Router,
    private authService: AppAuthService,
    private activatedRoute: ActivatedRoute,
    private _adminService: AdminServiceProxy,
  ) {
    super(injector);
  }

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

  ngOnInit() {
    this.packageId = parseInt(this.activatedRoute.snapshot.paramMap.get('packageId'));
    this.packages = this.customPackages.map(item =>
      ({
        label: item.name,
        value: item.id
      }));
  }

  // save(): void {
  //   this.saving = true;
  //   //this.createUserRequestDto.packageId = this.packageId
  //   this._accountService
  //     .register(this.model)
  //     .pipe(
  //       finalize(() => {
  //         this.saving = false;
  //       })
  //     )
  //     .subscribe((result: RegisterOutput) => {
  //       if (!result.canLogin) {
  //         this.notify.success(this.l('SuccessfullyRegistered'));
  //         this._router.navigate(['/login']);
  //         return;
  //       }

  //       // Autheticate
  //       this.saving = true;
  //       this.authService.authenticateModel.userNameOrEmailAddress = this.model.userName;
  //       this.authService.authenticateModel.password = this.model.password;
  //       this.authService.authenticate(() => {
  //         this.saving = false;
  //       });
  //     });
  // }

  save(): void {
    this.saving = true;
    this.createUserRequestDto.packageId = this.packageId
    this._adminService
      .createOrEdit(this.createUserRequestDto)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe((result) => {
        this._router.navigate(['']);
        this.notify.success(this.l('SuccessfullyRegistered'));
        // if (!result.canLogin) {
        //   this.notify.success(this.l('SuccessfullyRegistered'));
        //   this._router.navigate(['/login']);
        //   return;
        // }

        // // Autheticate
        // this.saving = true;
        // this.authService.authenticateModel.userNameOrEmailAddress = this.model.userName;
        // this.authService.authenticateModel.password = this.model.password;
        // this.authService.authenticate(() => {
        //   this.saving = false;
        // });
      });
  }

  checkPassword(str) {
    // at least one number, one lowercase and one uppercase letter
    // at least six characters
    var re = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}/;
    return re.test(str);
  }


  confirmPassword() {
    debugger;
    if (this.checkPassword(this.createUserRequestDto.password)) {
      if (this.passwordConfirm == this.createUserRequestDto.password) {
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
}
