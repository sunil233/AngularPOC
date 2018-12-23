import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AppConfig } from '../app.configuration';

@Injectable()
export class AuthenticationService {
    public apiUrl: string;
    constructor(private http: HttpClient, private config: AppConfig) {
        var appconfig = this.config ;
        this.apiUrl = appconfig.getApi() + 'api/Login';
  }
    login(username: string, password: string) {
        var url = this.apiUrl + '/Login';
        var data = {
            "username": username,
            "password": password
        }
        return this.http.post<any>(url, data);
  }
}