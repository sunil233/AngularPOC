import { TimeSheetDetails } from "./user.timesheet.model";
export class TimeSheetData {
    FromDate: Date;
    ToDate: Date;
    TimeSheetMasterID: number;
    TotalHours: number;
    ActionType: string;
    Comments: string;
    UserId: number;   
    timeSheetList: Array<TimeSheetDetails[]>
}
