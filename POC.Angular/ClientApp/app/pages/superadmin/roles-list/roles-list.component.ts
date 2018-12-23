import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { RolesModel } from '../../../models/superadmin-models/roles.model';
import { RolesService } from '../../../services/superadmin-services/superadmin.roles.service';
import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'superadmin-roles',
    templateUrl: './roles-list.component.html',
    styleUrls: ['./roles-list.component.scss'],
    providers: [RolesService]
})
export class RolesComponent implements OnInit {
    userId: number = 0;
    rolesdetails: Array<RolesModel> = [];
    columns: any[] = [
        { headertext: 'Role Name', name: 'Rolename', filter: 'text' },
        { headertext: 'Role Code', name: 'RoleCode', filter: 'text' },
        { headertext: 'Active/InActive', name: 'IsActive', filter: 'bool' }
    ];
    sorting: any = {
        column: 'Rolename',
        descending: false
    };
    rolesbusy: Subscription;
    constructor(private rolesService: RolesService, private router: Router, private toastr: ToastrService) {
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    ngOnInit() {
        if (this.userId > 0) {
            this.rolesbusy = this.rolesService.getAll().subscribe(rolesdata => {
                this.rolesdetails = rolesdata;
            }, (err) => { this.toastr.error(err.error); });
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    gridaction(gridaction: any): void {
        switch (gridaction.action) {
            case "edit":
                var role = gridaction.items[0].item;
                var RoleId = role.RoleID;
                this.router.navigate(['/superadmin/role', RoleId]);
                break;
            case "delete":
                var role = gridaction.items[0].item;
                var RoleID = role.RoleID;
                this.rolesbusy = this.rolesService.delete(RoleID).subscribe((rolesdata) => {
                    if (rolesdata) {
                        this.toastr.success("Deleted Successfully."); 
                    }
                }, (err) => { debugger; this.toastr.error(err.error); });

                break;
        }
    }
}