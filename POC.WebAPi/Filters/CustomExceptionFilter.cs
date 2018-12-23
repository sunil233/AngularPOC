using System.Data.Entity.Validation;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace POC.WepApi.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string exceptionMessage = string.Empty;
           
            if (actionExecutedContext.Exception.InnerException == null)
            {
                exceptionMessage = actionExecutedContext.Exception.Message;
            }
            else if (actionExecutedContext.Exception is DbEntityValidationException)
            {

                var e = actionExecutedContext.Exception as DbEntityValidationException;
                //  var errors = new List<POCErrors>();
                StringBuilder sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //errors.Add(new POCErrors { ErrorProperty = ve.PropertyName, ErrorDescription = ve.ErrorMessage });
                        sb.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"", ve.PropertyName, eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName), ve.ErrorMessage));
                    }
                    var errorMessagError = sb.ToString();
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessagError);
                }
            }
            else
            {
                exceptionMessage = actionExecutedContext.Exception.InnerException.InnerException.Message;
            }
            //We can log this exception message to the file or database.  
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(exceptionMessage),
                ReasonPhrase = "DB Error.Please Contact your Administrator."
            };
            actionExecutedContext.Response = response;         
        }

    }

    public class TextPlainErrorResult : IHttpActionResult
    {
        public HttpRequestMessage Request { private get; set; }
        public string Content { private get; set; }
        public HttpStatusCode Statuscode { private get; set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response =
                new HttpResponseMessage(Statuscode)
                {
                    Content = new StringContent(Content),
                    RequestMessage = Request
                };
            return Task.FromResult(response);
        }
    }
    public class POCErrors
    {
        public string ErrorCode { get; set; }
        public string ErrorProperty { get; set; }
        public string ErrorDescription { get; set; }
    }
}