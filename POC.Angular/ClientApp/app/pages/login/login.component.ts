import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthenticationService } from "../../services/auth.service";
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
 
})
export class LoginComponent implements OnInit {
    loginForm: FormGroup;
    submitted: boolean = false;
    invalidLogin: boolean = false;
    constructor(private formBuilder: FormBuilder,
        private router: Router,
        private authService: AuthenticationService,
        private toastr: ToastrService) {
        localStorage.clear();
    }
    onSubmit() {
        this.submitted = true;
        if (!this.loginForm.invalid) {
            let username = this.loginForm.controls.username.value;
            let password = this.loginForm.controls.password.value;
            this.authService.login(username, password).subscribe(data => {
                if (data != null) {
                    if (data["UserId"] != null) {
                        localStorage["UserId"] = data["UserId"];
                    }
                    if (data["Username"] != null) {
                        localStorage["Username"] = data["Username"];
                    }
                    if (data.RoleCode != undefined && data["RoleCode"] == "SuperAdmin") {
                        localStorage["Role"] = "SuperAdmin";
                        this.router.navigate(['superadmin/dashboard']);
                    }
                    else if (data.RoleCode != undefined && data["RoleCode"] == "Manager") {
                        localStorage["Role"] = "Manager";
                        this.router.navigate(['admin/dashboard']);
                    }
                    else if (data.RoleCode != undefined && data["RoleCode"] == "User") {
                        localStorage["Role"] = "User";
                        this.router.navigate(['user/dashboard']);
                    }
                }
            }, (err) => { debugger;this.toastr.warning(err.error); });
        }
        else {
            this.invalidLogin = true;
            localStorage.clear();
        }
    }
    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

}