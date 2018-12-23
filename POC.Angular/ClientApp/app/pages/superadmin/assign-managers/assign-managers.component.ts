import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormBuilder, Validators, FormGroup, FormArray, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SuperAdminUserService } from '../../../services/superadmin-services/superadmin.user.service';
import { UsersModel } from '../../../models/superadmin-models/users.model';
import { CustomValidators } from '../../../customvalidations/CustomValidators';
import { AssignRolesModel } from '../../../models/superadmin-models/assign-roles.model';
@Component({
    selector: 'superadmin-assign-managers',
    templateUrl: './assign-managers.component.html',
    styleUrls: ['./assign-managers.component.scss'],
    providers: [SuperAdminUserService]
})
export class AssignManagersComponent implements OnInit {
    userId: number = 0;
    userdetails: UsersModel;
    userbusy: Subscription;
    AssignForm: FormGroup;
    userslist: Array<UsersModel> = [];
    adminlist: Array<UsersModel> = [];
    assignRolesModel: AssignRolesModel = null;
    constructor(private userService: SuperAdminUserService,
        private router: Router,
        private fb: FormBuilder,
        private toastr: ToastrService, private _changeDetectionRef: ChangeDetectorRef) {
        this.userdetails = new UsersModel();
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    ngOnInit() {
        this.initializeForm();
        this.userbusy = this.userService.GetAdminUsers().subscribe(data => {
            this.adminlist = data;
        }, (err) => { this.showError(err) });
        this.getUnAssignedUsers();
    }
    getUnAssignedUsers(): void {
        this.userbusy = this.userService.GetListofUnAssignedUsers().subscribe(data => {
            this.userslist = [];
            this.userslist = data;
            var chkUsersControls = <FormArray>this.AssignForm.controls['chkUsers'];
            this.userslist.forEach(item => {
                chkUsersControls.push(new FormControl(''))
            });
        }, (err) => { this.showError(err) });
    }
    initializeForm(): void {
        this.AssignForm = this.fb.group({
            'ddlAdmin': [null, Validators.required],          
            'chkUsers': this.fb.array([], CustomValidators.multipleCheckboxRequireOne)
        });
    }
    submitForm($ev, value: any) {
        $ev.preventDefault();
        for (let c in this.AssignForm.controls) {
            this.AssignForm.controls[c].markAsTouched();
        }
        if (this.AssignForm.valid) {
            this.assignRolesModel = new AssignRolesModel()
            var SelectedUsersList = this.userslist.filter(it => {
                return it.selectedUsers == true;
            });
            this.assignRolesModel.ListofUser = SelectedUsersList;
            this.assignRolesModel.ListofAdmins = this.adminlist;
            this.assignRolesModel.RegistrationID = this.userdetails.RegistrationID;
            this.userService.SaveAssignedRoles(this.assignRolesModel).subscribe(data => {
                if (data) {
                    this.getUnAssignedUsers();
                    this.userdetails.RegistrationID = -1;
                    this.toastr.success('Assigned Successfully.');
                }
            }, (err) => { this.showError(err) });
        }
    }
    ngAfterViewChecked(): void {
        this._changeDetectionRef.detectChanges();
    }
    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }
}
//https://stackblitz.com/edit/angular-custom-form-validation?file=app%2Fapp.component.html
