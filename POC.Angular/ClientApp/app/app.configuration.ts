import { Injectable, APP_INITIALIZER } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment'; //path to your environment files
/**
 *  class to read data from appconfig.json  before application startup.
 */
@Injectable()
export class AppConfig {
    private _config: Object
    private _env: string;
    public  ApiUrl: string;
    constructor(private _http: HttpClient) { }
    load() {
        return new Promise((resolve, reject) => {
         
            this._env = 'development';
            if (environment.production)
                this._env = 'production';
            console.log(this._env)
            this._http.get('./assets/config.' + this._env + '.json')
                .subscribe((data) => {
                    this._config = data;
                    this.ApiUrl = data["ApiUrl"];
                    resolve(true);
                },
                (error: any) => {
                        console.error(error);
                        return Observable.throw(error.json().error || 'Server error');
                    });
        });
    }
    // Is app in the development mode?
    isDevmode() {
        return this._env === 'development';
    }
    // Gets API route based on the provided key
    getApi(): string {
        return this._config["ApiUrl"];
    }
    // Gets a value of specified property in the configuration file
    get(key: any) {
        return this._config[key];
    }
}

export function ConfigFactory(config: AppConfig) {
    return () => config.load();
}

export function init() {
    return {
        provide: APP_INITIALIZER,
        useFactory: ConfigFactory,
        deps: [AppConfig],
        multi: true
    }
}

const ConfigModule = {
    init: init
}

export { ConfigModule };
