import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { AdminTimesheetComponent } from './timesheet/timesheet.component';
import { TeamComponent } from './team/team.component';
import { ProjectListComponent } from './projects-list/projects-list.component';
import { AssignProjectsComponent } from './assign-projects/assign-projects.component';
import { UserTaskListComponent } from './tasks-list/user-task-list.component';
import { TaskComponent } from './task/task.component';
import { RemoveProjectComponent } from './remove-project/remove-project.component';
import { DocumentComponent } from './add-document/add-document.component';
import { FileUploadModule } from 'ng2-file-upload';
import { DocumentListComponent } from './documents/documents.component';

const routes: Routes = [
    { path: '', redirectTo: 'dashboard' ,  pathMatch: 'full'},
    { path: 'dashboard', component: DashboardComponent },
    { path: 'projects-list', component: ProjectListComponent },
    { path: 'team', component: TeamComponent },
    { path: 'timesheet/:timesheetmasterId', component: AdminTimesheetComponent },
    { path: 'assign-projects', component: AssignProjectsComponent },
    { path: 'task', component: TaskComponent },
    { path: 'task/:Id', component: TaskComponent },
    { path: 'task/:Id/:projectId', component: TaskComponent },
    { path: 'tasks/:projectId', component: UserTaskListComponent },
    { path: 'remove-project', component: RemoveProjectComponent },
    { path: 'add-document', component: DocumentComponent },
    { path: 'documents', component: DocumentListComponent }    
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        SharedModule.forRoot(),
        FileUploadModule
    ],
    declarations: [
        DashboardComponent,
        TeamComponent,
        AdminTimesheetComponent,
        ProjectListComponent,
        AssignProjectsComponent,
        UserTaskListComponent,
        TaskComponent,
        RemoveProjectComponent,
        DocumentComponent,
        DocumentListComponent
    ],
    exports: [
        RouterModule      
    ]
})
export class AdminModule { }
