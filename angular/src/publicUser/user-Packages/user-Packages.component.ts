import { Component, Injector, ChangeDetectionStrategy, OnInit, Renderer2 } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PackageServiceProxy, PackageDto, AdminServiceProxy, UserPersonalDetailServiceProxy } from '@shared/service-proxies/service-proxies';
import { Router } from '@angular/router';

@Component({
  templateUrl: './user-Packages.component.html',
  styleUrls: [
    './user-Packages.component.css'
],
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserPackageComponent extends AppComponentBase
  implements OnInit {
  constructor(injector: Injector,
    private _adminService: AdminServiceProxy,
    private _UserPersonalDetailServiceProxy: UserPersonalDetailServiceProxy,
    private router: Router, private renderer: Renderer2) {
    super(injector);
  }

  packages: PackageDto[] = [];
  menuitem: [
    { "value": "New", "onclick": "CreateNewDoc()" },
    { "value": "Open", "onclick": "OpenDoc()" },
    { "value": "Close", "onclick": "CloseDoc()" }
  ];
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
    }
  ];

  ngOnInit() {
    this.getPackageList();
    this.customPackages = this.customPackages.filter(i => i.isUnlimited == false);
    this.renderer.removeClass(document.body, 'login-page');
  }
  getPackageList() {
    this._adminService.getAll().subscribe((result: any) => {
      console.log("result", result);
      if (result) {
        this.packages = result;

      }
    });
  }


}
