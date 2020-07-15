import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { WithdrawRequestDto, WithdrawRequestServiceProxy, CreateWithdrawRequestDto } from '@shared/service-proxies/service-proxies';
import { SelectItem } from 'primeng/api';
//import {DropdownModule} from 'primeng/dropdown';

interface City {
  name: string;
  id: number;
}

@Component({
  templateUrl: './withdraw-request.component.html',
  animations: [appModuleAnimation()]
})
export class WithdrawRequestComponent extends AppComponentBase implements OnInit {
  //cities1: SelectItem[];
  createWithdrawRequestDto: WithdrawRequestDto;
  //selectedWithdrawType: City;
  amount: number;
  status: boolean = false;
  selectedWithdrawTypeId: number;
  //companyArrayObj: PrimefacesDropDownObject[];
  withdrawTypes = [
    { label: 'Select Withdraw Type', value: null },
    { label: 'Others', value: 1 },
    { label: 'Easypaisa', value: 2 },
  ];
  constructor(injector: Injector,
    private _withdrawRequestService: WithdrawRequestServiceProxy) {
    super(injector);

  }


  ngOnInit() {

  }

  create() {
    debugger;
    var createWithdrawRequestDto = new CreateWithdrawRequestDto;
    createWithdrawRequestDto.amount = this.amount;
    createWithdrawRequestDto.withdrawTypeId = this.selectedWithdrawTypeId;
    //createWithdrawRequestDto.status = this.status;
    createWithdrawRequestDto.userId = this.appSession.userId;
    this._withdrawRequestService.createOrEdit(createWithdrawRequestDto).subscribe((result) => {
      if (result) {
        this.notify.info(this.l('SavedSuccessfully'));
      }

    })

  }
  back() {

  }
}
