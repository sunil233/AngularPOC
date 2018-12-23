/**
 * These interceptors are called for every request and response, and allow to easily handle tasks like
 * adding a header to every request, or handling errors in a generic way
 */
import { Injectable, Injector } from '@angular/core'
import { HttpClient, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpHeaders, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from "rxjs";
import { catchError, finalize } from 'rxjs/operators';
import { AppConfig } from "../../app.configuration";
@Injectable()
export class CustomHttpInterceptor implements HttpInterceptor {
    authService: AppConfig;
    constructor(private injector: Injector) {
        setTimeout(() => {
            this.authService = this.injector.get(AppConfig);
        })
    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (!req.headers.has('Content-Type')) {
            req = req.clone({ headers: req.headers.set('Content-Type', 'application/json') });
        }
        //set the token(acysAuthtoken) needed in the Authorization header
        var acysAuthtoken = localStorage.getItem("acysAuthtoken");
        if (acysAuthtoken != null) {
            req = req.clone({ setHeaders: { 'Authorization': "Bearer " + localStorage.getItem("acysAuthtoken") } });
        }
        req = req.clone({ headers: req.headers.set('Accept', 'application/json') });
        return next.handle(req).pipe(
            catchError(err => {
                if (err instanceof HttpErrorResponse) {
                    if (err.status === 401) {
                       
                    }
                }
              
                return Observable.throw(err);
            }),
            finalize(() => {

            })
        );
    }
}


