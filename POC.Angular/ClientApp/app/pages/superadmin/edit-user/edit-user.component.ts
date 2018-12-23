import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { CustomValidators } from 'ng2-validation';
import { UsersModel } from '../../../models/superadmin-models/users.model';
import { SuperAdminUserService } from '../../../services/superadmin-services/superadmin.user.service';
import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'edit-user',
    templateUrl: './edit-user.component.html',
    styleUrls: ['./edit-user.component.scss'],
    providers: [SuperAdminUserService]
})
export class EditUserComponent implements OnInit {
    userId: number = 0;
    userdetails: UsersModel;
    RegistrationID: number = 0;
    bsConfig: Partial<BsDatepickerConfig>;
    colorTheme = 'theme-green';
    userForm: FormGroup;
    constructor(private userService: SuperAdminUserService,
        private route: ActivatedRoute,
        public fb: FormBuilder,
        private toastr: ToastrService) {
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
        if (this.RegistrationID > 0) {
            this.userService.GetUserDetailsById(this.RegistrationID).subscribe(userData => {
                this.userdetails = userData;
            }, (err) => { this.showError(err) });
        }

    }
    initializeForm(): void {
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
            'WorkEmail': [null, Validators.required],
            'ddlDepartment': [null, Validators.required],
            'ddlRole': [null, Validators.required],
            'ddlJob': [null, Validators.required],
            'ddlManager': [null, Validators.required],
            'IsActive': [null],
            'DateofJoining': [null, Validators.required],
            'DateofLeaving': [null]
           
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
                    this.toastr.success('Employee Saved successfully.');
                }
            }, (err) => { this.showError(err) });
        }
    }

    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }

}


