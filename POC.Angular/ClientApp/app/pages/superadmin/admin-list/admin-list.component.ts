import { Component, OnInit } from '@angular/core';
import { SuperAdminDashboardService } from '../../../services/superadmin-services/superadmin.dashboard.service';
import { Router } from '@angular/router';
import { UsersModel } from '../../../models/superadmin-models/users.model';
import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'admin-list-dashboard',
    templateUrl: './admin-list.component.html',
    styleUrls: ['./admin-list.component.scss'],
    providers: [SuperAdminDashboardService]
})

export class AdminUsersListComponent implements OnInit {
    userId: number = 0;
    adminuserdetails: Array<UsersModel>=[];
    columns: any[] = [
        { headertext: 'Full Name', name: 'FullName', filter: 'text' },
        { headertext: 'Mobile No', name: 'Mobileno', filter: 'text' },
        { headertext: 'Work Email', name: 'WorkEmail', filter: 'text' },
        { headertext: 'Username', name: 'Username', filter: 'text' },       
        { headertext: 'Active/In Active', name: 'IsActive', filter: 'bool' }

    ];
    sorting: any = {
        column: 'name',
        descending: false
    };
    constructor(private dashboardService: SuperAdminDashboardService, private router: Router, private toastr: ToastrService) {
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
        }
    }
    ngOnInit() {
        this.dashboardService.GetAllAdminUsers().subscribe(userdata => {
            this.adminuserdetails = userdata;
        }, (err) => { this.showError(err) });
    }
    gridaction(gridaction: any): void {
        switch (gridaction.action) {
            case "view":
                var timesheet = gridaction.items[0].item;
                var timesheetstatus = timesheet.TimeSheetStatus;
                var timeSheetMasterID = timesheet.TimeSheetMasterID;
                switch (timesheetstatus) {
                    case 'Saved':
                        this.router.navigate(['/user/timesheet', timeSheetMasterID]);
                        break;
                    case 'Submitted':
                        this.router.navigate(['/user/submittedtimesheet', timeSheetMasterID]);
                        break;
                    case 'Approved':
                        this.router.navigate(['/user/timesheet', timeSheetMasterID]);
                        break;
                    case 'Rejected':
                        this.router.navigate(['/user/timesheet', timeSheetMasterID]);
                        break;
                }
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