import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AccountRoutingModule } from './account-routing.module';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { AccountComponent } from './account.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountLanguagesComponent } from './layout/account-languages.component';
import { AccountHeaderComponent } from './layout/account-header.component';
import { AccountFooterComponent } from './layout/account-footer.component';

// tenants
import { TenantChangeComponent } from './tenant/tenant-change.component';
import { TenantChangeDialogComponent } from './tenant/tenant-change-dialog.component';
//User Package
import { UserPackageComponent } from './user-Packages/user-Packages.component';
import { CreatePackageRequestComponent } from './user-Packages/create-package-request/create-package-request.component';
import { UserRequestServiceProxy, PackageServiceProxy } from '@shared/service-proxies/service-proxies';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        SharedModule,
        ServiceProxyModule,
        AccountRoutingModule,
        ModalModule.forChild()
    ],
    providers: [
        UserRequestServiceProxy,
        PackageServiceProxy
    ],
    declarations: [
        AccountComponent,
        LoginComponent,
        RegisterComponent,
        AccountLanguagesComponent,
        AccountHeaderComponent,
        AccountFooterComponent,
        // tenant
        TenantChangeComponent,
        TenantChangeDialogComponent,
        //User Package
        UserPackageComponent,
        CreatePackageRequestComponent
    ],
    entryComponents: [
        // tenant
        TenantChangeDialogComponent,
        //User Package
        UserPackageComponent,
        CreatePackageRequestComponent
    ]
})
export class AccountModule {

}
