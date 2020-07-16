import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PackageServiceProxy, PackageDto } from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './user-Packages.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserPackageComponent extends AppComponentBase
implements OnInit {
  constructor(injector: Injector,private _packageService: PackageServiceProxy) {
    super(injector);
  }

  packages: PackageDto[];

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
}
