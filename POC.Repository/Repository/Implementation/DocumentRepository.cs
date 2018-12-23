using System;
using System.Collections.Generic;
using System.Linq;
using POC.Repository.Interface;
using POC.Models;
using POC.ViewModels;
using System.Web.Services.Protocols;

namespace POC.Repository.Implementation
{
    public class DocumentRepository : IDocumentRepository
    {
        public int Add(Documents Documents)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    _context.Documents.Add(Documents);
                    _context.SaveChanges();
                    int id = Documents.DocumentID;
                    return id;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
        public Documents GetById(long DocumentId)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var tempDocument = (from document in _context.Documents
                                        where document.DocumentID == DocumentId
                                        select document).FirstOrDefault();
                    return tempDocument;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<DocumentsVM> GetAll(long projectId, int userId)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    if (projectId < 1)
                    {
                        var projects = GetAssignedProjects(userId);
                        var tempDocument = (from document in _context.Documents
                                            join doctypes in _context.DocumentTypes on document.DocumentTypeId equals doctypes.DocumentTypeId
                                            join pm in _context.ProjectMaster on document.ProjectId equals pm.ProjectID into pmSet
                                            from pmlist in pmSet.DefaultIfEmpty()
                                            where projects.Contains(document.ProjectId)
                                            select new DocumentsVM
                                            {
                                                DocumentID = document.DocumentID,
                                                ProjectName= pmlist.ProjectName,
                                                DocumentTitle = document.DocumentTitle,
                                                DocumentType = doctypes.DocumentType,
                                                ProjectId = document.ProjectId,
                                                FileNameUrl = document.FileNameUrl,
                                                AssignedToId = document.AssignedToId,
                                                CreatedOn = document.CreatedOn
                                            }).ToList();
                        return tempDocument;
                    }
                    else if (projectId > 0)
                    {
                        var tempDocument = (from document in _context.Documents
                                            join doctypes in _context.DocumentTypes on document.DocumentTypeId equals doctypes.DocumentTypeId
                                            join pm in _context.ProjectMaster on document.ProjectId equals pm.ProjectID into pmSet
                                            from pmlist in pmSet.DefaultIfEmpty()
                                            where document.ProjectId == projectId
                                            select new DocumentsVM
                                            {
                                                DocumentID = document.DocumentID,
                                                ProjectName = pmlist.ProjectName,
                                                DocumentTitle = document.DocumentTitle,
                                                DocumentType = doctypes.DocumentType,
                                                ProjectId = document.ProjectId,
                                                FileNameUrl = document.FileNameUrl,
                                                AssignedToId = document.AssignedToId,
                                                CreatedOn = document.CreatedOn
                                            }).ToList();
                        return tempDocument;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Delete(long DocumentId)
        {
            try
            {
                try
                {
                    using (var _context = new DatabaseContext())
                    {
                        var document = (from doc in _context.Documents
                                        where doc.DocumentID == DocumentId
                                        select doc).SingleOrDefault(); ;
                        if (document != null)
                        {
                            _context.Documents.Remove(document);
                            int resultProject = _context.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.InnerException.ToString().Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                    {
                        throw new Exception("This Document cannot be deleted");
                    }
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public byte[] GetAttachmentById(long DocumentId)
        {
            var docbytes = new byte[1000];
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var document = (from doc in _context.Documents
                                    where doc.DocumentID == DocumentId
                                    select doc).FirstOrDefault();
                    docbytes = document.DocumentBytes;
                    return docbytes;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public byte[] Generate_SSRSReport(string reportName, string reporttype, List<dynamic> parameters)
        {
            byte[] fileData = new byte[10000];
            try
            {
                //var rsExec = new ReportExecutionService();
                //var username = "";
                //var password = "";
                //var domainname = "";
                //rsExec.Credentials = new System.Net.NetworkCredential(username, password, domainname);
                //string historyID = null;
                //string reportPath = "";              
                //ExecutionHeader execHeader = new ExecutionHeader();
                //ServerInfoHeader serviceInfo = new ServerInfoHeader();
                //TrustedUserHeader trusteduserHeader = new TrustedUserHeader();
                //var execInfo = rsExec.LoadReport(reportPath, null);
                //if (parameters != null)
                //    rsExec.SetExecutionParameters(parameters.ToArray(), "en-us");
                //else
                //{
                //    parameters = new List<ParameterValue>();
                //    rsExec.SetExecutionParameters(parameters.ToArray(), "en-us");
                //}
                //var deviceInfo = "<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";
                //var extension = string.Empty;
                //string encoding = string.Empty;
                //Warning[] warnings = null;
                //string mimeType = String.Empty;
                //string[] streamIDs = null;
                //string _reportformat = "";
                //switch (reporttype.ToLower())
                //{
                //    case "pdf":
                //        _reportformat = "PDF";
                //        break;
                //    case "excel":
                //        _reportformat = "Excel";
                //        break;
                //    case "csv":
                //        _reportformat = "CSV";
                //        break;
                //    case "word":
                //        _reportformat = "Word";
                //        break;
                //    default:
                //        _reportformat = "PDF";
                //        break;
                //}
                //Byte[] result;
                //result = rsExec.Render(_reportformat, deviceInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);
                //fileData = result;
                return fileData;
            }
            catch (SoapException soapex)
            {
                throw new Exception(soapex.Message);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }

        }
        private List<int> GetAssignedProjects(int userId)
        {
            var _context = new DatabaseContext();
            var projectIds = (from projectmaster in _context.ProjectMaster
                              join _assignedProjects in _context.AssignedProjects on projectmaster.ProjectID equals _assignedProjects.ProjectId into gj
                              from projectset in gj.DefaultIfEmpty()
                              join user in _context.Registration on projectset.ManagerId equals user.RegistrationID into rj
                              from userset in rj.DefaultIfEmpty()
                              where userset.RegistrationID == userId
                              select projectmaster.ProjectID).ToList();
            return projectIds;
        }
    }
}
