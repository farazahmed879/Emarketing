import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxPaginationModule } from 'ngx-pagination';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { HomeComponent } from '@app/home/home.component';
import { AboutComponent } from '@app/about/about.component';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
// tenants
import { TenantsComponent } from '@app/tenants/tenants.component';
import { CreateTenantDialogComponent } from './tenants/create-tenant/create-tenant-dialog.component';
import { EditTenantDialogComponent } from './tenants/edit-tenant/edit-tenant-dialog.component';
// roles
import { RolesComponent } from '@app/roles/roles.component';
import { CreateRoleDialogComponent } from './roles/create-role/create-role-dialog.component';
import { EditRoleDialogComponent } from './roles/edit-role/edit-role-dialog.component';
// users
import { UsersComponent } from '@app/users/users.component';
import { CreateUserDialogComponent } from '@app/users/create-user/create-user-dialog.component';
import { EditUserDialogComponent } from '@app/users/edit-user/edit-user-dialog.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { ResetPasswordDialogComponent } from './users/reset-password/reset-password.component';
// layout
import { HeaderComponent } from './layout/header.component';
import { HeaderLeftNavbarComponent } from './layout/header-left-navbar.component';
import { HeaderLanguageMenuComponent } from './layout/header-language-menu.component';
import { HeaderUserMenuComponent } from './layout/header-user-menu.component';
import { FooterComponent } from './layout/footer.component';
import { SidebarComponent } from './layout/sidebar.component';
import { SidebarLogoComponent } from './layout/sidebar-logo.component';
import { SidebarUserPanelComponent } from './layout/sidebar-user-panel.component';
import { SidebarMenuComponent } from './layout/sidebar-menu.component';
import { WithdrawRequestComponent } from './withdraw-request/withdraw-request.component';
import { WithdrawHistoryComponent } from './withdraw-history/withdraw-history.component'
import {
  WithdrawRequestServiceProxy,
  PackageServiceProxy,
  UserRequestServiceProxy,
  UserWithdrawDetailServiceProxy,
  UserPersonalDetailServiceProxy,
  PackageAdServiceProxy,
  UserReferralServiceProxy,
  UserReferralRequestServiceProxy,
  UserPackageAdDetailServiceProxy,
  AdminServiceProxy,
  UserPackageAdDetailDto,
  DashboardServiceProxy
} from '@shared/service-proxies/service-proxies';
// Package
import { PackagesComponent } from './packages/packages.component';
import { CreatePackageDialogComponent } from './packages/create-package/create-package-dialog.component';
import { EditPackageDialogComponent } from './packages/edit-package/edit-package-dialog.component';
//User Profile
import { UserProfileComponent } from './user-Profile/user-Profile.component';
//Packages-Advertisement
import { PackagesAdvertisementComponent } from './packages-advertisement/packages-advertisement.component';
import { EditPackagesAdvertisementComponent } from './packages-advertisement/edit-packages-advertisement/edit-package-advertisement-dialog.component';
import { CreatePackagesAdvertisementComponent } from './packages-advertisement/create-packages-advertisement/create-package-advertisement-dialog.component';
// User Request
import { UserRequestComponent } from './user-request/user-request.component';
//User Referal
import { UserReferalComponent } from './user-referal/user-referal.component';
import { ViewUserReferralComponent } from '@app/user-referal/view-user-referral/view-user-referral.component';
//User Referal Request
import { UserReferalRequestComponent } from './user-referal-request/user-referal-request.component';
import { CreateUserReferalRequestDialogComponent } from './user-referal-request/create-user-referal-request/create-user-referal-request-dialog.component';
import { EditUserReferalRequestDialogComponent } from './user-referal-request/edit-user-referal-request/edit-user-referal-request-dialog.component';
//User Package Detail
import { UserPackageAdsDetailComponent } from './user-package-ads-detail/user-package-ads-detail.component';
import { EditUserPackageAdsDetailComponent } from './user-package-ads-detail/edit-user-package-ads-detail/edit-user-package-ads-detail-dialog.component';

import { ViewAds } from './user-package-ads-detail/view-ads/view-ads.component'

//count Down
import { CountdownModule } from 'ngx-countdown';
// import { ViewUserReferralComponent } from './app/user-referal/view-user-referral/view-user-referral.component';
@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AboutComponent,
    // tenants
    TenantsComponent,
    CreateTenantDialogComponent,
    EditTenantDialogComponent,
    // roles
    RolesComponent,
    CreateRoleDialogComponent,
    EditRoleDialogComponent,
    // users
    UsersComponent,
    CreateUserDialogComponent,
    EditUserDialogComponent,
    ChangePasswordComponent,
    ResetPasswordDialogComponent,
    // layout
    HeaderComponent,
    HeaderLeftNavbarComponent,
    HeaderLanguageMenuComponent,
    HeaderUserMenuComponent,
    FooterComponent,
    SidebarComponent,
    SidebarLogoComponent,
    SidebarUserPanelComponent,
    SidebarMenuComponent,
    WithdrawRequestComponent,
    WithdrawHistoryComponent,
    //Packages
    PackagesComponent,
    CreatePackageDialogComponent,
    EditPackageDialogComponent,
    //User Profile
    UserProfileComponent,
    //PAckages-Advertisement
    PackagesAdvertisementComponent,
    EditPackagesAdvertisementComponent,
    CreatePackagesAdvertisementComponent,
    // User Request
    UserRequestComponent,
    //User Referal
    UserReferalComponent,
    // User Referal Request
    UserReferalRequestComponent,
    CreateUserReferalRequestDialogComponent,
    EditUserReferalRequestDialogComponent,
    //User Package Detail
    EditUserPackageAdsDetailComponent,
    UserPackageAdsDetailComponent,
    //ViewAds
    ViewAds,
    ViewUserReferralComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpClientJsonpModule,
    ModalModule.forChild(),
    BsDropdownModule,
    CollapseModule,
    TabsModule,
    AppRoutingModule,
    ServiceProxyModule,
    SharedModule,
    NgxPaginationModule,
    DropdownModule,
    MultiSelectModule,
    CountdownModule
  ],
  providers: [
    WithdrawRequestServiceProxy,
    PackageServiceProxy,
    UserRequestServiceProxy,
    UserWithdrawDetailServiceProxy,
    UserPersonalDetailServiceProxy,
    PackageAdServiceProxy,
    UserReferralServiceProxy,
    UserRequestComponent,
    UserReferralRequestServiceProxy,
    UserPackageAdDetailServiceProxy,
    AdminServiceProxy,
    DashboardServiceProxy
  ],
  entryComponents: [
    // tenants
    CreateTenantDialogComponent,
    EditTenantDialogComponent,
    // roles
    CreateRoleDialogComponent,
    EditRoleDialogComponent,
    // users
    CreateUserDialogComponent,
    EditUserDialogComponent,
    ResetPasswordDialogComponent,
    //Withdraw request
    WithdrawRequestComponent,
    WithdrawHistoryComponent,
    //Package 
    PackagesComponent,
    CreatePackageDialogComponent,
    EditPackageDialogComponent,
    //User Profile
    UserProfileComponent,
    //PAckages-Advertisement
    PackagesAdvertisementComponent,
    EditPackagesAdvertisementComponent,
    CreatePackagesAdvertisementComponent,
    // User Request
    UserRequestComponent,
    //User Referal
    UserReferalComponent,
    // User Referal Request
    UserReferalRequestComponent,
    CreateUserReferalRequestDialogComponent,
    EditUserReferalRequestDialogComponent,
    //User Package Detail
    UserPackageAdDetailDto,
    EditUserPackageAdsDetailComponent,
    //ViewAds
    ViewAds

  ],
})
export class AppModule { }
