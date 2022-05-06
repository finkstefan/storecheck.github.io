using Microservice_Feedback.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Data
{
    public interface IFeedbackCategoryRepository
    {
         
            List<FeedbackCategory> GetFeedbackCategories();

            FeedbackCategory GetFeedbackCategoryById(Guid feedbackCategoryId);

            FeedbackCategory CreateFeedbackCategory(FeedbackCategory feedbackCategory);

            void UpdateFeedbackCategory(FeedbackCategory feedbackCategory);

            void DeleteFeedbackCategory(Guid feedbackCategoryId);

            bool SaveChanges();

            FeedbackCategory GetFeedbackCategoryByName(string name);


    }
}
