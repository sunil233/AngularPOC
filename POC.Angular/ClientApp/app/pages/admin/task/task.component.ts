import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UsersModel } from '../../../models/admin-models/admin.users.model';
import { TaskModel } from '../../../models/admin-models/admin.tasks.model';
import { ProjectModel } from '../../../models/admin-models/admin.project.model';
import { ProjectService } from '../../../services/admin-services/admin.project.service';
import { SuperAdminUserService } from '../../../services/superadmin-services/superadmin.user.service';
import { TaskService } from '../../../services/admin-services/admin.task.service';
@Component({
    selector: 'admin-task',
    templateUrl: './task.component.html',
    styleUrls: ['./task.component.scss'],
    providers: [ProjectService, TaskService, SuperAdminUserService]
})
export class TaskComponent implements OnInit {
    userId: number = 0;
    taskId: number = 0;
    projectId: number = 0;
    AssignedtoId: number = 0;
    Taskname: string = "";
    status: number = 0;
    comments: string = "";
    IsActive: boolean = true;
    projectdetails: Array<ProjectModel> = [];
    teamMemberslist: Array<UsersModel> = [];
    taskmodel: TaskModel;
    taskbusy: Subscription;
    taskForm: FormGroup;
    isDisable: boolean = false;
    constructor(private router: Router,
        private route: ActivatedRoute,
        private fb: FormBuilder,
        private toastr: ToastrService,
        private projectService: ProjectService,
        private userService: SuperAdminUserService,
        private taskService: TaskService) {
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
            this.taskmodel = new TaskModel();
            if (this.route.snapshot.paramMap.get('Id') != undefined && this.route.snapshot.paramMap.get('Id') != null) {
                this.taskId = parseInt(this.route.snapshot.paramMap.get('Id'));
            }
            if (this.route.snapshot.paramMap.get('projectId') != undefined && this.route.snapshot.paramMap.get('projectId') != null) {
                this.projectId = parseInt(this.route.snapshot.paramMap.get('projectId'));
            }
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    ngOnInit() {
        this.initializeForm();
        this.getProjects();
        this.getTasks();
        this.getTeamMembers();     
    }
    getTasks() {
        this.taskService.getById(this.taskId).subscribe(taskdata => {
            this.taskmodel = taskdata;
        })
    }
    getProjects() {
        this.taskbusy = this.projectService.getAssignedProjects(this.userId).subscribe(projectsdata => {
            this.projectdetails = projectsdata;
        }, (err) => { this.showError(err) });
    }
    getTeamMembers() {
        this.taskbusy = this.userService.GetTeamMembers(this.userId).subscribe(data => {
            this.teamMemberslist = data;

        }, (err) => { this.showError(err) });
    }
    initializeForm(): void {
        this.taskForm = this.fb.group({
            'ddlProject': [null, Validators.required],
            'ddlUser': [null, Validators.required],
            'ddlStatus': [null, Validators.required],
            'TaskName': [null, Validators.required],          
            'Comments': [null],
            'IsActive': [null, Validators.required]
        });
    }
    submitForm($ev) {
        $ev.preventDefault();
        for (let c in this.taskForm.controls) {
            this.taskForm.controls[c].markAsTouched();
        }
        if (this.taskForm.valid) {
            this.taskmodel = new TaskModel();
            this.taskmodel.ProjectID = this.projectId;
            this.taskmodel.AssignedtoID = this.AssignedtoId;
            this.taskmodel.Status = this.status;
            this.taskmodel.Comments = this.comments;
            this.taskmodel.Taskname = this.Taskname;
            this.taskmodel.IsActive = this.IsActive;
            this.taskService.save(this.taskmodel).subscribe(taskdata => {
                if (taskdata) {
                    this.toastr.success('Task Saved successfully.');
                    if (!this.isDisable) {
                        this.taskmodel.ProjectID = -1;
                        this.taskmodel.AssignedtoID = -1;
                        this.taskmodel.Status = -1;
                        this.taskmodel.Comments = "";
                        this.taskmodel.Taskname = "";
                        this.taskmodel.IsActive =true;                      
                    }
                }
            }, (err) => { this.showError(err) });
        }
    }
    onTaskNameChange() {
        if (this.taskId < 1) {
            var Taskname = this.Taskname;
            if (Taskname != "" && Taskname != undefined) {

            }
        }
    }
    onCancelClick() {
        this.router.navigate(['/admin/tasks', this.projectId]);
    }
    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }
}