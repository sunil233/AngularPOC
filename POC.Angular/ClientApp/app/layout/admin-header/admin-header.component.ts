import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
@Component({
    selector: 'admin-header',
    templateUrl: './admin-header.component.html',
    styleUrls: ['./admin-header.component.scss']
})
export class AdminHeaderComponent implements OnInit {
    userName: string = "";
    constructor(private router: Router) {
        this.userName = localStorage["Username"];
    }
    ngOnInit() {
    }
    onMenuClick(routepath) {
        switch (routepath) {
            case 'dashboard':
                this.router.navigate(['/admin/dashboard']);
                break;
            case 'timesheets':
                this.router.navigate(['/admin/timesheets']);
                break;
            case 'users':
                this.router.navigate(['/admin/users-list']);
                break;
            case 'team':
                this.router.navigate(['/admin/team']);
                break;
            case 'projects':
                this.router.navigate(['/admin/projects-list']);
                break;
            case 'adminusers':
                this.router.navigate(['/admin/admin-list']);
                break;
            case 'assignprojects':
                this.router.navigate(['/admin/assign-projects']);
                break;     
            case 'removeprojects':
                this.router.navigate(['/admin/remove-project']);
                break;
            case 'tasks':
                this.router.navigate(['/admin/tasks', 0]);
                break;
            case 'addtask':
                this.router.navigate(['/admin/task']);
                break;
            case 'adddocuments':
                this.router.navigate(['/admin/add-document']);
                break;  
            case 'documents':
                this.router.navigate(['/admin/documents']);
                break;     
            case 'logout':
                this.router.navigate(['/login']);              
                break;
        }
    }
}