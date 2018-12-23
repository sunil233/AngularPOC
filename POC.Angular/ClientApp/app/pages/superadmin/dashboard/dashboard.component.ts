import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { SuperAdminDashboardService } from '../../../services/superadmin-services/superadmin.dashboard.service';
import { UsersModel } from '../../../models/superadmin-models/users.model';
declare var $: any;
@Component({
    selector: 'superadmin-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
    providers: [SuperAdminDashboardService]
})

export class DashboardComponent implements OnInit {
    timesheetdetails: any[] = [];
    userId: number = 0;
    dashboardbusy: Subscription;
    UsersCount: number = 0;
    AdminsCount: number = 0;
    ProjectsCount: number = 0;
    adminuserdetails: Array<UsersModel> = [];
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
    panelheader: string = "";
    panelClass: string = ""  
    constructor(private dashboardService: SuperAdminDashboardService, private router: Router) {
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
        }
    }
    ngOnInit() {
        if (this.userId > 0) {
            this.dashboardbusy= this.dashboardService.Dashboard(this.userId).subscribe(dashboardData => {
                if (dashboardData != null && dashboardData != undefined) {
                    if (dashboardData.usersCount != undefined) {
                        this.UsersCount = dashboardData.usersCount;
                    }
                    if (dashboardData.adminCount != undefined) {
                        this.AdminsCount = dashboardData.adminCount;
                    }
                    if (dashboardData.projectCount != undefined) {
                        this.ProjectsCount = dashboardData.projectCount;
                    }
                }
                 this.LoadData("users");
            });
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    onCardClick(cardtitle) {
        this.LoadData(cardtitle);
    }
    LoadData(divName): void {
        var divUser = $('#divUser');
        var divAdmins = $('#divAdmins');
        var divProjects = $('#divProjects');
        switch (divName) {
            case 'users': //green
                this.panelheader = "Users";
                this.panelClass = "panel panel-green";
                if (!divUser.hasClass("green-triangle-down")) {
                    divUser.addClass("green-triangle-down")
                }
                divAdmins.removeClass("blue-triangle-down");
                divProjects.removeClass("red-triangle-down");              
                this.dashboardbusy = this.dashboardService.GetAllUsers().subscribe(userdata => {
                    this.adminuserdetails = userdata;
                });
                break;
            case 'admin'://blue
                this.panelheader = "Admins";
                this.panelClass = "panel panel-blue";
                if (!divAdmins.hasClass("blue-triangle-down")) {
                    divAdmins.addClass("blue-triangle-down")
                }
                divUser.removeClass("green-triangle-down");
                divProjects.removeClass("red-triangle-down");              
                this.dashboardbusy = this.dashboardService.GetAllAdminUsers().subscribe(userdata => {
                    this.adminuserdetails = userdata;
                });
                break;
            case 'projects': //red
                this.panelheader = "Projects";
                this.panelClass = "panel panel-red";
                if (!divProjects.hasClass("red-triangle-down")) {
                    divProjects.addClass("red-triangle-down")
                }
                divAdmins.removeClass("blue-triangle-down");
                divUser.removeClass("green-triangle-down");
                this.router.navigate(['/superadmin/projects-list']);
                break;
            
        }
    }
    gridaction(gridaction: any): void {
        switch (gridaction.action) {
            case "edit":
                var user = gridaction.items[0].item;
                var RegistrationID = user.RegistrationID;
                if (this.panelheader == "Users") {
                    this.router.navigate(['/superadmin/user', RegistrationID]);
                }
                else if (this.panelheader == "Admins")
                {
                    this.router.navigate(['/superadmin/user', RegistrationID]);
                }
                else if (this.panelheader == "Projects") {
                    this.router.navigate(['/superadmin/projects-list']);
                }
                break;
        }
    }
}