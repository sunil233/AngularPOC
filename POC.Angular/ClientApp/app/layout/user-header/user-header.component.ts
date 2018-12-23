import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
declare var $: any;
@Component({
    selector: 'user-header',
    templateUrl: './user-header.component.html',
    styleUrls: ['./user-header.component.scss']
})
export class UserHeaderComponent implements OnInit {
    isCollapsed = true;
    userName: string = "";
    constructor(private router: Router) {
        this.userName = localStorage["Username"];
    }

    ngOnInit() {

    }
    onMenuClick(routepath) {
       

        switch (routepath) {
            case 'dashboard':
                this.router.navigate(['/user/dashboard']);
                break;
            case 'timesheet':
                this.router.navigate(['/user/timesheet']);
                break;
            case 'projects':              
                this.router.navigate(['/user/project-list']);
                break;
            case 'tasks':
                this.router.navigate(['/user/tasks',0]);
                break;
            case 'addtask':
                this.router.navigate(['/user/task']);
                break;
            case 'logout':
                this.router.navigate(['/login']);
                break;
        }
    }
}
    