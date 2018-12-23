import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { TaskModel } from '../../../models/user-models/user.task.model';
import { TaskService } from '../../../services/user-services/user.task.service';
import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'user-task-list',
    templateUrl: './user-task-list.component.html',
    styleUrls: ['./user-task-list.component.scss'],
    providers: [TaskService]
})
export class UserTaskListComponent implements OnInit {
    userId: number = 0;
    projectId: number = 0;
    taskdetails: Array<TaskModel> = [];
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
        private route: ActivatedRoute,
        private router: Router,
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
        if (this.userId > 0) {
            this.getTasks();
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    getTasks() {
        this.tasksbusy = this.taskService.GetTasksByUserId(this.projectId, this.userId).subscribe(tasksdata => {
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