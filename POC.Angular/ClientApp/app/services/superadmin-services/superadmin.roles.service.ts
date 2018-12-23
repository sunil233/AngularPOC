import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../app.configuration';
import { BaseBackendService } from '../../shared/services/base-backend.service';
import { RolesModel } from '../../models/superadmin-models/roles.model';

@Injectable()
export class RolesService extends BaseBackendService<RolesModel> {
    public apiUrl: string;
    constructor(public http: HttpClient, private config: AppConfig) {
        super(http, "Roles", config);
        var appconfig = this.config;
        this.apiUrl = appconfig.getApi() + 'api/Roles/';
    }
    ShowallRoles() {
        var url = this.apiUrl + 'ShowallRoles';
        return this.http.get<any>(url);
    }
    RemovefromRole(RegistrationID:string) {
        var url = this.apiUrl + 'RemovefromRole?RegistrationID=' + RegistrationID;
        return this.http.get<any>(url);
    }
    CheckRoleCodeExists(RoleCode: string) {
        var url = this.apiUrl + 'CheckRoleCodeExists?RoleCode=' + RoleCode;
        return this.http.get<any>(url);
    }
    CheckRoleNameExists(RoleName: string) {
        var url = this.apiUrl + 'CheckRoleNameExists?RoleName=' + RoleName;
        return this.http.get<any>(url);
    }
}