import { ValueDescriptionModel } from "../value-description.model";

export class UsersModel {
    RegistrationID: number;
    FirstName: string;
    MiddleName: string;
    LastName: string;
    FullName: string;
    Birthdate: Date;
    Mobileno: string;
    Gender: string;
    EmailID: string;
    Username: string;
    Password: string;
    ConfirmPassword: string;
    AssignToAdmin: string;
    WorkEmail: string;
    RoleID?: number;
    RoleName: string;
    IsActive?: boolean=true;
    EmployeeID: string;
    Roles: Array<ValueDescriptionModel>;
    Departments: Array<ValueDescriptionModel>;
    Jobs: Array<ValueDescriptionModel>;
    Managers: Array<ValueDescriptionModel>;
    DateofJoining: Date;
    DateofLeaving: Date;
    EmergencyContact: string;
    EmergencyContactNumber: string;
    JobId?: number;
    DeptId?: number;
    ManagerId?: number;
    selectedUsers?: boolean=false;
}

    