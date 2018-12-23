import { UsersModel } from "./admin.users.model";
import { ProjectModel } from "./admin.project.model";

export class AssignProjectModel {
    ProjectId?: number;
    ManagerId?: number;  
    Projects: Array<ProjectModel>;  
    Users: Array<UsersModel>;  
}
