import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { WithdrawRequestDto, WithdrawRequestServiceProxy, CreateWithdrawRequestDto, CreateUserRequestDto, UserRequestServiceProxy } from '@shared/service-proxies/service-proxies';
import { SelectItem } from 'primeng/api';
import {Router} from '@angular/router';

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
  constructor(injector: Injector,
    private _userRequestService: UserRequestServiceProxy,
    private router: Router
    ) {
    super(injector);

  }


  ngOnInit() {

  }

  create() {
    debugger;
    var createUserRequestDto = new CreateUserRequestDto;
    createUserRequestDto.firstName = this.firstName;
    createUserRequestDto.lastName = this.lastName;
    createUserRequestDto.userName = this.userName;
    createUserRequestDto.email = this.email;
    createUserRequestDto.phoneNumber = this.phoneNumber;
    createUserRequestDto.password = this.password;

    this._userRequestService.createOrEdit(createUserRequestDto).subscribe((result) => {
      if (result) {
        this.notify.info(this.l('SavedSuccessfully'));
        this.router.navigateByUrl('/account/userPackage');
      }

    })

  }
  back() {

  }
}
