import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { TaskModel } from '../../../models/admin-models/admin.tasks.model';
import { UsersModel } from '../../../models/admin-models/admin.users.model';
import { ProjectModel } from '../../../models/admin-models/admin.project.model';
import { TaskService } from '../../../services/admin-services/admin.task.service';
import { SuperAdminUserService } from '../../../services/superadmin-services/superadmin.user.service';
import { ProjectService } from '../../../services/admin-services/admin.project.service';
@Component({
    selector: 'user-task-list',
    templateUrl: './user-task-list.component.html',
    styleUrls: ['./user-task-list.component.scss'],
    providers: [TaskService, SuperAdminUserService, ProjectService]
})
export class UserTaskListComponent implements OnInit {
    userId: number = 0;
    projectId: number = 0;
    taskdetails: Array<TaskModel> = [];
    TeamMemberId: number = 0;
    teamMemberslist: Array<UsersModel> = [];
    projectlist: Array<ProjectModel> = [];
    taskForm: FormGroup;
    columns: any[] = [
        { headertext: 'Project Name', name: 'ProjectName', filter: 'text' },
        { headertext: 'Task Name', name: 'Taskname', filter: 'text' },
        { headertext: 'Assigned To', name: 'AssignedtoUser', filter: 'text' },
        { headertext: 'Status', name: 'StatusType', filter: 'text' },   
        { headertext: 'IsActive', name: 'IsActive', filter: 'bool' }
    ];
    sorting: any = {
        column: 'ProjectName',
        descending: false
    };
    tasksbusy: Subscription;
    constructor(private taskService: TaskService,
        private userService: SuperAdminUserService,
        private projectService: ProjectService,
        private route: ActivatedRoute,
        private router: Router,
        private fb: FormBuilder,
        private toastr: ToastrService) {
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
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
        if (this.userId > 0) {
            this.getTeamMembers();
            this.getAssignedProjects();
            this.getTasks();
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    initializeForm(): void {
        this.taskForm = this.fb.group({
            'ddlProject': [null, Validators.required],
            'ddlUser': [null, Validators.required]          
        });
    }
    submitForm($ev) {
        $ev.preventDefault();
        for (let c in this.taskForm.controls) {
            this.taskForm.controls[c].markAsTouched();
        }
     
        if (this.taskForm.valid) {
            this.taskService.GetTasksByUserId(this.projectId, this.TeamMemberId).subscribe(taskdata => {
                this.taskdetails = taskdata;
            }, (err) => { this.showError(err) });
        }
    }
    getTeamMembers() {
        this.tasksbusy = this.userService.GetTeamMembers(this.userId).subscribe(data => {
            this.teamMemberslist = data;           
        }, (err) => { this.showError(err) });
    }
    getAssignedProjects(): void {
        this.tasksbusy = this.projectService.getAssignedProjects(this.userId).subscribe(projectsdata => {
            this.projectlist = projectsdata;
        }, (err) => { this.showError(err) });
    }
    getTasks() {
        this.tasksbusy = this.taskService.GetTasksByUserId(this.projectId, this.TeamMemberId).subscribe(tasksdata => {
            this.taskdetails = tasksdata;
        }, (err) => { this.showError(err) });
    }
    gridaction(gridaction: any): void {
        switch (gridaction.action) {
            case "edit":
                var task = gridaction.items[0].item;
                var TaskID = task.TaskID;
                this.router.navigate(['/user/task', TaskID]);
                break;
            case "delete":
                var task = gridaction.items[0].item;
                var TaskID = task.TaskID;
                this.tasksbusy = this.taskService.delete(TaskID).subscribe((taskdata) => {
                    if (taskdata) {
                        this.toastr.success("Task Deleted Successfully.");
                        this.getTasks();
                    }
                }, (err) => { this.showError(err) });
                break;
        }
    }
    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }
}