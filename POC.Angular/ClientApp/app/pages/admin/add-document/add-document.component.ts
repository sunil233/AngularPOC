import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ProjectModel } from '../../../models/admin-models/admin.project.model';
import { ProjectService } from '../../../services/admin-services/admin.project.service';
import { DocumentsModel } from '../../../models/admin-models/admin.documents.model';
import { FileUploader, FileItem } from 'ng2-file-upload';
import { AppConfig } from '../../../app.configuration';
import { DocumentService } from '../../../shared/services/documents.service';
@Component({
    selector: 'admin-add-document',
    templateUrl: './add-document.component.html',
    styleUrls: ['./add-document.component.scss'],
    providers: [ProjectService, DocumentService]
})
export class DocumentComponent implements OnInit {
    userId: number = 0;
    projectId: number = 0;
    public uploader: FileUploader;
    uploadURL: string;
    projectdetails: Array<ProjectModel> = [];
    taskbusy: Subscription;
    documentForm: FormGroup;
    isDisable: boolean = false;
    document: DocumentsModel;
    constructor(private router: Router,
        private fb: FormBuilder,
        private toastr: ToastrService,
        private appconfig: AppConfig,
        private projectService: ProjectService, private documentService: DocumentService) {
        this.uploadURL = this.appconfig.ApiUrl + "api/Documents/Upload";
        this.document = new DocumentsModel();
        if (localStorage["UserId"] != undefined && localStorage["UserId"] != null) {
            this.userId = localStorage["UserId"];      
            this.document.DocumentTypeId = -1;
            this.document.ProjectId = -1;
        }
        else {
            console.log("Invalid UserId.");
            this.router.navigate(['/login']);
        }
    }
    ngOnInit() {
        this.uploader = new FileUploader({ url: this.uploadURL });
        this.initializeForm();
        this.getProjects();
        // Add in the  upload form parameters.        
        this.uploader.onBuildItemForm = (item, form) => {
            form.append("ProjectId", this.document.ProjectId);
            form.append("DocumentTypeId", this.document.DocumentTypeId);
            form.append("DocumentTitle", this.document.DocumentTitle);
            form.append("DocumentDescription", this.document.DocumentDescription);
            form.append("UploadeById", this.userId);          
        };
        this.uploader.onBeforeUploadItem = (fileItem: FileItem): any => {
            return { fileItem };
        };
        this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
        this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
            console.log("document uploaded:", item, status, response);
            this.toastr.success("Document Uploaded Successfully");
        };
    }
    getProjects() {
        this.taskbusy = this.projectService.getAssignedProjects(this.userId).subscribe(projectsdata => {
            this.projectdetails = projectsdata;
        }, (err) => { this.showError(err) });
    }
    initializeForm(): void {
        this.documentForm = this.fb.group({
            'ddlProject': [null, Validators.required],
            'ddlDocumentType': [null, Validators.required],
            'DocumentTitle': [null, Validators.required],
            'DocumentDescription': [null]
        });
    }
    submitForm($ev) {
        $ev.preventDefault();
        for (let c in this.documentForm.controls) {
            this.documentForm.controls[c].markAsTouched();
        }
        if (this.documentForm.valid) {
            this.uploader.uploadAll();
        }

    }
    onCancelClick() {
        this.router.navigate(['/admin/tasks', this.projectId]);
    }
    showError(err: any) {
        if (err.error.MessageDetail != null && err.error.MessageDetail != undefined) {
            this.toastr.warning(err.error.MessageDetail);
        }
        else { this.toastr.warning(err.error); }
    }
}