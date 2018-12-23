using POC.Models;
using POC.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;


namespace Acys.WebApi.Controllers
{
    public class DocumentsController : ApiController
    {
        private readonly ILogger _logger;
        private readonly IDocumentRepository _documentService;
        public DocumentsController(ILogger logger, IDocumentRepository documentService)
        {
            _logger = logger;
            _documentService = documentService;
        }

        /// <summary>
        /// Method to Upload documents
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Upload()
        {

            int ProjectId = 0, AssignedToId = 0, UploadeById = 0, DocumentTypeId = 0;
            string DocumentTitle = string.Empty, DocumentDescription = string.Empty;
            DateTime EffectiveDate = DateTime.Now;
            byte[] fileData = null;
            var result = new HttpResponseMessage();
            var httpContext = HttpContext.Current;
            var httpRequest = httpContext.Request;
            try
            {
                if (!string.IsNullOrEmpty(httpRequest.Form["ProjectId"]))
                {
                    ProjectId = Convert.ToInt32(httpRequest.Form["ProjectId"]);
                }
                if (!string.IsNullOrEmpty(httpRequest.Form["AssignedToId"]))
                {
                    AssignedToId = Convert.ToInt32(httpRequest.Form["AssignedToId"]);
                }
                if (!string.IsNullOrEmpty(httpRequest.Form["DocumentType"]))
                {
                    DocumentTypeId = Convert.ToInt32(httpRequest.Form["DocumentTypeId"]);
                }
                if (!string.IsNullOrEmpty(httpRequest.Form["DocumentTitle"]))
                {
                    DocumentTitle = httpRequest.Form["DocumentTitle"];
                }
                if (!string.IsNullOrEmpty(httpRequest.Form["UploadeById"]))
                {
                    UploadeById = Convert.ToInt32(httpRequest.Form["UploadeById"]);
                }
                if (!string.IsNullOrEmpty(httpRequest.Form["DocumentDescription"]))
                {
                    DocumentDescription = httpRequest.Form["DocumentDescription"];
                }
                var postedFile = httpRequest.Files[0];
                string FileName = postedFile.FileName;
                string contentType = postedFile.ContentType;
                using (var binaryReader = new BinaryReader(postedFile.InputStream))
                {
                    fileData = binaryReader.ReadBytes(postedFile.ContentLength);
                    var document = new Documents()
                    {
                        ProjectId = ProjectId,
                        DocumentTitle = DocumentTitle,
                        DocumentTypeId = DocumentTypeId,
                        AssignedToId = AssignedToId,
                        UploadedBy = UploadeById,
                        FileNameUrl = FileName,
                        CreatedOn = DateTime.Now,
                        DocumentBytes = fileData,
                        DocumentDescription = DocumentDescription
                    };
                    var docId = _documentService.Add(document);
                    if (docId > 0)
                        return Ok("Uploaded successfully");
                    else
                        return Ok("Uploading failed");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to get document By id
        /// </summary>
        /// <param name="documentId">documentId</param>
        /// <returns>Document details</returns>
        [HttpGet]
        public IHttpActionResult GetById(long documentId)
        {
            var document = _documentService.GetById(documentId);
            return Ok(document);
        }

        [HttpGet]
        public IHttpActionResult GetAll(long projectId, int userId = 0)
        {
            var documents = _documentService.GetAll(projectId, userId);
            return Ok(documents);
        }

        /// <summary>
        /// Method to delete documents
        /// </summary>
        /// <param name="documentId">documentId</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Delete(long documentId)
        {
            var isDeleted = _documentService.Delete(documentId);
            return Ok(isDeleted);
        }

        /// <summary>
        /// Method to Download SSRS Report
        /// </summary>
        /// <param name="reportData"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DownloadReport(ReportData reportData)
        {

            HttpResponseMessage result = null;
            string ReportName = "";
            byte[] FileData = new byte[10000];
            FileData = _documentService.Generate_SSRSReport(reportData.ReportName, reportData.ReportType, reportData.Reportparams);
            MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("application/octet-stream");
            result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(FileData);
            result.Content.Headers.ContentType = mediaType;
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = ReportName
            };
            return ResponseMessage(result);
        }

        /// <summary>
        /// Method to download attachment
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult DownloadAttachment(long documentId)
        {
            HttpResponseMessage result = null;
            byte[] FileData = new byte[10000];
            var document = _documentService.GetById(documentId);
            FileData = document.DocumentBytes;
            if (FileData != null)
            {
                MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("application/octet-stream");
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(FileData);
                result.Content.Headers.ContentType = mediaType;
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = document.FileNameUrl
                };
                return ResponseMessage(result);
            }
            else
            {
                return Ok("No data found");
            }

        }
    }
    public class ReportData
    {
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public List<dynamic> Reportparams { get; set; }
    }
    public class DocumentsData
    {
        public int countyID { get; set; }
        public int subCategoryID { get; set; }
        public List<int> PhotoIds { get; set; }
    }
}