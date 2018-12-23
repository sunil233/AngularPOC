import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { CustomValidators } from 'ng2-validation';
import { UsersModel } from '../../../models/superadmin-models/users.model';
import { SuperAdminUserService } from '../../../services/superadmin-services/superadmin.user.service';

@Component({
    selector: 'user-super-admin',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.scss'],
    providers: [SuperAdminUserService]
})
export class UserComponent implements OnInit {
    userId: number = 0;
    userdetails: UsersModel;
    RegistrationID: number = 0;
    bsConfig: Partial<BsDatepickerConfig>;
    colorTheme = 'theme-green';
    userForm: FormGroup;
    constructor(private userService: SuperAdminUserService, private route: ActivatedRoute, public fb: FormBuilder) {
        this.userdetails = new UsersModel();
        this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
        }
        if (this.route.snapshot.paramMap.get('Id') != undefined && this.route.snapshot.paramMap.get('Id') != null) {
            this.RegistrationID = parseInt(this.route.snapshot.paramMap.get('Id'));
        }
        this.initializeForm();
    }
    ngOnInit() {
        this.userService.GetUserDetails().subscribe(userData => {
            this.userdetails = userData;
            this.userdetails.Gender="-1"
            this.userdetails.DeptId = -1;
            this.userdetails.RoleID = -1;
            this.userdetails.JobId = -1;
            this.userdetails.ManagerId = -1;
        });
    }
    initializeForm(): void {
        let password = new FormControl('', Validators.required);
        let confirmPassword = new FormControl('', CustomValidators.equalTo(password));
        // Model Driven validation
        this.userForm = this.fb.group({
            'FirstName': [null, Validators.required],
            'MiddleName': [null],
            'LastName': [null, Validators.required],
            'Birthdate': [null, Validators.required],
            'Mobileno': [null, Validators.required],
            'EmailID': [null, Validators.required],
            'EmergencyContact': [null, Validators.required],
            'Gender': [null, Validators.required],
            'EmergencyContactNumber': [null, Validators.required],
            'Username': [null, Validators.required],
            'Password': [null, Validators.required],
            'ConfirmPassword': [null, Validators.required],
            'WorkEmail': [null, Validators.required],
            'ddlDepartment': [null, Validators.required],
            'ddlRole': [null, Validators.required],
            'ddlJob': [null, Validators.required],
            'ddlManager': [null, Validators.required],
            'DateofJoining': [null, Validators.required],
            'passwordGroup': this.fb.group({
                Username: new FormControl('', Validators.required),
                password: password,
                confirmPassword: confirmPassword
            })
        });
    }
    submitForm($ev, value: any) {
        $ev.preventDefault();
        for (let c in this.userForm.controls) {
            this.userForm.controls[c].markAsTouched();
        }
        if (this.userForm.valid) {
            console.log('Valid!');
            this.userService.SaveUser(this.userdetails).subscribe(userId => {
                if (userId > 0) {
                    alert("user added successfully.");
                }
            }, error => {
                debugger;
                console.log(error);
            });
        }
        console.log(value);
    }

}


