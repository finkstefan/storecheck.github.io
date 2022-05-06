using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Models
{
    public class FeedbackFrontDTO
    {
        /// <summary>
        /// Feedback category ID
        /// </summary>
        public string FeedbackCategoryName { get; set; }

        

        /// <summary>
        /// Feedback text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Feedback creation date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Is feedback resolved
        /// </summary>
        public bool Resolved { get; set; }

        /// <summary>
        /// Feedback image route
        /// </summary>
        public string Img { get; set; }

        /// <summary>
        /// Feedback users username
        /// </summary>
        public string Username { get; set; }
    }
}
