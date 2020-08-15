import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { WithdrawRequestDto, WithdrawRequestServiceProxy, CreateWithdrawRequestDto, UserWithdrawDetailServiceProxy, UserPackageAdDetailServiceProxy, UserPackageAdDetailDto } from '@shared/service-proxies/service-proxies';
import { SelectItem } from 'primeng/api';
import { PrimefacesDropDownObject } from '@app/app.component';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { CountdownComponent } from 'ngx-countdown';
import { Route } from '@angular/compiler/src/core';

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
  userPackageAdDetailDto = new UserPackageAdDetailDto();
  //selectedWithdrawType: City;
  url: string = "https://player.vimeo.com/video/70591644?autoplay=true&showinfo=0&controls=0";
  videoUrl: any;
  id: number;
  public YT: any;
  public video: any;
  public player: any;
  public reframed: any;

  constructor(injector: Injector,
    public _userPackageAdDetailServiceProxy: UserPackageAdDetailServiceProxy,
    private activatedRoute: ActivatedRoute,
    private _router: Router,
    public sanitizer: DomSanitizer) {
    super(injector);

  }


  ngOnInit() {
    this.id = parseInt(this.activatedRoute.snapshot.paramMap.get('adId'));
    this.getAdsUrlById();
  }

  getAdsUrlById() {

    this._userPackageAdDetailServiceProxy.getById(this.id).subscribe((result) => {
      console.log("Ads", result);
      if (result) {
        this.userPackageAdDetailDto = result;
        this.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(result.url)+'?autoplay=true&showinfo=0&controls=0';
        
      }
    })
  }

  create() {
  }
  back() {

  }

  handleEvent(event) {
    if (event.left <= 0) {
      this._userPackageAdDetailServiceProxy.createOrEdit(this.userPackageAdDetailDto).subscribe((result) => {
        if (result) {
          this._router.navigate(['/app/ads']);
        }
      })

    }
  }

}
