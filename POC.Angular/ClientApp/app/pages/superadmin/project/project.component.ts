import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ProjectModel } from '../../../models/superadmin-models/project.model';
import { ProjectService } from '../../../services/superadmin-services/superadmin.project.service';
@Component({
    selector: 'superadmin-project',
    templateUrl: './project.component.html',
    styleUrls: ['./project.component.scss'],
    providers: [ProjectService]
})
export class ProjectComponent implements OnInit {
    userId: number = 0;
    ProjectId: number = 0;
    projectdetails: ProjectModel;
    projectsbusy: Subscription;
    projectForm: FormGroup;
    isDisable: boolean = false;
    constructor(private projectService: ProjectService, private router: Router, private route: ActivatedRoute, private fb: FormBuilder, private toastr: ToastrService) {
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
            if (this.route.snapshot.paramMap.get('Id') != undefined && this.route.snapshot.paramMap.get('Id') != null) {
                this.ProjectId = parseInt(this.route.snapshot.paramMap.get('Id'));
            }
            this.projectdetails = new ProjectModel();
            if (this.ProjectId > 0) {
                this.isDisable = true;
                this.projectsbusy = this.projectService.getById(this.ProjectId).subscribe(projectsdata => {
                    this.projectdetails = projectsdata;
                }, (err) => { this.showError(err) });
            }
            else {
                this.projectdetails.IsActive = true;
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
        this.projectForm = this.fb.group({
            'ProjectName': [null, Validators.required],
            'ProjectCode': [null, Validators.required],
            'IsActive': [null, Validators.required]
        });
    }
    submitForm($ev, value: any) {
        $ev.preventDefault();
        for (let c in this.projectForm.controls) {
            this.projectForm.controls[c].markAsTouched();
        }
        if (this.projectForm.valid) {
            this.projectService.save(this.projectdetails).subscribe(projectdata => {
                if (projectdata != null && projectdata != undefined && projectdata.ProjectID > 0) {
                    this.toastr.success('Project Saved successfully.');
                }
            }, (err) => { this.showError(err) });
        }
    }
    onProjectNameChange() {
        if (this.ProjectId < 1) {
            var ProjectName = this.projectdetails.ProjectName;
            if (ProjectName != "" && ProjectName != undefined) {
                this.projectsbusy = this.projectService.CheckProjectNameExists(ProjectName).subscribe(projectsdata => {
                    if (projectsdata) {
                        this.toastr.warning('Project Name already exists.');
                        this.projectdetails.ProjectName = "";
                    }
                });
            }
        }
    }
    onProjectCodeChange() {
        if (this.ProjectId < 1) {
            var ProjectCode = this.projectdetails.ProjectCode;
            if (ProjectCode != "" && ProjectCode != undefined) {
                this.projectsbusy = this.projectService.CheckProjectCodeExists(ProjectCode).subscribe(projectsdata => {
                    if (projectsdata) {
                        this.toastr.warning('Project Code already exists.');
                        this.projectdetails.ProjectCode = "";
                    }
                });
            }
        }
    }
    onCancel() {
        this.router.navigate(['/superadmin/projects-list']);
    }
    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }
}