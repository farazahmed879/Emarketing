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
  userNameValidationMessage: string;
  userNameMessage: string;

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
      id: 1, name: 'Package 2500',
      isUnlimited: false,
      durationInDays: 30,
      price: 2500,
      totalEarning: 5010,
      profitValue: 0,
      dailyAdCount: 1,
      pricePerAd: 167,
      referralAmount: 200,
      minimumWithdraw: 500,
      maximumWithdraw: 1000
    },
    {
      id: 2,
      name: 'Package 3000',
      isUnlimited: false,
      durationInDays: 30,
      price: 3000,
      totalEarning: 6300,
      profitValue: 0,
      dailyAdCount: 1,
      pricePerAd: 215,
      referralAmount: 200,
      minimumWithdraw: 500,
      maximumWithdraw: 3500
    },
    // {
    //   id: 3,
    //   name: 'Package 3',
    //   isUnlimited: true,
    //   durationInDays: 30,
    //   price: 5000,
    //   totalEarning: 11040,
    //   profitValue: 0,
    
    //   dailyAdCount: 5,
    //   pricePerAd: 73.6,
    //   referralAmount: 300,
    //   minimumWithdraw: 500,
    //   maximumWithdraw: 1000

    // profitValue: 0,
    // dailyAdCount: 5,
    // pricePerAd: 36.8,
    // referralAmount: 700,
    // minimumWithdraw: 1500,
    // maximumWithdraw: 2000
    // },
    {
      id: 4,
      name: 'Package 5000',
      isUnlimited: false,
      durationInDays: 30,
      price: 5000,
      totalEarning: 11040,
      profitValue: 0,
      dailyAdCount: 1,
      pricePerAd: 368,
      referralAmount: 300,
      minimumWithdraw: 500,
      maximumWithdraw: 3500
    },
    {
      id: 5,
      name: 'Package 10000',
      isUnlimited: false,
      durationInDays: 30,
      price: 10000,
      totalEarning: 18000,
      profitValue: 0,
      dailyAdCount: 1,
      pricePerAd: 600,
      referralAmount: 500,
      minimumWithdraw: 1200,
      maximumWithdraw: 5000
    },
    {
      id: 6,
      name: 'Package 1200',
      isUnlimited: false,
      durationInDays: 21,
      price: 1200,
      totalEarning: 2310,
      profitValue: 0,
      dailyAdCount: 1,
      pricePerAd: 110,
      referralAmount: 150,
      minimumWithdraw: 220,
      maximumWithdraw: 1000
    },
    {
      id: 7,
      name: 'Package 1000',
      isUnlimited: false,
      durationInDays: 45,
      price: 1000,
      totalEarning: 2025.00,
      profitValue: 0,
      dailyAdCount: 1,
      pricePerAd: 45.00,
      referralAmount: 200.00,
      minimumWithdraw:325.00 ,
      maximumWithdraw: 500.00
    },
    {
      id: 3,
      name: 'Package 500',
      isUnlimited: false,
      durationInDays: 7,
      price: 500,
      totalEarning: 700,
      profitValue: 0,
      dailyAdCount: 1,
      pricePerAd: 100,
      referralAmount: 50.00,
      minimumWithdraw: '-' ,
      maximumWithdraw: '-'
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
   
    if (!this.checkPassword(this.createUserRequestDto.password)) {    
      this.passordMessage = "at least one number, one lowercase and one uppercase letter, at least six characters";
      return;
    }
    if (!this.ValidateUserName(this.createUserRequestDto.userName)) {    
      this.userNameMessage = "Username must be without space.";
      return;
    }
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
    var re = /^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$/;
    return re.test(str);
  }

  onConfirmPasswordChange(){
    this.matchPassword();
  }
  onPasswordChange(){
    this.matchPassword();
  }


  matchPassword() {
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
      this.passordMessage = "Minimum eight characters, at least one capital letter, one number and one special character:";
    }
  } 
  ValidateUserName(str) {
    // at least one number, one lowercase and one uppercase letter
    // at least six characters
    var re = /^([A-z0-9!@$%^&*().,<>{}[\]<>?_=+\-|;:\'\"\/])*[^\s]\1*$/;
    return re.test(str); 
     
     
  }

  onUserNameChange(){
    this.onChangeValidateUserName();
  }


  onChangeValidateUserName() {
    if (this.ValidateUserName(this.createUserRequestDto.userName)) {
      this.userNameMessage = "";
    } else {
      this.userNameMessage = "User name must be without space.";
    }
  } 
}
