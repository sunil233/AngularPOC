using System.Linq;
using System.Web.Http;


namespace POCServices
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
          
            GlobalConfiguration.Configure(WebApiConfig.Register);
          
        }

        /// <summary>
        /// to handle OPTIONS requests you must reply with empty response,
        /// </summary>
        protected void Application_BeginRequest()
        {
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {
                Response.Flush();
            }
        }

        protected void Application_PostAuthorizeRequest()
        {
            string url = System.Web.HttpContext.Current.Request.Path;
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }
    }
}