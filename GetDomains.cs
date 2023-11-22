using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace b2c_local
{
    public class GetDomains
    {
        private readonly ILogger _logger;

        public GetDomains(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetDomains>();
        }

        [Function("GetDomains")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            List<string> domains = new List<string>() { "vmghealth.com", "concurrency.com", "gmail.com", "mailinator.com" };

            // return list of domains
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(JsonConvert.SerializeObject(domains));
            return response;
        }
    }
}
