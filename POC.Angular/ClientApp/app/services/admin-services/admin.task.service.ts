import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../../app.configuration';
import { TaskModel } from '../../models/admin-models/admin.tasks.model';
import { BaseBackendService } from '../../shared/services/base-backend.service';

@Injectable()
export class TaskService extends BaseBackendService<TaskModel> {
      public apiUrl: string;
     constructor(public http: HttpClient, private config: AppConfig) {
         super(http, "Task", config);
         var appconfig = this.config;
         this.apiUrl = appconfig.getApi() + 'api/Task/';
    }  
    GetTasksByProjectId(ProjectId: number) {
        var url = this.apiUrl + 'GetTasksByProjectId?ProjectId=' + ProjectId;
        return this.http.get<TaskModel[]>(url);
    }
    GetTasksByUserId(ProjectId: number, userId: number) {
        var url = this.apiUrl + 'GetTasksByUserId?ProjectId=' + ProjectId + '&UserId=' + userId;
        return this.http.get<TaskModel[]>(url);
    }
}
