using AutoMapper;
using Microservice_Feedback.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Data
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly StoreCheckFeedbackContext context;
        private readonly IMapper mapper;
        

        public FeedbackRepository(StoreCheckFeedbackContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public Feedback CreateFeedback(Feedback feedback)
        {
            feedback.FeedbackId = Guid.NewGuid();
            context.Feedbacks.Add(feedback);
            context.SaveChanges();
            return feedback;
        }

        public void DeleteFeedback(Guid feedbackId)
        {
            context.Feedbacks.Remove(context.Feedbacks.FirstOrDefault(p => p.FeedbackId == feedbackId));
        }

        public Feedback GetFeedbackById(Guid feedbackId)
        {
            return context.Feedbacks.FirstOrDefault(p => p.FeedbackId == feedbackId);
        }

        public List<Feedback> GetFeedbacks()
        {
            return context.Feedbacks.ToList();
        }
        public Feedback GetFeedbackByImg(string img)
        {
            return context.Feedbacks.FirstOrDefault(p => p.Img == img);
        }


        public List<Feedback> GetUnresolvedFeedbacks()
        {
            return (from f in context.Feedbacks where f.Resolved==false select f).ToList();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateFeedback(Feedback feedback)
        {

        }

        public List<Feedback> GetFeedbacksByCategoryName(string categoryName)
          {
              var category=context.FeedbackCategories.FirstOrDefault(f => f.FeedbackCategoryName== categoryName);
              return (from f in context.Feedbacks where f.FeedbackCategoryId == category.FeedbackCategoryId select f).ToList();
          }

        public List<Feedback> GetFeedbacksByObjectStoreCheckId(Guid objectStoreCheckId)
        {
            return (from f in context.Feedbacks where f.ObjectStoreCheckId == objectStoreCheckId select f).ToList();
        }

    }
}
