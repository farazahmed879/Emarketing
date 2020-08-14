import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { WithdrawRequestDto, WithdrawRequestServiceProxy, CreateWithdrawRequestDto, UserWithdrawDetailServiceProxy, UserPackageAdDetailServiceProxy } from '@shared/service-proxies/service-proxies';
import { SelectItem } from 'primeng/api';
import { PrimefacesDropDownObject } from '@app/app.component';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { CountdownComponent  } from 'ngx-countdown';

interface City {
  name: string;
  id: number;
}

@Component({
  templateUrl: './view-ads.component.html',
  animations: [appModuleAnimation()]
})
export class ViewAds extends AppComponentBase implements OnInit {
  // @ViewChild('cd', { static: false }) private countdown: CountdownComponent;
  // this.countdown.begin();
  //cities1: SelectItem[];
  config: any;
  createWithdrawRequestDto: WithdrawRequestDto;
  //selectedWithdrawType: City;
  url: string = "https://player.vimeo.com/video/70591644?autoplay=true&showinfo=0&controls=0";
  videoUrl: any;
  id: number;
  constructor(injector: Injector,
    public _userPackageAdDetailServiceProxy: UserPackageAdDetailServiceProxy,
    private activatedRoute: ActivatedRoute,
    public sanitizer: DomSanitizer) {
    super(injector);

  }


  ngOnInit() {
    this.id = parseInt(this.activatedRoute.snapshot.paramMap.get('adId'));
    this.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.url);  
   // this.getAdsUrlById();
  }

  getAdsUrlById() {

    this._userPackageAdDetailServiceProxy.getById(this.id).subscribe((result)=>{

    })
  }

  create() {
  }
  back() {

  }

  handleEvent(event){

  }
}
