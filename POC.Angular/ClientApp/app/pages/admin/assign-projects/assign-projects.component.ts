import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormBuilder, Validators, FormGroup, FormArray, FormControl } from '@angular/forms';
import { CustomValidators } from '../../../customvalidations/CustomValidators';
import { ToastrService } from 'ngx-toastr';

import { UsersModel } from '../../../models/superadmin-models/users.model';
import { ProjectModel } from '../../../models/admin-models/admin.project.model';
import { AssignProjectModel } from '../../../models/admin-models/admin.assign-projects.model';
import { SuperAdminUserService } from '../../../services/superadmin-services/superadmin.user.service';
import { ProjectService } from '../../../services/admin-services/admin.project.service';

@Component({
    selector: 'superadmin-assign-projects',
    templateUrl: './assign-projects.component.html',
    styleUrls: ['./assign-projects.component.scss'],
    providers: [SuperAdminUserService, ProjectService]
})
export class AssignProjectsComponent implements OnInit {
    projectbusy: Subscription;
    AssignForm: FormGroup;
    userId: number = 0;
    projectId: number = 0;
    projectdetails: ProjectModel;
    teamMemberslist: Array<UsersModel> = [];
    projectlist: Array<ProjectModel> = [];
    assignProjectsModel: AssignProjectModel = null;
    constructor(private userService: SuperAdminUserService,
        private projectService: ProjectService,
        private router: Router,
        private fb: FormBuilder,
        private toastr: ToastrService, private _changeDetectionRef: ChangeDetectorRef) {
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
        this.getTeamMembers();
        this.getAssignedProjects();
    }
    getTeamMembers() {
        this.projectbusy = this.userService.GetTeamMembers(this.userId).subscribe(data => {
            this.teamMemberslist = data;
            var chkUsersControls = <FormArray>this.AssignForm.controls['chkUsers'];
            this.teamMemberslist.forEach(item => {
                chkUsersControls.push(new FormControl(''))
            });
        }, (err) => { this.showError(err) });
    }
    getAssignedProjects(): void {
        this.projectbusy = this.projectService.getAssignedProjects(this.userId).subscribe(projectsdata => {
            this.projectlist = projectsdata;
           
        }, (err) => { this.showError(err) });
    }
    initializeForm(): void {
        this.AssignForm = this.fb.group({
            'ddlProject': [null, Validators.required],
            'chkUsers': this.fb.array([], CustomValidators.multipleCheckboxRequireOne)
        });
    }
    submitForm($ev) {
        $ev.preventDefault();
        for (let c in this.AssignForm.controls) {
            this.AssignForm.controls[c].markAsTouched();
        }
        if (this.AssignForm.valid) {
            this.assignProjectsModel = new AssignProjectModel()
            var SelectedUserList = this.teamMemberslist.filter(it => {
                return it.selectedUsers == true;
            });
            this.assignProjectsModel.Users = SelectedUserList;
            this.assignProjectsModel.ProjectId = this.projectId;
            this.projectService.SaveAssignedProjects(this.assignProjectsModel).subscribe(data => {
                if (data) {
                    this.toastr.success('Assigned Successfully.');
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
    ngAfterViewChecked(): void {
        this._changeDetectionRef.detectChanges();
    }
}

