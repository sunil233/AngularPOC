using POC.Repository.Interface;
using POCRepository.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace POCServices
{
    /// <summary>
    /// You can add message handlers to the pipeline to perform one or more of the following operations.
    /// a) Perform authentication and authorization b)Logging the incoming requests and the outgoing
    /// responses c)Add response headers to the response objects d)Read or modify the request headers
    /// </summary>
    public class LoggingHeaderHandler : DelegatingHandler
    {
        private readonly ILogger Logger;
        private const string WWWAuthenticateHeaderToken = "X-TraceId";

        public LoggingHeaderHandler(ILogger logger)
        {
            this.Logger = logger;
        }

        /// <summary>
        /// In the below method we are logging the incoming requests token Id
        /// </summary>
        /// <param name="request">          HttpRequestMessage</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IEnumerable<string> apiKeyHeaderValues = null;
            if (request.Headers.TryGetValues(WWWAuthenticateHeaderToken, out apiKeyHeaderValues))
            {
                var apiKeyHeaderValue = apiKeyHeaderValues.First();
                // ... your authentication logic here ...
                var msg = string.Format("Service Requested by UUID [{0}] and Requested URi [{1}]", apiKeyHeaderValue, request.RequestUri);
                Logger.Information(msg);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}