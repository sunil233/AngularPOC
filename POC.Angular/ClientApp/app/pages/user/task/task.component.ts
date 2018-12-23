import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { TaskModel } from '../../../models/user-models/user.task.model';
import { ProjectModel } from '../../../models/user-models/user.project.model';
import { ProjectService } from '../../../services/user-services/user.project.service';
import { TaskService } from '../../../services/user-services/user.task.service';
@Component({
    selector: 'user-task',
    templateUrl: './task.component.html',
    styleUrls: ['./task.component.scss'],
    providers: [ProjectService, TaskService]
})
export class TaskComponent implements OnInit {
    userId: number = 0;
    taskId: number = 0;
    projectdetails: Array<ProjectModel> = [];
    taskdetails: TaskModel;
    taskbusy: Subscription;
    taskForm: FormGroup;
    isDisable: boolean = false;
    projectId: number = 0;
    constructor(private projectService: ProjectService,
        private taskService: TaskService,
        private router: Router,
        private route: ActivatedRoute,
        private fb: FormBuilder,
        private toastr: ToastrService) {
        this.taskdetails = new TaskModel();
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
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
        if (this.taskId > 0) {
            this.taskdetails.TaskID = this.taskId;
            this.isDisable = true;
            this.getTasks();
        }
        else {
            this.taskdetails.IsActive = true;
            this.taskdetails.ProjectID = -1;
            this.isDisable = false;
        }
    }
    getTasks() {
        this.taskService.getById(this.taskId).subscribe(taskdata => {
            this.taskdetails = taskdata;
        })
    }
    getProjects() {
        this.taskbusy = this.projectService.getAssignedProjects(this.userId).subscribe(projectsdata => {
            this.projectdetails = projectsdata;
        }, (err) => { this.showError(err) });
    }
    initializeForm(): void {
        this.taskForm = this.fb.group({
            'ddlProject': [null, Validators.required],
            'TaskName': [null, Validators.required],
            'Comments': [null],
            'IsActive': [null, Validators.required],
            'ddlStatus': [null, Validators.required]
        });
    }
    submitForm($ev) {
        $ev.preventDefault();
        for (let c in this.taskForm.controls) {
            this.taskForm.controls[c].markAsTouched();
        }
        if (this.taskForm.valid) {
            this.taskdetails.AssignedtoID = this.userId;
            this.taskService.save(this.taskdetails).subscribe(taskdata => {
                if (taskdata) {
                    this.toastr.success('Task Saved successfully.');
                    if (!this.isDisable) {
                        this.taskdetails.ProjectID = -1;
                        this.taskdetails.Taskname = "";
                        this.taskdetails.Comments = "";
                        this.taskdetails.Status = -1;
                    }
                }
            }, (err) => { this.showError(err) });
        }
    }
    onTaskNameChange() {
        if (this.taskId < 1) {
            var Taskname = this.taskdetails.Taskname;
            if (Taskname != "" && Taskname != undefined) {

            }
        }
    }
    onCancelClick() {
        debugger;
        this.router.navigate(['/user/tasks', this.projectId]);
    }
    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }
}