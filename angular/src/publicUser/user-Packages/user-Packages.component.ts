import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PackageServiceProxy, PackageDto, AdminServiceProxy, UserPersonalDetailServiceProxy } from '@shared/service-proxies/service-proxies';
import { Router } from '@angular/router';

@Component({
  templateUrl: './user-Packages.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserPackageComponent extends AppComponentBase
  implements OnInit {
  constructor(injector: Injector,
    private _adminService: AdminServiceProxy,
    private _UserPersonalDetailServiceProxy: UserPersonalDetailServiceProxy,
    private router: Router) {
    super(injector);
  }

  packages: PackageDto[] = [];
  menuitem: [
    { "value": "New", "onclick": "CreateNewDoc()" },
    { "value": "Open", "onclick": "OpenDoc()" },
    { "value": "Close", "onclick": "CloseDoc()" }
  ];
  HEROES = [
    { id: 1, name: 'Superman' },
    { id: 2, name: 'Batman' },
    { id: 5, name: 'BatGirl' },
    { id: 3, name: 'Robin' },
    { id: 4, name: 'Flash' }
  ];

  ngOnInit() {
    this.getPackageList();

  }
  getPackageList() {
    this._adminService.getAll().subscribe((result: any) => {
      console.log("result",result);
      debugger;
      if (result) {
        this.packages = result;
        
      }
    });
  }


}
