import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../app.configuration';
import { TaskModel } from '../../models/user-models/user.task.model';
import { TimeSheetData } from '../../models/user-models/user.timesheetdata';



@Injectable()
export class UserTimeSheetService {
    public apiUrl: string;
    constructor(private http: HttpClient, private config: AppConfig) {
        var appconfig = this.config;
        this.apiUrl = appconfig.getApi() + 'api/TimeSheet/';
    }
    GetTimeSheetByMasterId(TimesheetMasterId: number) {
        var url = this.apiUrl + 'GetTimeSheetByMasterId?TimesheetMasterId=' + TimesheetMasterId;
        return this.http.get<TimeSheetData>(url);
    }
    GetTimeSheetForUserById(userdata:any) {
        var url = this.apiUrl + 'GetTimeSheetForUserById';
        return this.http.post<TimeSheetData>(url, userdata);
    }
    GetProjects() {
        var url = this.apiUrl + 'ListofProjects';
        return this.http.get<any>(url);
    }
    GetTasksByProjects(projectId: number) {
        var url = this.apiUrl + 'ListofTasks?ProjectId=' + projectId;
        return this.http.get<TaskModel>(url);
    }
    SaveTimeSheet(data: TimeSheetData) {
        var url = this.apiUrl + 'SaveTimeSheet';
        return this.http.post<any>(url, data);
    }
    Delete(TimesheetMasterId: number, userId: number) {
        var url = this.apiUrl + 'Delete?TimeSheetMasterID=' + TimesheetMasterId + '&userId=' + userId;;
        return this.http.get<any>(url);
    }
   
}