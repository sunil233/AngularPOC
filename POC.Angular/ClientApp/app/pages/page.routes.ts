import { Routes } from '@angular/router';
import { AdminLayoutComponent } from '../layout/admin-layout/admin-layout.component';
import { SuperAdminLayoutComponent } from '../layout/superadmin-layout/superadmin-layout.component';
import { LoginComponent } from './login/login.component';
import { UserLayoutComponent } from '../layout/user-layout/user-layout.component';
import { SuperAdminAuthGuardService as SuperAdminGuard } from './guards/SuperAdminAuthGuardService';
import { AdminAuthGuardService as AdminGuard} from './guards/AdminAuthGuardService';
import { UserAuthGuardService as UserAuthGuard} from './guards/UserAuthGuardService';

//Lazy Loading Feature Modules
export const pageroutes: Routes = [
   //admin routes goes here
    {
        path: '',
        component: AdminLayoutComponent,
        canActivate: [AdminGuard], 
        children: [
            { path: '', redirectTo: 'login', pathMatch: 'full' }, 
            { path: 'admin', loadChildren: './admin/admin.module#AdminModule' }                 
        ],
        
    },
    //super admin routes goes here 
    {
        path: '',
        canActivate: [SuperAdminGuard], 
        component: SuperAdminLayoutComponent,
        children: [
            { path: 'superadmin', loadChildren: './superadmin/superadmin.module#SuperAdminModule' }
        ]
    },

    //user routes goes here 
    {
        path: '',
        component: UserLayoutComponent,
        canActivate: [UserAuthGuard], 
        children: [
            { path: 'user', loadChildren: './user/user.module#UserModule' }
        ]
    },

    // Not lazy-loaded routes
    { path: 'login', component: LoginComponent },
    // Not found
    { path: '**', redirectTo: '' }
];
