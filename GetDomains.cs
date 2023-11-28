using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace b2c_local
{
    public class DomainResponse
    {

        public string? version { get; set; }
        public int status { get; set; }
        public List<string> domains { get; set; }
    }
    public class GetDomains
    {
        private readonly ILogger _logger;

        public GetDomains(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetDomains>();
        }

        [Function("GetDomains")]
        public DomainResponse Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            List<string> domains = new List<string>() { "vmghealth.com", "concurrency.com", "gmail.com", "mailinator.com" };

            DomainResponse domainResponse = new DomainResponse
            {
                version = "1.0.0",
                status = 200,
                domains = domains
            };

            return domainResponse;
        }
    }
}
