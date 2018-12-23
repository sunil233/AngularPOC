import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ProjectModel } from '../../../models/admin-models/admin.project.model';
import { DocumentsModel } from '../../../models/admin-models/admin.documents.model';
import { DocumentService } from '../../../shared/services/documents.service';
import { ProjectService } from '../../../services/admin-services/admin.project.service';
import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'admin-documents-list',
    templateUrl: './documents.component.html',
    styleUrls: ['./documents.component.scss'],
    providers: [DocumentService, ProjectService]
})
export class DocumentListComponent implements OnInit {
    userId: number = 0;
    projectId: number = 0;
    documents: Array<DocumentsModel> = [];
    projectsdetails: Array<ProjectModel> = [];
    columns: any[] = [
        { headertext: 'Project Name', name: 'ProjectName', filter: 'text' },
        { headertext: 'Document Title', name: 'DocumentTitle', filter: 'text' },
        { headertext: 'File Name', name: 'FileNameUrl', filter: 'text' },
        { headertext: 'Document Type', name: 'DocumentType', filter: 'text' }
    ];
    sorting: any = {
        column: 'DocumentTitle',
        descending: false
    };
    documentsbusy: Subscription;
    constructor(private documentService: DocumentService, private projectService: ProjectService, private router: Router, private toastr: ToastrService) {
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
            this.getProjects();
            this.getDocuments();
        }
    }
    getProjects() {
        this.documentsbusy = this.projectService.getAssignedProjects(this.userId).subscribe(projectsdata => {
            this.projectsdetails = projectsdata;
        }, (err) => { this.showError(err) });
    }
    getDocuments() {
        this.documentsbusy = this.documentService.GetAll(this.projectId,this.userId).subscribe(documents => {
            this.documents = documents;
        }, (err) => { this.showError(err) });
    }
    onDocumentSearch() {
        this.getDocuments();
    }
    gridaction(gridaction: any): void {
        switch (gridaction.action) {
            case "download":
                var Document = gridaction.items[0].item;
                var DocumentID = Document.DocumentID;
                var fileName = Document.FileNameUrl;
                this.documentsbusy = this.documentService.downloadAttachment(DocumentID, fileName).subscribe((projectdata) => {
                    if (projectdata) {
                       
                    }
                }, (err) => { this.toastr.warning(err.error); });
                break;
            case "delete":
                var Document = gridaction.items[0].item;
                var DocumentID = Document.DocumentID;
                this.documentsbusy = this.documentService.DeleteDocument(DocumentID).subscribe((projectdata) => {
                    if (projectdata) {
                        this.toastr.success("Deleted Successfully.");
                    }
                }, (err) => { this.toastr.warning(err.error); });

                break;
        }
    }
    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }
}