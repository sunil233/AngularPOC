import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { ProjectModel } from '../../../models/admin-models/admin.project.model';
import { ProjectService } from '../../../services/admin-services/admin.project.service';

@Component({
    selector: 'superadmin-remove-project',
    templateUrl: './remove-project.component.html',
    styleUrls: ['./remove-project.component.scss'],
    providers: [ProjectService]
})
export class RemoveProjectComponent implements OnInit {
    userId: number = 0;
    projects: Array<ProjectModel>=[];
    projectsbusy: Subscription;
 
    columns: any[] = [
        { headertext: 'Project Name', name: 'ProjectName', filter: 'text' },
        { headertext: 'Project Code', name: 'ProjectCode', filter: 'text' },
        { headertext: 'Project Manager', name: 'ManagerName', filter: 'text' },
    ];
    sorting: any = {
        column: 'LastName',
        descending: false
    };
    gridbtns: any[] = [
        { title: 'Remove Project', keys: ['Id'], action: "Removeproject", ishide: false }
    ];
    constructor(private projectService: ProjectService,
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
        this.GetAllProjects();
    }
    gridaction(gridaction: any): void {
        switch (gridaction.action.action) {
            case "Removeproject":
                var project = gridaction.items[0].item;
                var regId = project.ManagerId;
                var projectId = project.ProjectID;
                this.projectsbusy = this.projectService.RemoveProject(regId, projectId).subscribe(data => {
                    if (data) {
                        this.toastr.success('Project Unlinked Successfully.');
                        this.GetAllProjects();
                    }
                }, (err) => { this.showError(err) });
                break;
        }
    }
    GetAllProjects() {
        this.projectsbusy = this.projectService.GetAllByRole("User").subscribe(projectsdata => {
            this.projects = projectsdata;
        });
    }
    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }
}