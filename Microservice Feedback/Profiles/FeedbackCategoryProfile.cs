using AutoMapper;
using Microservice_Feedback.Entities;
using Microservice_Feedback.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Profiles
{
    public class FeedbackCategoryProfile : Profile
    {
        public FeedbackCategoryProfile()
        {
            CreateMap<FeedbackCategory, FeedbackCategoryDTO>();
            CreateMap<FeedbackCategoryDTO, FeedbackCategory>();
        }
    }
}
