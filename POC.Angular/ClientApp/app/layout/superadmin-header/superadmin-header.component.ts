import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
@Component({
    selector: 'superadmin-header',
    templateUrl: './superadmin-header.component.html',
    styleUrls: ['./superadmin-header.component.scss']
})
export class SuperAdminHeaderComponent implements OnInit {
    userName: string = "";
    constructor(private router: Router) {
        this.userName = localStorage["Username"];
    }
    ngOnInit() {
    }
    onMenuClick(routepath) {
        switch (routepath) {
            case 'dashboard':
                this.router.navigate(['/superadmin/dashboard']);
                break;
            case 'users':
                this.router.navigate(['/superadmin/users-list']);
                break;
            case 'Addusers':
                this.router.navigate(['/superadmin/user']);
                break;
            case 'project':
                this.router.navigate(['/superadmin/project']);
                break;
            case 'projects':
                this.router.navigate(['/superadmin/projects-list']);
                break;
            case 'assignprojects':
                this.router.navigate(['/superadmin/assign-projects']);
                break;
            case 'removeprojects':
                this.router.navigate(['/superadmin/remove-project']);
                break;
            case 'adminusers':
                this.router.navigate(['/superadmin/admin-list']);
                break;
            case 'assignmanager':
                this.router.navigate(['/superadmin/assign-managers']);
                break;
            case 'removemanager':
                this.router.navigate(['/superadmin/remove-manager']);
                break;
            case 'roles':
                this.router.navigate(['/superadmin/roles-list']);
                break;
            case 'addrole':
                this.router.navigate(['/superadmin/role']);
                break;           
            case 'logout':
                this.router.navigate(['/login']);
                break;

                
        }
    }
}