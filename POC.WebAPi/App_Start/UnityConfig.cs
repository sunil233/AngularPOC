using Microsoft.Practices.Unity.Configuration;
using System.Web.Http;
using Unity;

namespace POCServices
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            if (System.Configuration.ConfigurationManager.GetSection("unity") != null)
            {
                // register all your components with the container here.It is NOT necessary to
                // register your controllers e.g. container.RegisterType<ITestService, TestService>();

                container.LoadConfiguration();
                //Set the dependency resolver on the DependencyResolver property of the global HttpConfiguration object.
                //registering Unity for web API
                GlobalConfiguration.Configuration.DependencyResolver = new UnityResolver(container);

                var LoggingHeaderHandler = container.Resolve<LoggingHeaderHandler>();
                GlobalConfiguration.Configuration.MessageHandlers.Add(LoggingHeaderHandler);

                //registering Unity for MVC
                //  DependencyResolver.SetResolver(new UnityResolver(container));
            }
        }
    }
}