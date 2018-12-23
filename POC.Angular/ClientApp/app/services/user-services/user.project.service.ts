import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../app.configuration';
import { ProjectModel } from '../../models/user-models/user.project.model';
import { BaseBackendService } from '../../shared/services/base-backend.service';

@Injectable()
export class ProjectService extends BaseBackendService<ProjectModel> {
      public apiUrl: string;
     constructor(public http: HttpClient, private config: AppConfig) {
         super(http, "Project", config);
         var appconfig = this.config;
         this.apiUrl = appconfig.getApi() + 'api/Project/';
    }
    CheckProjectCodeExists(ProjectCode: string) {
        var url = this.apiUrl + 'CheckProjectCodeExists?ProjectCode=' + ProjectCode;
        return this.http.get<any>(url);
    }
    getAssignedProjects(UserId: number) {
        var url = this.apiUrl + 'GetAssignedProjects?userId=' + UserId;
        return this.http.get<ProjectModel[]>(url);
    }

    GetAllByRole(RoleCode: string) {
        var url = this.apiUrl + 'GetAllByRole?RoleCode=' + RoleCode;
        return this.http.get<ProjectModel[]>(url);
    }

}
