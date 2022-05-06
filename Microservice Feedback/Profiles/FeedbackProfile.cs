using AutoMapper;
using Microservice_Feedback.Entities;
using Microservice_Feedback.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Profiles
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackDTO>();
            CreateMap<FeedbackDTO, Feedback>();
            CreateMap<FeedbackFrontDTO,FeedbackDTO>();
            CreateMap<FeedbackDTO, FeedbackFrontDTO>();
            CreateMap<Feedback, FeedbackFrontDTO>();
            CreateMap<FeedbackFrontDTO, Feedback>();
        }
    }
}
