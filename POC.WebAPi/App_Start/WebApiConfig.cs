using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Newtonsoft.Json.Serialization;
using POC.WepApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Routing;
using Unity;

namespace POCServices
{
    public static class WebApiConfig
    {
      
        public static void Register(HttpConfiguration config)
        {
          
            var container = new UnityContainer();
            container.LoadConfiguration();
            //Configuring the Dependency Resolver
            //Set the dependency resolver on the DependencyResolver property of the global HttpConfiguration object.
            config.DependencyResolver = new UnityResolver(container);
           // config.EnableCors();
            config.Routes.MapHttpRoute(
                "DefaultApiWithId",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional },
                new { id = @"\d+" }
            );

            config.Routes.MapHttpRoute(
                "DefaultWithAction",
                "api/{controller}/{action}"
            );

            config.Routes.MapHttpRoute(
                "DefaultApiGet",
                "api/{controller}",
                new { action = "Get" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );

            config.Routes.MapHttpRoute(
                "DefaultApiPost",
                "api/{controller}",
                new { action = "Post" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );

            config.Routes.MapHttpRoute(
                "DefaultApiPut",
                "api/{controller}",
                new { action = "Put" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) }
            );

            config.Routes.MapHttpRoute(
                "DefaultApiDelete",
                "api/{controller}",
                new { action = "Delete" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) }
            );
            config.Filters.Add(new CustomExceptionFilter());
            //Sets default to JsonFormatter
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }
    }
}