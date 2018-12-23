import { Component, OnInit } from '@angular/core';
import { SuperAdminDashboardService } from '../../../services/superadmin-services/superadmin.dashboard.service';
import { Router } from '@angular/router';
import { UsersModel } from '../../../models/superadmin-models/users.model';
import { Subscription } from 'rxjs';

@Component({
    selector: 'users-list-dashboard',
    templateUrl: './users-list.component.html',
    styleUrls: ['./users-list.component.scss'],
    providers: [SuperAdminDashboardService]
})

export class UsersListComponent implements OnInit {
    userId: number = 0;
    userdetails: Array<UsersModel>=[];
    columns: any[] = [
        { headertext: 'Full Name', name: 'FullName', filter: 'text' },
        { headertext: 'Mobile No', name: 'Mobileno', filter: 'text' },
        { headertext: 'Work Email', name: 'WorkEmail', filter: 'text' },
        { headertext: 'Username', name: 'Username', filter: 'text' },
        { headertext: 'Manager', name: 'AssignToAdmin', filter: 'text' },
        { headertext: 'Role Name', name: 'RoleName', filter: 'text' },
        { headertext: 'Active/In Active', name: 'IsActive', filter: 'bool' }

    ];
    sorting: any = {
        column: 'name',
        descending: false
    };
    usersbusy: Subscription;
    constructor(private dashboardService: SuperAdminDashboardService, private router: Router) {
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
        }
    }
    ngOnInit() {
        this.usersbusy= this.dashboardService.GetAllUsers().subscribe(userdata => {
            this.userdetails = userdata;
        });
    }
    gridaction(gridaction: any): void {
        switch (gridaction.action) {
            case "edit":
                var user = gridaction.items[0].item;
                var RegistrationID = user.RegistrationID;
                this.router.navigate(['/superadmin/user', RegistrationID]);
                break;
            case "delete":
                var user = gridaction.items[0].item;
                var RegistrationID = user.RegistrationID;
                break;
        }
    }
}