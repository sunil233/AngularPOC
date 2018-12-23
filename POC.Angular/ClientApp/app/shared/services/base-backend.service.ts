import { Injectable } from '@angular/core'
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import 'rxjs/Rx';
import { catchError } from 'rxjs/operators';
import { AppConfig } from '../../app.configuration';

@Injectable()
export abstract class BaseBackendService<T> {

    public actionUrl: string;
    public  _name: string;
    private isprodenv: boolean;
    constructor(public http: HttpClient, private name: string, configuration: AppConfig) {
        this.actionUrl = configuration.ApiUrl + "api/" + name + "/";
        this._name = name;
    }
    public getAll(): Observable<T[]> {
        let url = this.actionUrl +"GetAll";
        return this.http.get<T[]>(url)
            .pipe(catchError(this.handleError.bind(this)));
    }
    public getById(id: number): Observable<T> {
        let url = this.actionUrl+"getById?ID="+ id;
        return this.http.get<T>(url)
           .pipe(catchError(this.handleError.bind(this)));
    }
    public save(data: T): Observable<T> {
        let url = this.actionUrl + "/Save";
        return this.http.post<T>(url, data)
                       .pipe(catchError(this.handleError.bind(this)));
    }
    public update(data: T): Observable<T> {
        let url = this.actionUrl + "/Update";
        return this.http.put<T>(url, data)
            .pipe(catchError(this.handleError.bind(this)));
    }
    public delete(id: number): Observable<T> {
        let url = this.actionUrl + "Delete?ID=" + id;
        return this.http.get<T>(url)
          .pipe(catchError(this.handleError.bind(this)));
    }
    public handleError(error: HttpErrorResponse | any) {
        return Observable.throw(error);
    }
}



