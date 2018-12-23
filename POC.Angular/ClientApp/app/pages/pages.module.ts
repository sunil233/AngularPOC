import { NgModule } from '@angular/core';
import {  RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './login/login.component';
import { SuperAdminAuthGuardService as SuperAdminGuard } from './guards/SuperAdminAuthGuardService';
import { AdminAuthGuardService as AdminGuard } from './guards/AdminAuthGuardService';
import { UserAuthGuardService as UserAuthGuard } from './guards/UserAuthGuardService';
import { pageroutes } from './page.routes';

@NgModule({
    imports: [
        SharedModule,      
        RouterModule.forRoot(pageroutes)
    ],
    declarations: [LoginComponent],
    exports: [
        RouterModule
    ],
    providers: [SuperAdminGuard, AdminGuard, UserAuthGuard],
})
export class PagesModule { }
