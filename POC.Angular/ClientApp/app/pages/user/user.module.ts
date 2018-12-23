import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TimeSheetComponent } from './timesheet/timesheet.component';
import { UserTimeSheetsComponent } from './timesheets/timesheets.component';
import { UserProjectListComponent } from './projects-list/user-projects-list.component';
import { UserTaskListComponent } from './tasks-list/user-task-list.component';
import { TaskComponent } from './task/task.component';

const routes: Routes = [
    { path: '', redirectTo: 'dashboard' ,  pathMatch: 'full'},
    { path: 'dashboard', component: DashboardComponent },
    { path: 'timesheet', component: TimeSheetComponent },
    { path: 'timesheet/:timesheetmasterId', component: TimeSheetComponent },
    { path: 'timesheets', component: UserTimeSheetsComponent },
    { path: 'timesheets/:timesheetmasterId', component: UserTimeSheetsComponent },
    { path: 'project-list', component: UserProjectListComponent },
    { path: 'task', component: TaskComponent },
    { path: 'task/:Id', component: TaskComponent },
    { path: 'task/:Id/:projectId', component: TaskComponent },
    { path: 'tasks/:projectId', component: UserTaskListComponent }    
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        SharedModule.forRoot()
    ],
    declarations: [
        DashboardComponent,
        TimeSheetComponent,
        UserTimeSheetsComponent,
        UserProjectListComponent,
        TaskComponent,
        UserTaskListComponent
    ],
    exports: [
        RouterModule
    ]
})
export class UserModule { }
