import { UsersModel } from "./users.model";

export class AssignRolesModel {
    AssignToAdmin?: number;
    RegistrationID?: number;
    ListofAdmins: Array<UsersModel>;
    ListofUser: Array<UsersModel>;  
}
