using AutoMapper;
using Microservice_Feedback.Data;
using Microservice_Feedback.Entities;
using Microservice_Feedback.Models;
using Microservice_Feedback.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Controllers
{
    [ApiController]
    [Route("api/feedbackCategories")]
    [Produces("application/json", "application/xml")]
    public class FeedbackCategoryController : ControllerBase
    {
        private readonly IFeedbackCategoryRepository feedbackCategoryRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkgenerator;
        private readonly ILoggerService loggerService;
        private readonly LogDto logDto;

        public FeedbackCategoryController(IFeedbackCategoryRepository feedbackCategoryRepository, IMapper mapper, LinkGenerator linkgenerator, ILoggerService loggerService)
        {
            this.feedbackCategoryRepository = feedbackCategoryRepository;
            this.mapper = mapper;
            this.linkgenerator = linkgenerator;
            this.loggerService = loggerService;
            logDto = new LogDto();
            logDto.NameOfTheService = "Feedback";
        }

        /// <summary>
        /// Returns all feedback categories from database.
        /// </summary>
        /// <returns>List of feedback categories</returns>
        /// <response code="200">Returns list of feedback categories</response>
        /// <response code="204">Nothing to return</response>
        /// <response code="505">Error in getting one feedback caterogory</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<FeedbackDTO>> GetFeedbackCategories()
        {
            try
            {
                logDto.HttpMethod = "GET";
                logDto.Message = "Return all feedback categories";
                var feedbackCategory = feedbackCategoryRepository.GetFeedbackCategories();

                if (feedbackCategory == null || feedbackCategory.Count == 0)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NoContent();
                }

                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return Ok(mapper.Map<List<FeedbackCategoryDTO>>(feedbackCategory));
            }
            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Get all feedbackCategory Error");

            }
        }

        /// <summary>
        /// Adds feedback categories
        /// </summary>
        /// <returns>Adding new feedback categories</returns>
        /// <response code="200">Creates new feedback categories</response>
        /// <response code="500">Error in creationg new feedback categories</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<FeedbackCategoryDTO> CreateFeedbackCategory([FromBody] FeedbackCategoryDTO feedbackCategoryDTO)
        {
            try
            {
                logDto.HttpMethod = "POST";
                logDto.Message = "Create feedback category";

                FeedbackCategory feedbackCategory = mapper.Map<FeedbackCategory>(feedbackCategoryDTO);
                FeedbackCategory helper = feedbackCategoryRepository.CreateFeedbackCategory(feedbackCategory);
                feedbackCategoryDTO = mapper.Map<FeedbackCategoryDTO>(helper);
                feedbackCategoryRepository.SaveChanges();
                string location = linkgenerator.GetPathByAction("GetFeedbackCategories", "FeedbackCategory", new { feedbackCategoryId = helper.FeedbackCategoryId });

                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return Created(location, feedbackCategoryDTO);
            }
            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create feedbackCategory Error");
            }
        }


        /// <summary>
        /// Returns one feedback category from database.
        /// </summary>
        /// <returns>Feedback category</returns>
        /// <response code="200">Returns one feedback category</response>
        /// <response code="404">Neither feedback category has been found</response>
        /// <response code="505">Error in getting one feedback cateogory</response>
        [HttpGet("{feedbackCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<FeedbackCategoryDTO> GetFeedbackCategory(Guid feedbackCategoryId)
        {
            try
            {
                logDto.HttpMethod = "GET";
                logDto.Message = "Return one feedback category by id";
                FeedbackCategory feedbackCategory = feedbackCategoryRepository.GetFeedbackCategoryById(feedbackCategoryId);

                if (feedbackCategory == null)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NotFound();
                }

                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return Ok(mapper.Map<FeedbackCategoryDTO>(feedbackCategory));
            }
            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Get one feedbackCategory by id Error");
            }
        }



        /// <summary>
        /// Updates feedback category
        /// </summary>
        /// <returns>Updating a feedback category</returns>
        /// <response code="200">Updates feedback category</response>
        /// <response code="404">Neither feedback category has been found</response>
        /// <response code="500">Error in updating a feedback category</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<FeedbackCategoryDTO> UpdateFeedbackCategory([FromBody] FeedbackCategory feedbackCategory)
        {
            try
            {
                logDto.HttpMethod = "PUT";
                logDto.Message = "Updates feedback category";
                FeedbackCategory oldFeedbackCategory = feedbackCategoryRepository.GetFeedbackCategoryById(feedbackCategory.FeedbackCategoryId);
                if (oldFeedbackCategory == null)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NotFound();
                }

                oldFeedbackCategory.FeedbackCategoryName = feedbackCategory.FeedbackCategoryName;

                feedbackCategoryRepository.SaveChanges();


                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return Ok(mapper.Map<FeedbackCategoryDTO>(oldFeedbackCategory));
            }
            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update feedbackCategory Error");
            }
        }

        /// <summary>
        /// Deletes feedback category
        /// </summary>
        /// <returns>Adding new feedback category</returns>
        /// <response code="200">Deletes feedback category</response>
        /// <response code="404">Neither feedback category has been found</response>
        /// <response code="500">Error in deleting feedback category</response>
        [HttpDelete("{feedbackCategoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteFeedbackCategory(Guid feedbackCategoryId)
        {
            try
            {
                logDto.HttpMethod = "DELETE";
                logDto.Message = "Deletes feedback category";
                FeedbackCategory feedbackCategory = feedbackCategoryRepository.GetFeedbackCategoryById(feedbackCategoryId);
                if (feedbackCategory == null)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NotFound();
                }
                feedbackCategoryRepository.DeleteFeedbackCategory(feedbackCategoryId);
                feedbackCategoryRepository.SaveChanges();

                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return NoContent();
            }
            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete feedbackCategory Error");
            }
        }

    }

}

