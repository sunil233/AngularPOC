import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

import { TimeSheetDetails } from '../../../models/user-models/user.timesheet.model';
import { TaskModel } from '../../../models/user-models/user.task.model';
import { ProjectModel } from '../../../models/user-models/user.project.model';
import { TimeSheetData } from '../../../models/user-models/user.timesheetdata';
import { UserTimeSheetService } from '../../../services/user-services/user.timesheet';
import { ProjectService } from '../../../services/user-services/user.project.service';

@Component({
    selector: 'timesheet-dashboard',
    templateUrl: './timesheet.component.html',
    styleUrls: ['./timesheet.component.scss'],
    providers: [UserTimeSheetService, ProjectService]
})
export class TimeSheetComponent implements OnInit {

    // public timesheetdetails: any[] = [];
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
    timesheetbusy: Subscription;
    //----------- *************** End of Declarations ****************

    constructor(private _userTimeSheetService: UserTimeSheetService,
        private _projectService: ProjectService,
        private route: ActivatedRoute,
        private toastr: ToastrService) {
        if (this.route.snapshot.paramMap.get('timesheetmasterId') != undefined && this.route.snapshot.paramMap.get('timesheetmasterId') != null) {
            this.TimesheetMasterId = parseInt(this.route.snapshot.paramMap.get('timesheetmasterId'));
        }
        this.generateCalendar();
        if (localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
        }
    }
    ngOnInit(): void {
        this.gettimesheetdata();
    }
    gettimesheetdata() {
        if (this.TimesheetMasterId > 0) {
            this.timesheetbusy = this._userTimeSheetService.GetTimeSheetByMasterId(this.TimesheetMasterId).subscribe(data => {
                this.updatedata(data);
            }, (err) => { this.showError(err) });
        }
        else {
            var userdata = {
                "UserId": this.userId,
                "Startdate": this.StartOfWeek,
                "Enddate": this.EndOfWeek
            };
            this.timesheetbusy = this._userTimeSheetService.GetTimeSheetForUserById(userdata).subscribe(data => {
                this.updatedata(data);
            }, (err) => { this.showError(err) });
        }
        this.timesheetbusy = this._projectService.getAssignedProjects(this.userId).subscribe(projectdetails => {
            this.projects = projectdetails;
        }, (err) => { this.showError(err) });
    }
    addRow() {
        var v_start = moment().startOf('week').add(this.WeekCount, "weeks");
        this.StartOfWeek = v_start.format('MM/DD/YYYY');
        var v_week = this.fillDates(v_start);
        this.TimeSheetList.push(v_week);
    }
    deleteRow(index) {
        if (this.TimeSheetList.length > 1) {
            this.TimeSheetList.splice(index, 1);
            this.timesheetbusy = this._userTimeSheetService.Delete(this.TimesheetMasterId, this.userId).subscribe(data => {
                if (data) {
                    this.toastr.success('Saved Successfully.');
                }
            }, (err) => { this.showError(err) });
        }
    }
    updatedata(data) {
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
                    this.selectedProject[index] = projectId;
                    this._userTimeSheetService.GetTasksByProjects(projectId).subscribe(taskdetails => {
                        this.taskdetails[index] = taskdetails;
                        this.selectedTask[index] = item[0].TaskID;
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
    save() {
        this.saveData("Save");
    }
    submittosupervisor() {
        this.saveData("SubmitToSupervisor");
    }
    saveData(ActionType) {
        this.timesheetdata = new TimeSheetData();
        this.timesheetdata.FromDate = new Date(this.StartOfWeek);
        this.timesheetdata.ToDate = new Date(this.EndOfWeek);
        this.timesheetdata.TimeSheetMasterID = this.TimesheetMasterId;
        this.timesheetdata.TotalHours = 40;
        this.timesheetdata.ActionType = ActionType;
        this.timesheetdata.Comments = "";
        this.timesheetdata.UserId = this.userId;
        this.timesheetdata.timeSheetList = this.TimeSheetList;
        this._userTimeSheetService.SaveTimeSheet(this.timesheetdata).subscribe(data => {
            if (data) {
                this.toastr.success('Saved Successfully.');
            }
        }, (err) => { this.toastr.warning(err.error); });
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
    generateCalendar(): void {
        const dates = this.fillDates(this.StartOfWeek);
        const myweeks: Array<TimeSheetDetails[]> = [];
        while (dates.length > 0) {
            myweeks.push(dates.splice(0, 7));
        }
        this.TimeSheetList = myweeks;
    }
    GetPreviousWeek() {
        this.WeekCount--;
        this.setDates(null);
        this.gettimesheetdata();
    }
    GetNextWeek() {
        this.WeekCount++;
        this.setDates(null);
        this.gettimesheetdata();
    }
    GetCurrentWeek() {
        this.WeekCount = 0;
        this.setDates(null);
        this.gettimesheetdata();
    }
    OnProjectChange(projectId, week, index) {
        this.selectedProject[index] = projectId;
        var userId = this.userId;
        week.forEach(function (day) {
            day["ProjectID"] = projectId;
            day["UserID"] = userId;
        });
        this._userTimeSheetService.GetTasksByProjects(projectId).subscribe(taskdetails => {
            this.taskdetails[index] = taskdetails;
        }, (err) => { this.toastr.warning(err.error); });
    }
    OnTaskChange(taskId, week, index) {
        this.selectedTask[index] = taskId;
        week.forEach(function (day) {
            day["TaskID"] = taskId;
        });
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
    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }
}


