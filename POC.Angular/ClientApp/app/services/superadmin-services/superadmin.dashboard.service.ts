import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../app.configuration';
import { UsersModel } from '../../models/superadmin-models/users.model';
import { throwError } from 'rxjs';


@Injectable()
export class SuperAdminDashboardService {
    public apiUrl: string;
    constructor(private http: HttpClient, private config: AppConfig) {
        var appconfig = this.config ;
        this.apiUrl = appconfig.getApi() + 'api/SuperAdmin/';
    }
    Dashboard(userId: number) {
        var url = this.apiUrl + 'Dashboard?userId=' + userId;
        return this.http.get<any>(url);
    }
    GetAllAdminUsers() {
        var url = this.apiUrl + 'GetAllAdminUsers';
        return this.http.get<Array<UsersModel>>(url);
    }
    GetAllUsers() {
        var url = this.apiUrl + 'GetAllUsers';
        return this.http.get<Array<UsersModel>>(url);
    }
    GetUserDetailsById(userId: number) {
        var url = this.apiUrl + 'GetUserDetailsById?RegistrationID=' + userId;
        return this.http.get<UsersModel>(url);
    }
    GetUserDetails() {
        var url = this.apiUrl + 'GetUserDetails';
        return this.http.get<UsersModel>(url);
    }
    SaveUser(userdata: UsersModel) {
        var url = this.apiUrl + 'SaveUser';
        return this.http.post<number>(url, userdata);
           
    }
    onError(res: Response) {
        debugger;
        const statusCode = res.status;
        const body = res.json();
        const error = {
            statusCode: statusCode,
            error: body
        };
        return throwError(error);
    }
}