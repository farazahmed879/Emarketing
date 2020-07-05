import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateWithdrawRequestDto } from '@shared/service-proxies/service-proxies';
import {SelectItem} from 'primeng/api';

interface City {
  name: string;
  code: string;
}

@Component({
  templateUrl: './withdraw-request.component.html',
  styles: [
    `
      mat-form-field {
        width: 100%;
      }
      mat-checkbox {
        padding-bottom: 5px;
      }
    `
  ],
  animations: [appModuleAnimation()]
})
export class WithdrawRequestComponent extends AppComponentBase implements OnInit {
  constructor(injector: Injector) {
    super(injector);
    this.cities1 = [
      {label:'Select City', value:null},
      {label:'New York', value:{id:1, name: 'New York', code: 'NY'}},
      {label:'Rome', value:{id:2, name: 'Rome', code: 'RM'}},
      {label:'London', value:{id:3, name: 'London', code: 'LDN'}},
      {label:'Istanbul', value:{id:4, name: 'Istanbul', code: 'IST'}},
      {label:'Paris', value:{id:5, name: 'Paris', code: 'PRS'}}
  ];
  }
  cities1: SelectItem[];
  createWithdrawRequestDto: CreateWithdrawRequestDto;
  withdrawTypes = [{name: "Other", id: 1}, {name: "EasyPaisa", id: 2}];
  statuses = [{name: "Other", id: 1}, {name: "EasyPaisa", id: 2}];
  ngOnInit(){

  }

  create(){

    
  }
}
