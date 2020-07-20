import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountComponent } from './account.component';
import { UserPackageComponent } from './user-Packages/user-Packages.component';
import { CreatePackageRequestComponent } from './user-Packages/create-package-request/create-package-request.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AccountComponent,
                children: [
                    { path: 'login', component: LoginComponent },
                    { path: 'register', component: RegisterComponent },
                    { path: 'userPackage', component: UserPackageComponent,  },
                    { path: 'packageRequest', component: CreatePackageRequestComponent, },
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class AccountRoutingModule { }
