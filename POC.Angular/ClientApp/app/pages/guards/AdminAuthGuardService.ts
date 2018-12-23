import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';

@Injectable()
export class AdminAuthGuardService implements CanActivate {
    constructor(public router: Router) { }
    canActivate(): boolean {
        if (localStorage["Role"] != null && localStorage["Role"] != undefined && localStorage["Role"] != "") {
            if (localStorage["Role"] == "Manager") {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            this.router.navigate(['login']);
            return false;
        }
    }
}