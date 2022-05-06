using Microservice_Feedback.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservice_Feedback.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly IConfiguration configuration;

        public LoggerService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<HttpStatusCode> CreateLog(LogDto logDto)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri($"{configuration["Services:LoggerService"]}/api/logger");

                HttpContent content = new StringContent(JsonConvert.SerializeObject(logDto));
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;
                var responseContent = response.StatusCode;

                return await Task.FromResult(responseContent);
            }
        }
    }
}
