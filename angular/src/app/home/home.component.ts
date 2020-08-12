import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { DashboardServiceProxy, GetUserCurrentSubscriptionStatsDto } from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './home.component.html',
  animations: [appModuleAnimation()],
})
export class HomeComponent extends AppComponentBase implements OnInit {

  constructor(injector: Injector,
    public _dashboardService: DashboardServiceProxy) {

    super(injector);
  }

  currentSubscriptionStats: GetUserCurrentSubscriptionStatsDto;


  ngOnInit(): void {
    var GetUserCurrentSubscriptionStatsDto = new GetUserCurrentSubscriptionStatsDto();
    this._dashboardService.getUserCurrentSubscriptionStats(GetUserCurrentSubscriptionStatsDto).subscribe((result) => {
      this.currentSubscriptionStats = result;
      console.log("currentSubscriptionStats", result);

    })
  }
}
