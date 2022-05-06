using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservice_Feedback.Services
{
    public class ObjectStoreCheckService : IObjectStoreCheckService
    {
        private readonly IConfiguration configuration;

        public ObjectStoreCheckService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<Guid> GetObjectStoreCheckIdByUsernameAsync(string username)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri($"{configuration["Services:ObjectStoreCheckService"]}/api/objectStoreChecks/nopdf/{username}");

                HttpResponseMessage response = client.GetAsync(url).Result;

                var responseContent = await response.Content.ReadAsStringAsync();
                var id = JsonConvert.DeserializeObject<Guid>(responseContent);

                return id;
            }
        }

    }
}
