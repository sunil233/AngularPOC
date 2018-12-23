import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { Subscription } from 'rxjs';
import { TimeSheetDetails } from '../../../models/admin-models/admin.timesheet.model';
import { ProjectModel } from '../../../models/admin-models/admin.project.model';
import { TaskModel } from '../../../models/admin-models/admin.tasks.model';
import { TimeSheetData } from '../../../models/admin-models/admin.timesheetdata';
import { AdminTimeSheetService } from '../../../services/admin-services/admin.timesheet.service';
import { UserTimeSheetService } from '../../../services/user-services/user.timesheet';
import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'app-timesheet',
    templateUrl: './timesheet.component.html',
    styleUrls: ['./timesheet.component.scss'],
    providers: [UserTimeSheetService, AdminTimeSheetService]

})
export class AdminTimesheetComponent implements OnInit {

    //  ----------- ***************  Declarations   ****************

    public userId: number = 0;
    public TimesheetMasterId: number = 0;
    public currentDate = moment();
    public WeekCount: number = 0;
    public start: any = moment().startOf('week').add(this.WeekCount, "weeks");
    public end: any = moment().endOf('week').add(this.WeekCount, "weeks");
    public StartOfWeek: string = this.start.format('MM/DD/YYYY');
    public EndOfWeek: string = this.end.format('MM/DD/YYYY');
    public dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    public TimeSheetList: Array<TimeSheetDetails[]> = [];
    public TimeSheetStatus: number = 0;
    public projects: Array<ProjectModel> = [];
    public taskdetails: Array<TaskModel> = [];
    public timesheetdata: TimeSheetData;
    public selectedProject = [];
    public selectedTask = [];
    public iscontrolDisabled: boolean = false;
    public comments: string = "";
    timesheetbusy: Subscription;

    //----------- *************** End of Declarations ****************

    constructor(private _userTimeSheetService: UserTimeSheetService,
        private _adminTimeSheetService: AdminTimeSheetService,
        private route: ActivatedRoute,
        private toastr: ToastrService) {
        if (this.route.snapshot.paramMap.get('timesheetmasterId') != undefined && this.route.snapshot.paramMap.get('timesheetmasterId') != null) {
            this.TimesheetMasterId = parseInt(this.route.snapshot.paramMap.get('timesheetmasterId'));
        }
        else {
            this.TimesheetMasterId = 0;
        }
        this.generateCalendar();
        if (localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
        }
    }
    ngOnInit() {
        this.gettimesheetdata(this.TimesheetMasterId);
    }
    fillDates(start) {
        var myWeekDays = [];
        var selWeek = moment(start);
        for (var i = 0; i < 7; i++) {
            var selDate = moment(selWeek).add(i, "days").format();
            var tm = new TimeSheetDetails();
            tm.DaysofWeek = this.dayNames[moment(selDate).weekday()];
            tm.Period = moment(selDate).format("MM/DD/YYYY");
            myWeekDays.push(tm);
        }
        return myWeekDays;
    }
    generateCalendar() {
        const dates = this.fillDates(this.StartOfWeek);
        const myweeks: Array<TimeSheetDetails[]> = [];
        while (dates.length > 0) {
            myweeks.push(dates.splice(0, 7));
        }
        this.TimeSheetList = myweeks;
    }
    setDates(range) {
        var v_start = null, v_end = null;
        if (range === null) {
            v_start = moment().startOf('week').add(this.WeekCount, "weeks");
            v_end = moment().endOf('week').add(this.WeekCount, "weeks");
        } else {
            v_start = range.start;
            v_end = range.end;
        }
        this.StartOfWeek = v_start.format('MM/DD/YYYY');
        this.EndOfWeek = v_end.format('MM/DD/YYYY');
    }
    gettimesheetdata(TimesheetMasterId: number): void {
        if (TimesheetMasterId > 0) {
            this.timesheetbusy = this._userTimeSheetService.GetTimeSheetByMasterId(TimesheetMasterId).subscribe(data => {
                this.updatedata(data);
            })
        }
        this.timesheetbusy = this._userTimeSheetService.GetProjects().subscribe(projectdetails => {
            this.projects = projectdetails;
        });
    }
    updatedata(data): void {
        this.TimeSheetList = data.ListTimeSheetDetails;
        this.TimeSheetStatus = data.TimeSheetStatus;
        if (data.ListTimeSheetDetails == null) {
            this.generateCalendar();
            this.iscontrolDisabled = false;
            this.selectedTask = [];
            this.selectedProject = [];
        }
        else {
            data.ListTimeSheetDetails.forEach((item, index) => {
                var projectId = 0;
                if (item != null && item[0].ProjectID != undefined) {
                    projectId = item[0].ProjectID;
                    var ProjectName = item[0].ProjectName
                    this.selectedProject[index] = ProjectName;
                    this._userTimeSheetService.GetTasksByProjects(projectId).subscribe(taskdetails => {
                        this.taskdetails[index] = taskdetails;
                        this.selectedTask[index] = item[0].TaskName;
                    });
                }
            });
            if (this.TimeSheetStatus == 4) {
                this.iscontrolDisabled = true;
            }
            else {
                this.iscontrolDisabled = false;
            }
        }
    }
    Reject() {
        if (this.TimesheetMasterId != undefined) {
            var data = { "TimeSheetMasterID": this.TimesheetMasterId, "comment": this.comments, "userId": this.userId };
            this.timesheetbusy = this._adminTimeSheetService.Rejected(data).subscribe(data => {
            }, (err) => { this.toastr.warning(err.error); });
        }
    }
    Approve() {
        if (this.TimesheetMasterId != undefined) {
            var data = { "TimeSheetMasterID": this.TimesheetMasterId, "comment": this.comments, "userId": this.userId };
            this.timesheetbusy = this._adminTimeSheetService.Approval(data).subscribe(data => {
            }, (err) => { this.toastr.warning(err.error); });
        }
    }
}