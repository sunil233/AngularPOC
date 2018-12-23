import { Component, OnInit } from '@angular/core';
import { AdminDashboardService } from '../../../services/admin-services/admin.dashboard.service';
import { Router } from '@angular/router';
import { UsersModel } from '../../../models/admin-models/admin.users.model';
import { Subscription } from 'rxjs';

@Component({
    selector: 'admin-team',
    templateUrl: './team.component.html',
    styleUrls: ['./team.component.scss'],
    providers: [AdminDashboardService]
})
export class TeamComponent implements OnInit {
    timesheetdetails: any[] = [];
    userId: number = 0;
    teamdetails: Array<UsersModel> = [];
    usersbusy: Subscription;
    columns: any[] = [
        { headertext: 'Full Name', name: 'FullName', filter: 'text' },
        { headertext: 'Mobile No', name: 'Mobileno', filter: 'text' },
        { headertext: 'Work Email', name: 'WorkEmail', filter: 'text' },
        { headertext: 'Username', name: 'Username', filter: 'text' },      
        { headertext: 'Role Name', name: 'RoleName', filter: 'text' },
        { headertext: 'Active/In Active', name: 'IsActive', filter: 'bool' }

    ];
    sorting: any = {
        column: 'name',
        descending: false
    };
    constructor(private dashboardService: AdminDashboardService, private router: Router) {
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
        }
    }
    ngOnInit() {
        if (this.userId > 0) {
            this.dashboardService.GetTeamByAdminId(this.userId).subscribe(teamdata => {
                this.teamdetails = teamdata;
            });
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    gridaction(gridaction: any): void {
        switch (gridaction.action) {
            case "view":
                var user = gridaction.items[0].item;
                var RegistrationID = user.RegistrationID;
                this.router.navigate(['/superadmin/user', RegistrationID]);
                break;
        }
    }

}