export class ProjectModel {
    ProjectID: number;
    ProjectCode?: string = "";
    ProjectName?: string = "";
    IsActive: boolean;
    IsDeleted: boolean;
    RegistrationID: number;
    selectedProject?: boolean = false;
    ManagerName: string = "";
    Status: string = "";   
}
