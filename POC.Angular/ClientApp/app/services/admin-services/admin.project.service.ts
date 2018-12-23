import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../app.configuration';
import { ProjectModel } from '../../models/admin-models/admin.project.model';
import { AssignProjectModel } from '../../models/admin-models/admin.assign-projects.model';
import { BaseBackendService } from '../../shared/services/base-backend.service';

@Injectable()
export class ProjectService extends BaseBackendService<ProjectModel> {
      public apiUrl: string;
     constructor(public http: HttpClient, private config: AppConfig) {
         super(http, "Project", config);
         var appconfig = this.config;
         this.apiUrl = appconfig.getApi() + 'api/Project/';
    }
    getAssignedProjects(AdminUserId:number) {
        var url = this.apiUrl + 'GetAdminProjects?AdminUserId=' + AdminUserId;
        return this.http.get<ProjectModel[]>(url);
    }
    getAdminProjects(AdminUserId: number) {
        var url = this.apiUrl + 'GetAdminProjects?AdminUserId=' + AdminUserId;
        return this.http.get<ProjectModel[]>(url);
    }
    SaveAssignedProjects(assignProjectModel: AssignProjectModel) {
        var url = this.apiUrl + 'SaveAssignedProjects';
        return this.http.post<boolean>(url, assignProjectModel);
    }
    RemoveProject(userId: number, projectId: number) {
        var url = this.apiUrl + 'RemoveProject?userId=' + userId + "&projectId=" + projectId;
        return this.http.get<ProjectModel[]>(url);
    }
    GetAllByRole(RoleCode: string) {
        var url = this.apiUrl + 'GetAllByRole?RoleCode=' + RoleCode;
        return this.http.get<ProjectModel[]>(url);
    }
}
