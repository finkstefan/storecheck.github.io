using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Models
{
    /// <summary>
    /// DTO for log
    /// </summary>
    public class LogDto
    {
        /// <summary>
        /// Http method
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Name of the service
        /// </summary>
        public string NameOfTheService { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
    }
}
