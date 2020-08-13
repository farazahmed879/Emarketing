import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PublicComponent } from './public.component';
import { UserPackageComponent } from './user-Packages/user-Packages.component';
import { HomeComponent } from './home/home.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: PublicComponent,
                children: [
                    { path: 'user-packages', component: UserPackageComponent },
                    { path: 'home', component: HomeComponent }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class PublicRoutingModule { }
