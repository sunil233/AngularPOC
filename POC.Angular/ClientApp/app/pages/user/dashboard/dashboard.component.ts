import { Component, OnInit } from '@angular/core';
import { UserDashboardService } from '../../../services/user-services/user.dashboard.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Status } from '../../../models/status.enum';
declare var $: any;
@Component({
    selector: 'user-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
    providers: [UserDashboardService]
})
export class DashboardComponent implements OnInit {
    timesheetdetails: any[] = [];
    userId: number = 0;
    RecentSubmissionsCount: number = 0;
    SubmittedTimesheetCount: number = 0;
    ApprovedTimesheetCount: number = 0;
    RejectedTimesheetCount: number = 0;
    SavedTimesheetCount: number = 0;
    dashboardbusy: Subscription;
    panelheader: string = "";
    panelClass: string = "";
    constructor(private dashboardService: UserDashboardService, private router: Router) {
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
        }
    }
    ngOnInit() {
        if (this.userId > 0) {
            this.dashboardbusy = this.dashboardService.Dashboard(this.userId).subscribe(dashboardData => {
                if (dashboardData.TimesheetData != null && dashboardData.TimesheetData != undefined) {
                    var TimesheetData = dashboardData.TimesheetData;
                    if (TimesheetData.RecentSubmissionsCount != undefined) {
                        this.RecentSubmissionsCount = TimesheetData.RecentSubmissionsCount;
                    }
                    if (TimesheetData.SavedTimesheetCount != undefined) {
                        this.SavedTimesheetCount = TimesheetData.SavedTimesheetCount;
                    }
                    if (TimesheetData.SubmittedTimesheetCount != undefined) {
                        this.SubmittedTimesheetCount = TimesheetData.SubmittedTimesheetCount;
                    }
                    if (TimesheetData.ApprovedTimesheetCount != undefined) {
                        this.ApprovedTimesheetCount = TimesheetData.ApprovedTimesheetCount;
                    }
                    if (TimesheetData.RejectedTimesheetCount != undefined) {
                        this.RejectedTimesheetCount = TimesheetData.RejectedTimesheetCount;
                    }
                }
            });
           
            this.dashboardbusy = this.dashboardService.GetTimeSheetsByStatus(this.userId, Status.Save).subscribe(tsdetails => {
                this.timesheetdetails = tsdetails;
                this.AddDownArrow("recent");
            });
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    columns: any[] = [
        { headertext: 'Username', name: 'Username', filter: 'text' },
        { headertext: 'Submitted Month', name: 'SubmittedMonth', filter: 'text' },
        { headertext: 'From Date', name: 'FromDate', filter: 'text' },
        { headertext: 'To Date', name: 'ToDate', filter: 'text' },
        { headertext: 'Total Hours', name: 'TotalHours', filter: 'number' },
        { headertext: 'Status', name: 'TimeSheetStatus', filter: 'text' }

    ];
    sorting: any = {
        column: 'name',
        descending: false
    };
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
                        this.router.navigate(['/user/timesheets', timeSheetMasterID]);
                        break;
                    case 'Approved':
                        this.router.navigate(['/user/timesheets', timeSheetMasterID]);
                        break;
                    case 'Rejected':
                        this.router.navigate(['/user/timesheet', timeSheetMasterID]);
                        break;
                }
                break;
        }
    }
    onCardClick(cardtitle) {
        this.AddDownArrow(cardtitle);
    }
    AddDownArrow(divName): void {
        var divRecentSubmissions = $('#divRecentSubmissions');
        var divSubmittedHours = $('#divSubmittedHours');
        var divRejectedHours = $('#divRejectedHours');
        var divApprovedHours = $('#divApprovedHours');
        switch (divName) {
            case 'recent':
                this.panelheader = "Saved Hours";
                this.panelClass = "panel panel-green";
                if (!divRecentSubmissions.hasClass("green-triangle-down")) {
                    divRecentSubmissions.addClass("green-triangle-down")
                }
                divSubmittedHours.removeClass("blue-triangle-down");
                divRejectedHours.removeClass("red-triangle-down");
                divApprovedHours.removeClass("lightgreen-triangle-down");
                //load recent time sheets
                this.dashboardbusy = this.dashboardService.GetTimeSheetsByStatus(this.userId, Status.Save).subscribe(tsdetails => {
                    this.timesheetdetails = tsdetails;                    
                });
                break;
            case 'submitted':
                this.panelheader = "Submitted Timesheets";
                this.panelClass = "panel panel-blue";
                if (!divSubmittedHours.hasClass("blue-triangle-down")) {
                    divSubmittedHours.addClass("blue-triangle-down")
                }
                divRecentSubmissions.removeClass("green-triangle-down");
                divRejectedHours.removeClass("red-triangle-down");
                divApprovedHours.removeClass("lightgreen-triangle-down");
                this.dashboardbusy = this.dashboardService.GetTimeSheetsByStatus(this.userId, Status.Submit).subscribe(tsdetails => {
                    this.timesheetdetails = tsdetails;                   
                });
                break;
            case 'rejected':
                this.panelheader = "Rejected Timesheets";
                this.panelClass = "panel panel-red";
                if (!divRejectedHours.hasClass("red-triangle-down")) {
                    divRejectedHours.addClass("red-triangle-down")
                }
                divSubmittedHours.removeClass("blue-triangle-down");
                divRecentSubmissions.removeClass("green-triangle-down");
                divApprovedHours.removeClass("lightgreen-triangle-down");
                this.dashboardbusy = this.dashboardService.GetTimeSheetsByStatus(this.userId, Status.Reject).subscribe(tsdetails => {
                    this.timesheetdetails = tsdetails;
                });
                break;
            case 'approved':
                this.panelheader = "Approved Timesheets";
                this.panelClass = "panel panel-lightgreen";
                if (!divApprovedHours.hasClass("lightgreen-triangle-down")) {
                    divApprovedHours.addClass("lightgreen-triangle-down")
                }
                divSubmittedHours.removeClass("blue-triangle-down");
                divRejectedHours.removeClass("red-triangle-down");
                divRecentSubmissions.removeClass("green-triangle-down");
                this.dashboardbusy = this.dashboardService.GetTimeSheetsByStatus(this.userId, Status.Approve).subscribe(tsdetails => {
                    this.timesheetdetails = tsdetails;
                });
                break;
        }
    }
}