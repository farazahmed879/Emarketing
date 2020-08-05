import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PublicComponent } from './public.component';
import { UserPackageComponent } from './user-Packages/user-Packages.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: PublicComponent,
                children: [
                    { path: 'user-packages', component: UserPackageComponent }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class PublicRoutingModule { }
