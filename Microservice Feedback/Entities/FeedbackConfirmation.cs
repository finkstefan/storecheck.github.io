using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Entities
{
    /// <summary>
    /// Confirmation of feedback
    /// </summary>
    public class FeedbackConfirmation
    {
        /// <summary>
        /// Feedback category ID
        /// </summary>
        public Guid FeedbackCategoryID;

        /// <summary>
        /// Object storecheck ID
        /// </summary>
        public Guid ObjectStoreCheckID;

        /// <summary>
        /// Feedback text
        /// </summary>
        public string Text;

        /// <summary>
        /// Feedback creation date
        /// </summary>
        public DateTime Date;

        /// <summary>
        /// Is feedback resolved
        /// </summary>
        public bool Resolved;

   

    }
}