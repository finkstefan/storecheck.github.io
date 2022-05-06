using Microservice_Feedback.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Data
{
    public interface IFeedbackRepository
    {
        List<Feedback> GetFeedbacks();

        Feedback GetFeedbackById(Guid feedbackId);

        Feedback CreateFeedback(Feedback feedback);

        void UpdateFeedback(Feedback feedback);

        void DeleteFeedback(Guid feedbackId);

        bool SaveChanges();

        List<Feedback> GetFeedbacksByCategoryName(string categoryName);

        List<Feedback> GetFeedbacksByObjectStoreCheckId(Guid objectStoreCheckId);
        List<Feedback> GetUnresolvedFeedbacks();

        Feedback GetFeedbackByImg(string img);


    }
}
