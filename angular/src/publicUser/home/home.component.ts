import { Component, Injector, ChangeDetectionStrategy, OnInit, Renderer2 } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { WithdrawRequestServiceProxy, CreateWithdrawRequestDto, CreateUserRequestDto, UserRequestServiceProxy } from '@shared/service-proxies/service-proxies';
import { SelectItem } from 'primeng/api';
import { Router, ActivatedRoute } from '@angular/router';
import { PrimefacesDropDownObject } from '@app/app.component';

interface City {
  name: string;
  id: number;
}

@Component({
  templateUrl: './home.component.html',
  styleUrls: [
    './home.component.css'
],
  animations: [appModuleAnimation()]
})
export class HomeComponent extends AppComponentBase implements OnInit {
  //cities1: SelectItem[];




  constructor(injector: Injector,private renderer: Renderer2
  ) {
    super(injector);

  }

  ngOnInit() {
    this.renderer.removeClass(document.body, 'login-page');
  }

}


