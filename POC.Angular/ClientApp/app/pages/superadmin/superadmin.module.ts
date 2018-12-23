import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { Routes, RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';//for table pagination


import { DashboardComponent } from './dashboard/dashboard.component';
import { UsersListComponent } from './users-list/users-list.component';
import { AdminUsersListComponent } from './admin-list/admin-list.component';
import { ProjectListComponent } from './projects-list/projects-list.component';
import { UserComponent } from './user/user.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { ProjectComponent } from './project/project.component';
import { RemoveManagerComponent } from './remove-manager/remove-manager.component';
import { AssignManagersComponent } from './assign-managers/assign-managers.component';
import { RolesComponent } from './roles-list/roles-list.component';
import { RoleComponent } from './role/role.component';
import { AssignProjectsComponent } from './assign-projects/assign-projects.component';
import { RemoveProjectComponent } from './remove-project/remove-project.component';

const routes: Routes = [
    { path: '', redirectTo: 'dashboard' ,  pathMatch: 'full'},
    { path: 'dashboard', component: DashboardComponent },  
    { path: 'user', component: UserComponent },    
    { path: 'user/:Id', component: EditUserComponent },    
    { path: 'users-list', component: UsersListComponent },
    { path: 'project', component: ProjectComponent },   
    { path: 'role', component: RoleComponent },
    { path: 'role/:Id', component: RoleComponent },
    { path: 'roles-list', component: RolesComponent },
    { path: 'project/:Id', component: ProjectComponent },    
    { path: 'projects-list', component: ProjectListComponent },
    { path: 'assign-projects', component: AssignProjectsComponent },
    { path: 'admin-list', component: AdminUsersListComponent },
    { path: 'assign-managers', component: AssignManagersComponent },
    { path: 'remove-manager', component: RemoveManagerComponent },
    { path: 'remove-project', component: RemoveProjectComponent },
    
];
@NgModule({
    imports: [
        RouterModule.forChild(routes),
        SharedModule.forRoot(),        
        NgxPaginationModule,
    ],
    declarations: [
        DashboardComponent,
        UsersListComponent,
        ProjectComponent,
        ProjectListComponent,
        AdminUsersListComponent,
        RemoveManagerComponent,
        UserComponent,
        EditUserComponent,
        AssignManagersComponent,
        AssignProjectsComponent,
        RoleComponent,
        RolesComponent,
        RemoveProjectComponent              
    ],
    exports: [
        RouterModule
        ]
})
export class SuperAdminModule { }
