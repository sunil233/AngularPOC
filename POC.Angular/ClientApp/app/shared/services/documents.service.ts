import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { saveAs } from 'file-saver';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/Rx';
import { AppConfig } from '../../app.configuration';

@Injectable()
export class DocumentService {
    private baseUrl: string;
    constructor(private http: HttpClient, private _config: AppConfig) {
        this.baseUrl = this._config.ApiUrl +"api/Documents/";
    }
    public downloadAttachment(documentId: number, fileName: string): Observable<any> {
        var url = this.baseUrl + "DownloadAttachment?documentId=" + documentId;
        return this.http.get(url, { responseType: 'blob' })
            .map(res => saveFile(res, fileName));
    }
    /**
     * method to download data with params
        below is the format to pass the data
        var params = [{ nameField: "Param1", valueField: "1" }, { nameField: "Param2", valueField: "2" }, { nameField: "Param3", valueField: "4" }];
        var reportdata = { "ReportName": reportName, "ReportType": "pdf", "reportparams": params }
     * @param data
     */
    public downloadFileWithParams(data: any, fileName: string): Observable<any> {
        var url = this.baseUrl + "DownloadFile";
        const reqOpts = { params: new HttpParams() };
        reqOpts.params.set('Accept', 'application/json');
        var reportData = JSON.stringify(data);
        return this.http.post(url, reportData, { responseType: 'blob' })
            .map(res => saveFile(res, fileName));
    }
    /**
    * method to get bytes array from the api Service*
    * @param data
    */
    public downloadfilewithbytes(data: any): Observable<any> {
        var url = this.baseUrl + "DownloadFile";
        const reqOpts = { params: new HttpParams() };
        reqOpts.params.set('Accept', 'application/json');
        var reportData = JSON.stringify(data);
        return this.http.post(url, reportData, { responseType: 'arraybuffer' });
    }
    public DeleteDocument(documentId: number): Observable<any> {
        var url = this.baseUrl + "Delete?documentId=" + documentId;
        return this.http.get(url);
    }
    public getDocumentsById(documentId): Observable<any> {
        var url = this.baseUrl + "GetById?documentId=" + documentId;
        return this.http.get(url);
    }
    public GetAll(projectId: number,userId: number): Observable<any> {
        var url = this.baseUrl + "GetAll?projectId=" + projectId + "&userId=" + userId;
        return this.http.get(url);
    }
    public SaveDocument(data: Document) {
        var url = this.baseUrl + "Add";
        return this.http.post(url, JSON.stringify(data));
    }
    public UpdateDocument(data: Document) {
        var url = this.baseUrl + "api/Documents/UpdateDocument";
        return this.http.post(url, JSON.stringify(data));
    }
}
export const saveFile = (blobContent: Blob, fileName: string) => {
    const blob = new Blob([blobContent], { type: 'application/octet-stream' });
    saveAs(blob, fileName);
};

