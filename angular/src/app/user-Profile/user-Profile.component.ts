import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { UserServiceProxy, UserDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  templateUrl: './user-profile.component.html',
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserProfileComponent extends AppComponentBase {
  user = new UserDto();
  id: number;
  saving = false;
  constructor(injector: Injector, public _userService: UserServiceProxy) {
    super(injector);


  }

  ngOnInit(): void {
    this._userService.get(this.appSession.userId).subscribe((result) => {
      this.user = result;
    });
  }

  save() {
    this.saving = true;
    this._userService
      .update(this.user)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
      });
  }
}
