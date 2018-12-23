import { UsersModel } from "./users.model";
import { ProjectModel } from "./project.model";

export class AssignProjectModel {
    ProjectId?: number;
    ManagerId?: number;  
    Projects: Array<ProjectModel>;  
    Users: Array<UsersModel>;  
}
