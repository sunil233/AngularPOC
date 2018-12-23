import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { RolesModel } from '../../../models/superadmin-models/roles.model';
import { RolesService } from '../../../services/superadmin-services/superadmin.roles.service';

@Component({
    selector: 'superadmin-remove-manager',
    templateUrl: './remove-manager.component.html',
    styleUrls: ['./remove-manager.component.scss'],
    providers: [RolesService]
})
export class RemoveManagerComponent implements OnInit {
    userId: number = 0;
    roles: Array<RolesModel>;
    rolesbusy: Subscription;
    projectForm: FormGroup;
    columns: any[] = [
        { headertext: 'Employee Name', name: 'FullName', filter: 'text' },
        { headertext: 'Manager Name', name: 'AssignToAdmin', filter: 'text' }
    ];
    sorting: any = {
        column: 'LastName',
        descending: false
    };
    gridbtns: any[] = [
        { title: 'Remove Manager', keys: ['Id'], action: "Removefromrole", ishide: false }
    ];
    constructor(private rolesService: RolesService,
        private router: Router,
        private toastr: ToastrService) {
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    ngOnInit() {
        this.GetAllAssignedEmployees();
    }
    gridaction(gridaction: any): void {
        switch (gridaction.action.action) {
            case "Removefromrole":
                var role = gridaction.items[0].item;
                var regId = role.RegistrationID;
                this.rolesbusy = this.rolesService.RemovefromRole(regId).subscribe(data => {
                    if (data) {
                        this.toastr.success('Manager removed Successfully.');
                        this.GetAllAssignedEmployees();
                    }
                }, (err) => { this.showError(err) });
                break;
        }
    }
    GetAllAssignedEmployees() {
        this.rolesbusy = this.rolesService.ShowallRoles().subscribe(rolesdata => {
            this.roles = rolesdata;
        });
    }
    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }
}