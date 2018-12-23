import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../app.configuration';
import { UsersModel } from '../../models/admin-models/admin.users.model';
import { TimeSheetMasterModel } from '../../models/admin-models/admin.timesheet-master.model';
import { TimeSheetData } from '../../models/admin-models/admin.timesheetdata';

@Injectable()
export class AdminTimeSheetService {
    public apiUrl: string;
    constructor(private http: HttpClient, private config: AppConfig) {
        var appconfig = this.config ;
        this.apiUrl = appconfig.getApi() + 'api/AdminTimeSheet/';
    }
    LoadTimeSheetData(userId: number) {
        var url = this.apiUrl + 'LoadTimeSheetData?userId=' + userId;
        return this.http.get<any>(url);
    }
    LoadTimeSheetDataByStatus(userId: number, status: number) {
        var url = this.apiUrl + 'LoadTimeSheetData?userId=' + userId + "&timesheetStatus=" + status;
        return this.http.get<Array<TimeSheetMasterModel>>(url);
    }
    GetTimeSheetByMasterId(TimesheetMasterId: number) {
        var url = this.apiUrl + 'GetTimeSheetByMasterId?TimesheetMasterId=' + TimesheetMasterId;
        return this.http.get<TimeSheetData>(url);
    }
    GetTimeSheetsByStatus(userId: number, timesheetStatus: number) {
        var url = this.apiUrl + 'GetTimeSheetsByStatus?userId=' + userId + "&timesheetStatus=" + timesheetStatus;
        return this.http.get<Array<UsersModel>>(url);
    }
    Rejected(data: any) {
        var url = this.apiUrl + 'Rejected' ;
        return this.http.post<any>(url, data);
    }
    Approval(data: any) {
        var url = this.apiUrl + 'Approval';
        return this.http.post<any>(url, data);
    }
}