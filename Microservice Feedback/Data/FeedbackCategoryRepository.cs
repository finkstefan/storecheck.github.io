using AutoMapper;
using Microservice_Feedback.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Data
{
    public class FeedbackCategoryRepository : IFeedbackCategoryRepository
    {

        private readonly StoreCheckFeedbackContext context;
        private readonly IMapper mapper;

        public FeedbackCategoryRepository(StoreCheckFeedbackContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

      
        public FeedbackCategory CreateFeedbackCategory(FeedbackCategory feedbackCategory)
        {
            feedbackCategory.FeedbackCategoryId = Guid.NewGuid();
            context.FeedbackCategories.Add(feedbackCategory);
            return feedbackCategory;
        }

        public void DeleteFeedbackCategory(Guid feedbackCategoryId)
        {
            context.FeedbackCategories.Remove(context.FeedbackCategories.FirstOrDefault(p => p.FeedbackCategoryId == feedbackCategoryId));
        }


        public List<FeedbackCategory> GetFeedbackCategories()
        {
            return context.FeedbackCategories.ToList();
        }

        public FeedbackCategory GetFeedbackCategoryById(Guid feedbackCategoryId)
        {
            return context.FeedbackCategories.FirstOrDefault(p => p.FeedbackCategoryId == feedbackCategoryId);
        }

        public FeedbackCategory GetFeedbackCategoryByName(string name)
        {
            return context.FeedbackCategories.FirstOrDefault(p => p.FeedbackCategoryName == name);
        }
        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }


        public void UpdateFeedbackCategory(FeedbackCategory feedbackCategory)
        {
           
        }
    }
}

