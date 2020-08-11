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
import { UserRequestComponent } from './user-request/user-request.component';
import { UserReferalComponent } from './user-referal/user-referal.component';
import { UserReferalRequestComponent } from './user-referal-request/user-referal-request.component';
import { UserPackageDetailComponent } from './user-package-detail/user-package-detail.component';

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
                    { path: 'update-password', component: ChangePasswordComponent, canActivate: [AppRouteGuard]},
                    { path: 'user-profile', component: UserProfileComponent, canActivate: [AppRouteGuard] },
                    { path: 'user-packages', component: UserPackageComponent, canActivate: [AppRouteGuard] },
                    { path: '', component: HomeComponent },
                    { path: 'packageAd/:packageId', component: PackagesAdvertisementComponent, canActivate: [AppRouteGuard] },
                    { path: 'user-request', component: UserRequestComponent, canActivate: [AppRouteGuard] },
                    { path: 'user-referal', component: UserReferalComponent, canActivate: [AppRouteGuard] },
                    { path: 'user-referal-request', component: UserReferalRequestComponent, canActivate: [AppRouteGuard] },
                    { path: 'user-package-detail', component: UserPackageDetailComponent, canActivate: [AppRouteGuard] },
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
