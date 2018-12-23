export class TaskModel {
    TaskID: number;
    ProjectID?: number=0;
    ProjectName?: string = "";
    Taskname: string;
    IsActive: boolean = false;
    Status: number=0;
    Comments: string = "";
    StatusType: string = "";
    AssignedtoID: number = 0;
    AssignedtoUser: string = "";
}
