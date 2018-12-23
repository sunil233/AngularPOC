import { TaskModel } from "./admin.tasks.model"

export class TimeSheetDetails {
    DaysofWeek: string;
    Period: string;
    Hours: number;
    ProjectID: number;
    TaskID: number;
    tasks: TaskModel[];
    TimeSheetID: number;
    UserID: number;

}
