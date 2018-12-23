using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace POCServices.Filters
{
    public class AcysAuthenticationAttribute : AuthorizationFilterAttribute
    {

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (!IsUserAuthorized(filterContext))
            {
                ShowAuthenticationError(filterContext);
                return;
            }
            base.OnAuthorization(filterContext);
        }
        public bool IsUserAuthorized(HttpActionContext actionContext)
        {
            var authHeader = FetchFromHeader(actionContext); //fetch authorization token from header
            if (authHeader != null)
            {
                var auth = new AuthenticationModule();
                JwtSecurityToken userPayloadToken = auth.GenerateUserClaimFromJWT(authHeader);
                if (userPayloadToken != null)
                {

                    var identity = auth.PopulateUserIdentity(userPayloadToken);
                    string[] roles = { "All" };
                    var genericPrincipal = new GenericPrincipal(identity, roles);
                    Thread.CurrentPrincipal = genericPrincipal;
                    var authenticationIdentity = Thread.CurrentPrincipal.Identity as JWTAuthenticationIdentity;
                    if (authenticationIdentity != null && !String.IsNullOrEmpty(authenticationIdentity.UserName))
                    {
                        authenticationIdentity.UserId = identity.UserId;
                        authenticationIdentity.UserName = identity.UserName;
                    }
                    return true;
                }
            
            }
            return false;

        }
        /// <summary>
        /// method to fetch token from the header
        /// </summary>
        /// <param name="actionContext">HttpActionContext</param>
        /// <returns>JWT token</returns>
        private string FetchFromHeader(HttpActionContext actionContext)
        {
            string requestToken = null;
            var authRequest = actionContext.Request.Headers.Authorization;
            if (authRequest != null && authRequest.Scheme=="Acys")
            {
                requestToken = authRequest.Parameter;
            }
            else
            {
                if(!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Headers["Authheader"]))
                {
                    requestToken = System.Web.HttpContext.Current.Request.Headers["Authheader"];
                }
            }
            return requestToken;
        }
        private static void ShowAuthenticationError(HttpActionContext filterContext)
        {
            var responseDTO = new ResponseDTO() { Code = 401, Message = "Unable to access, Please login again" };
            filterContext.Response =filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, responseDTO);
        }

    }
}