using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Services
{
    public interface IObjectStoreCheckService
    {
        Task<Guid> GetObjectStoreCheckIdByUsernameAsync(string username);
    }
}
