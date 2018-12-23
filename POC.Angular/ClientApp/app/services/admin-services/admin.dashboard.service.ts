import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../app.configuration';
import { UsersModel } from '../../models/admin-models/admin.users.model';

@Injectable()
export class AdminDashboardService {
    public apiUrl: string;
    constructor(private http: HttpClient, private config: AppConfig) {
        var appconfig = this.config ;
        this.apiUrl = appconfig.getApi() + 'api/AdminDashboard/';
    }
    Dashboard(userId: number) {
        var url = this.apiUrl + 'Dashboard?userId=' + userId;
        return this.http.get<any>(url);
    }
    GetTeamByAdminId(userId: number) {
        var url = this.apiUrl + 'GetTeamByAdminId?userId=' + userId;
        return this.http.get<Array<UsersModel>>(url);
    }
    
    
    
   
}