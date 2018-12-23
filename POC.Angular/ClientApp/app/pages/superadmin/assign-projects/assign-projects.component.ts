import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormBuilder, Validators, FormGroup, FormArray, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SuperAdminUserService } from '../../../services/superadmin-services/superadmin.user.service';
import { UsersModel } from '../../../models/superadmin-models/users.model';
import { CustomValidators } from '../../../customvalidations/CustomValidators';
import { ProjectService } from '../../../services/superadmin-services/superadmin.project.service';
import { ProjectModel } from '../../../models/superadmin-models/project.model';
import { AssignProjectModel } from '../../../models/superadmin-models/assign-projects.model';
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
    ManagerId: number = 0;
    projectdetails: ProjectModel;
    managerlist: Array<UsersModel> = [];
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
        this.getManagers();
        this.getUnAssignedProjects();
    }
    getManagers() {
        this.projectbusy = this.userService.GetAdminUsers().subscribe(data => {
            this.managerlist = data;
        }, (err) => { this.showError(err) });
    }
    getUnAssignedProjects(): void {
        this.projectbusy = this.projectService.getUnAssignedProjects().subscribe(projectsdata => {
            this.projectlist = projectsdata;
            var chkUsersControls = <FormArray>this.AssignForm.controls['chkProjects'];
            this.projectlist.forEach(item => {
                chkUsersControls.push(new FormControl(''))
            });
        }, (err) => { this.showError(err) });
    }
    initializeForm(): void {
        this.AssignForm = this.fb.group({
            'ddlManager': [null, Validators.required],
            'chkProjects': this.fb.array([], CustomValidators.multipleCheckboxRequireOne)
        });
    }
    submitForm($ev) {
        $ev.preventDefault();
        for (let c in this.AssignForm.controls) {
            this.AssignForm.controls[c].markAsTouched();
        }
        if (this.AssignForm.valid) {
            this.assignProjectsModel = new AssignProjectModel()
            var SelectedProjectList = this.projectlist.filter(it => {
                return it.selectedProject == true;
            });
            this.assignProjectsModel.Projects = SelectedProjectList;
            this.assignProjectsModel.ManagerId = this.ManagerId;
            this.projectService.SaveAssignedProjects(this.assignProjectsModel).subscribe(data => {
                if (data) {
                    this.getUnAssignedProjects();
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

