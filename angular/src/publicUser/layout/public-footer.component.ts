import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'public-footer',
  templateUrl: './public-footer.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PublicFooterComponent extends AppComponentBase {
  currentYear: number;
  versionText: string;

  constructor(injector: Injector) {
    super(injector);

    this.currentYear = new Date().getFullYear();
    this.versionText =
      this.appSession.application.version +
      ' [' +
      this.appSession.application.releaseDate.format('YYYYDDMM') +
      ']';
  }
}
