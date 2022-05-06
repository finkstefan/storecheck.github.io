using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microservice_Feedback.Models;

namespace Microservice_Feedback.Services
{
    public interface ILoggerService
    {
        Task<HttpStatusCode> CreateLog(LogDto logDTO);
    }
}
