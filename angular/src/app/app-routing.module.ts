import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { WithdrawRequestComponent } from './withdraw-request/withdraw-request.component';
import { WithdrawHistoryComponent } from './withdraw-history/withdraw-history.component';
import { PackagesComponent } from './packages/packages.component';
import { UserProfileComponent } from './user-Profile/user-Profile.component';
import { UserPackageComponent } from './user-Packages/user-Packages.component';
import { PackagesAdvertisementComponent } from './packages-advertisement/packages-advertisement.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'withdraw-request', component: WithdrawRequestComponent,  canActivate: [AppRouteGuard] },
                    { path: 'withdraw-history', component: WithdrawHistoryComponent,  canActivate: [AppRouteGuard] },
                    { path: 'package', component: PackagesComponent,  canActivate: [AppRouteGuard] },                   
                    { path: 'users', component: UsersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                    { path: 'roles', component: RolesComponent, data: { permission: 'Pages.Roles' }, canActivate: [AppRouteGuard] },
                    { path: 'tenants', component: TenantsComponent, data: { permission: 'Pages.Tenants' }, canActivate: [AppRouteGuard] },
                    { path: 'home', component: HomeComponent },
                    { path: 'about', component: AboutComponent },
                    { path: 'update-password', component: ChangePasswordComponent },
                    { path: 'user-profile', component: UserProfileComponent },
                    { path: 'user-packages', component: UserPackageComponent },
                    { path: 'packageAd/:packageId', component: PackagesAdvertisementComponent },
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
