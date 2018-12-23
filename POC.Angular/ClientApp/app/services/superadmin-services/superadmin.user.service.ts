import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../app.configuration';
import { UsersModel } from '../../models/superadmin-models/users.model';
import { throwError } from 'rxjs';
import { AssignRolesModel } from '../../models/superadmin-models/assign-roles.model';


@Injectable()
export class SuperAdminUserService {
    public apiUrl: string;
    constructor(private http: HttpClient, private config: AppConfig) {
        var appconfig = this.config ;
        this.apiUrl = appconfig.getApi() + 'api/User/';
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
    GetAdminUsers() {
        var url = this.apiUrl + 'GetAdminUsers';
        return this.http.get<Array<UsersModel>>(url);
    }
    GetListofUnAssignedUsers() {
        var url = this.apiUrl + 'GetListofUnAssignedUsers';
        return this.http.get<Array<UsersModel>>(url);
    }
    SaveAssignedRoles(assignRolesModel: AssignRolesModel) {
        var url = this.apiUrl + 'SaveAssignedRoles';
        return this.http.post<boolean>(url, assignRolesModel);
    }
    GetTeamMembers(AdminId: number) {
        var url = this.apiUrl + 'GetTeamMembers?AdminId=' + AdminId;
        return this.http.get<Array<UsersModel>>(url);
    }
}