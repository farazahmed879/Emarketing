import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { WithdrawRequestDto, WithdrawRequestServiceProxy, CreateWithdrawRequestDto, CreateUserRequestDto, UserRequestServiceProxy } from '@shared/service-proxies/service-proxies';
import { SelectItem } from 'primeng/api';
import { Router, ActivatedRoute } from '@angular/router';

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

  HEROES = [
    { value: 1, label: 'Superman' },
    { value: 2, label: 'Batman' },
    { value: 5, label: 'BatGirl' },
    { value: 3, label: 'Robin' },
    { value: 4, label: 'Flash' }
  ];
  constructor(injector: Injector,
    private _userRequestService: UserRequestServiceProxy,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {
    super(injector);

  }


  ngOnInit() {
    this.packageId = parseInt(this.activatedRoute.snapshot.paramMap.get('packageId'));
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


    this._userRequestService.createOrEdit(createUserRequestDto).subscribe((result) => {
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
    debugger;
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email)) {
      this.emailValidationMessage = ""
    }
    this.emailValidationMessage = "Please prove correct email address";
  }
}

// [disbaled]="!firstName || !lastName || !userName || !email || !password || !packageId || !passwordConfirm"