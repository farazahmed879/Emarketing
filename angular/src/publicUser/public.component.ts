import {
  Component,
  OnInit,
  ViewEncapsulation,
  Injector,
  Renderer2
} from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Router } from '@angular/router';

@Component({
  templateUrl: './public.component.html',
  encapsulation: ViewEncapsulation.None
})
export class PublicComponent extends AppComponentBase implements OnInit {
  constructor(injector: Injector, private renderer: Renderer2,     private router: Router) {
    super(injector);
    if(this.appSession && this.appSession.userId){
      this.router.navigate(['/app/dashboard']);
    }
  }

  showTenantChange(): boolean {
    return abp.multiTenancy.isEnabled;
  }

  ngOnInit(): void {
  
  }
}
