import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { RolesModel } from '../../../models/superadmin-models/roles.model';
import { RolesService } from '../../../services/superadmin-services/superadmin.roles.service';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'superadmin-role',
    templateUrl: './role.component.html',
    styleUrls: ['./role.component.scss'],
    providers: [RolesService]
})
export class RoleComponent implements OnInit {
    userId: number = 0;
    roleId: number = 0;
    roledetails: RolesModel;
    rolesbusy: Subscription;
    roleForm: FormGroup;
    isDisable: boolean = false;
    constructor(private roleService: RolesService,
        private router: Router,
        private route: ActivatedRoute,
        private fb: FormBuilder,
        private toastr: ToastrService) {
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
            if (this.route.snapshot.paramMap.get('Id') != undefined && this.route.snapshot.paramMap.get('Id') != null) {
                this.roleId = parseInt(this.route.snapshot.paramMap.get('Id'));
            }
            this.roledetails = new RolesModel();
            if (this.roleId > 0) {
                this.isDisable = true;
                this.rolesbusy = this.roleService.getById(this.roleId).subscribe(rolesdata => {
                    this.roledetails = rolesdata;
                }, (err) => { this.toastr.warning(err.error); });
            }
            else {
                this.roledetails.IsActive = true;
                this.isDisable = false;
            }
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    ngOnInit() {
        this.initializeForm();
    }
    initializeForm(): void {
        this.roleForm = this.fb.group({
            'RoleName': [null, Validators.required],
            'RoleCode': [null, Validators.required],
            'IsActive': [null, Validators.required]
        });
    }
    submitForm($ev, value: any) {
        $ev.preventDefault();
        for (let c in this.roleForm.controls) {
            this.roleForm.controls[c].markAsTouched();
        }
        if (this.roleForm.valid) {
            this.roleService.save(this.roledetails).subscribe(rolesdata => {
                if (rolesdata != null && rolesdata != undefined && rolesdata.RoleID > 0) {
                    this.toastr.success('Role Saved successfully.');
                }
            }, (err) => { this.showError(err) });
        }
    }
    onRoleNameChange() {
        if (this.roleId < 1) {
            var Rolename = this.roledetails.Rolename;
            if (Rolename != "" && Rolename != undefined) {
                this.rolesbusy = this.roleService.CheckRoleNameExists(Rolename).subscribe(rolesdata => {
                    if (rolesdata) {
                        this.toastr.warning('Project Name already exists.');
                        this.roledetails.Rolename = "";
                    }
                }, (err) => { this.showError(err) });
            }
        }
    }
    onRoleCodeChange() {
        if (this.roleId < 1) {
            var RoleCode = this.roledetails.RoleCode;
            if (RoleCode != "" && RoleCode != undefined) {
                this.rolesbusy = this.roleService.CheckRoleCodeExists(RoleCode).subscribe(projectsdata => {
                    if (projectsdata) {
                        this.toastr.warning('Role Code already exists.');
                        this.roledetails.RoleCode = "";
                    }
                }, (err) => { this.showError(err) });
            }
        }
    }
    onCancel() {
        this.router.navigate(['/superadmin/roles-list']);
    }
    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }
}