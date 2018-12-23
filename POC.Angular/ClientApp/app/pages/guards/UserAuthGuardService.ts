import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';

@Injectable()
export class UserAuthGuardService implements CanActivate {
    constructor(public router: Router) { }
    canActivate(): boolean {
        if (localStorage["Role"] != null && localStorage["Role"] != undefined && localStorage["Role"] != "") {
            if (localStorage["Role"] == "User") {
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