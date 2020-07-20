import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PackageServiceProxy, PackageDto } from '@shared/service-proxies/service-proxies';
import {Router} from '@angular/router';

@Component({
  templateUrl: './user-Packages.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserPackageComponent extends AppComponentBase
implements OnInit {
  constructor(injector: Injector,private _packageService: PackageServiceProxy,private router: Router) {
    super(injector);
  }

  packages: PackageDto[];
  menuitem: [
    {"value": "New", "onclick": "CreateNewDoc()"},
    {"value": "Open", "onclick": "OpenDoc()"},
    {"value": "Close", "onclick": "CloseDoc()"}
  ];
   HEROES = [
    {id: 1, name:'Superman'},
    {id: 2, name:'Batman'},
    {id: 5, name:'BatGirl'},
    {id: 3, name:'Robin'},
    {id: 4, name:'Flash'}
];

  ngOnInit(){
   this.getAllPackages();
  }

  getAllPackages(){
    this._packageService.getAll().subscribe((result)=>{
      if(result){
        this.packages = result;
        console.log("this.packages", this.packages);
      }
    })
  }

  selectPackage(){
    this.router.navigateByUrl('/account/packageRequest');
  }
}
