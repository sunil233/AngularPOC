import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../app.configuration';

@Injectable()
export class UserDashboardService {
    public apiUrl: string;
    constructor(private http: HttpClient, private config: AppConfig) {
        var appconfig = this.config ;
        this.apiUrl = appconfig.getApi() + 'api/UserDashboard/';
    }
    Dashboard(userId: number) {
        var url = this.apiUrl + 'Dashboard?userId=' + userId;
        return this.http.get<any>(url);
    }
    LoadRecentTimeSheets(userId: number) {
        var url = this.apiUrl + 'LoadRecentTimeSheets?userId=' + userId;
        return this.http.get<any>(url);
    }
    GetTimeSheetsByStatus(userId: number, timesheetStatus:number) {
        var url = this.apiUrl + 'GetTimeSheetsByStatus?userId=' + userId + "&timesheetStatus=" + timesheetStatus;
        return this.http.get<any>(url);
    }
   
}