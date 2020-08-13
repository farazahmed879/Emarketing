import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PublicRoutingModule } from './public-routing.module';
// import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
// import { SharedModule } from '@shared/shared.module';
import { PublicComponent } from './public.component';
import { DropdownModule } from 'primeng/dropdown';
import { UserPackageComponent } from './user-Packages/user-Packages.component';
import { PackageServiceProxy, AdminServiceProxy, UserPersonalDetailServiceProxy } from '@shared/service-proxies/service-proxies';
import { PublicHeaderComponent } from '../publicUser/layout/public-header.component';
import { PublicFooterComponent } from '../publicUser/layout/public-footer.component';
import { HomeComponent } from './home/home.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        PublicRoutingModule,
        // SharedModule,
        // ServiceProxyModule,
        DropdownModule,
        ModalModule.forChild()
    ],
    providers: [
        PackageServiceProxy,
        AdminServiceProxy,
        UserPersonalDetailServiceProxy
    ],
    declarations: [
        PublicComponent,
        UserPackageComponent,
        PublicHeaderComponent,
        PublicFooterComponent,
        HomeComponent
    ],
    entryComponents: [
        UserPackageComponent
    ]
})
export class PublicModule {

}
