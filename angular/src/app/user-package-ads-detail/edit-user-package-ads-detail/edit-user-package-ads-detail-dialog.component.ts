import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
  Optional,
  Inject
} from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import * as _ from 'lodash';
import { AppComponentBase } from '@shared/app-component-base';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import {
  UserPackageAdDetailServiceProxy,
  UserPackageAdDetailDto
} from '@shared/service-proxies/service-proxies';
//const { getVideoDurationInSeconds } = require('get-video-duration')

@Component({
  templateUrl: './edit-user-package-ads-detail-dialog.component.html',
  styleUrls: [
    './edit-user-package-ads-detail-dialog.component.css'
],
})
export class EditUserPackageAdsDetailComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  userPackageAdDetail = new UserPackageAdDetailDto();
  id: number;
  packageId: number;
  url: string = "https://player.vimeo.com/video/70591644?autoplay=true&showinfo=0&controls=0";
  videoUrl: any;
  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _userPackageAdDetailService: UserPackageAdDetailServiceProxy,
    public bsModalRef: BsModalRef,
    private _modalService: BsModalService,
    public sanitizer: DomSanitizer
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.show();
    // getVideoDurationInSeconds('https://player.vimeo.com/video/70591644?autoplay=true&showinfo=0&controls=0').then((duration) => {
    //   console.log("duration",duration);
    // });
    this.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.url);  
  }

  show() {
    this.packageId;
    this._userPackageAdDetailService.getById(this.id).subscribe((result) => {
      this.userPackageAdDetail = result;
    }
    )
  }




  save(): void {
    this.saving = true;

    this._userPackageAdDetailService
      .createOrEdit(this.userPackageAdDetail)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit();
      });
  }

  getDurrection(){
    debugger;
    // var myVideoPlayer = document.getElementById('https://www.w3schools./html/movie.mp4');
    // var duration = myVideoPlayer.duration;
  }

}
