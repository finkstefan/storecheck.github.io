using AutoMapper;
using Microservice_Feedback.Data;
using Microservice_Feedback.Entities;
using Microservice_Feedback.Models;
using Microservice_Feedback.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Feedback.Controllers
{
    [ApiController]
    [Route("api/feedbacks")]
    [Produces("application/json", "application/xml")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository feedbackRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkgenerator;
        private readonly IFeedbackCategoryRepository feedbackCategoryRepository;
        private readonly ILoggerService loggerService;
        private readonly IObjectStoreCheckService objectStoreCheckService;
        private readonly LogDto logDto;
        private readonly IHostingEnvironment hostingEnvironment;

        public FeedbackController(IFeedbackRepository feedbackRepository, IMapper mapper, LinkGenerator linkgenerator, IFeedbackCategoryRepository feedbackCategoryRepository, ILoggerService loggerService, IObjectStoreCheckService objectStoreCheckService, IHostingEnvironment hostingEnvironment)
        {
            this.feedbackRepository = feedbackRepository;
            this.mapper = mapper;
            this.linkgenerator = linkgenerator;
            this.feedbackCategoryRepository = feedbackCategoryRepository;
            this.loggerService = loggerService;
            this.objectStoreCheckService = objectStoreCheckService;
            logDto = new LogDto();
            logDto.NameOfTheService = "Feedback";
            this.hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Returns all feedbacks from database.
        /// </summary>
        /// <returns>List of feedbacks</returns>
        /// <response code="200">Returns list of feedbacks</response>
        /// <response code="204">Nothing to return</response>
        /// <response code="505">Error in getting all feedbacks</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<FeedbackFrontDTO>> GetFeedbacks()
        {
            try
            {
                logDto.HttpMethod = "GET";
                logDto.Message = "Return all feedbacks";
                var feedbacks = feedbackRepository.GetFeedbacks();

                if (feedbacks == null || feedbacks.Count == 0)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NoContent();
                }
                List<FeedbackFrontDTO> feedbacksDTO = new List<FeedbackFrontDTO>();
                foreach (Feedback feed in feedbacks)
                {
                    FeedbackFrontDTO feedbackDTO = mapper.Map<FeedbackFrontDTO>(feed);
                    feedbackDTO.FeedbackCategoryName = feedbackCategoryRepository.GetFeedbackCategoryById(feed.FeedbackCategoryId).FeedbackCategoryName;

                    feedbacksDTO.Add(feedbackDTO);
                }



                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return Ok(feedbacksDTO);
            }

            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Get all feedback Error");
            }
        }

        /// <summary>
        /// Returns one feedbacks from database.
        /// </summary>
        /// <returns>Feedback</returns>
        /// <response code="200">Returns one feedback</response>
        /// <response code="404">Neither feedback has been found</response>
        /// <response code="505">Error in getting a feedback</response>
        [HttpGet("{feedbackId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<FeedbackDTO> GetFeedback(Guid feedbackId)
        {
            try
            {
                logDto.HttpMethod = "GET";
                logDto.Message = "Return one feedback by id";
                Feedback feedback = feedbackRepository.GetFeedbackById(feedbackId);

                if (feedback == null)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NotFound();
                }
                FeedbackCategory feedbackCategory = feedbackCategoryRepository.GetFeedbackCategoryById(feedback.FeedbackCategoryId);

                FeedbackDTO feedbackDTO = mapper.Map<FeedbackDTO>(feedback);

                feedbackDTO.FeedbackCategoryName = feedbackCategory.FeedbackCategoryName;

                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return Ok(feedbackDTO);
            }
            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Get feedback by id Error");
            }
        }

        /// <summary>
        /// Returns one feedbacks from database.
        /// </summary>
        /// <returns>Feedback</returns>
        /// <response code="200">Returns one feedback</response>
        /// <response code="404">Neither feedback has been found</response>
        /// <response code="505">Error in getting a feedback</response>
        [HttpGet("getOneFeedback/{Img}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<FeedbackDTO> GetOneFeedbackByImg(string img)
        {
            try
            {
                logDto.HttpMethod = "GET";
                logDto.Message = "Return one feedback by id";
                Feedback feedback = feedbackRepository.GetFeedbackByImg(img);

                if (feedback == null)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NotFound();
                }
                FeedbackCategory feedbackCategory = feedbackCategoryRepository.GetFeedbackCategoryById(feedback.FeedbackCategoryId);

                FeedbackFrontDTO feedbackFrontDTO = mapper.Map<FeedbackFrontDTO>(feedback);

                feedbackFrontDTO.FeedbackCategoryName = feedbackCategory.FeedbackCategoryName;

                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return Ok(feedbackFrontDTO);
            }
            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Get feedback by id Error");
            }
        }


        /// <summary>
        /// Adds feedback
        /// </summary>
        /// <returns>Adding new feedback</returns>
        /// <response code="200">Creates new feedback</response>
        /// <response code="505">Error in creating new feedback</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<FeedbackFrontDTO> CreateFeedback(IFormFile file, [FromForm] FeedbackDTO feedbackDTO)
        {
            try
            {
                logDto.HttpMethod = "POST";
                logDto.Message = "Create new feedback";

                string images = Path.Combine(hostingEnvironment.ContentRootPath, "Images");
                Directory.CreateDirectory(images);
                if (file.Length > 0)
                {
                    string fileName = feedbackDTO.Username + DateTime.Now.ToString("yyyyMMddTHHmmss") + file.FileName;
                    string filePath = Path.Combine(images, fileName);
                    feedbackDTO.Img = "Images/" + fileName;
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fileStream);
                    }
                }

                feedbackDTO.ObjectStoreCheckId = objectStoreCheckService.GetObjectStoreCheckIdByUsernameAsync(feedbackDTO.Username).Result;
                Feedback feedback = mapper.Map<Feedback>(feedbackDTO);

                feedback.FeedbackCategoryId = feedbackCategoryRepository.GetFeedbackCategoryByName(feedbackDTO.FeedbackCategoryName).FeedbackCategoryId;

                Feedback helper = feedbackRepository.CreateFeedback(feedback);
                feedbackRepository.SaveChanges();


                FeedbackFrontDTO feedbackFrontDTO = mapper.Map<FeedbackFrontDTO>(helper);

                feedbackFrontDTO.FeedbackCategoryName = feedbackDTO.FeedbackCategoryName;

                string location = linkgenerator.GetPathByAction("GetFeedbacks", "Feedback", new { feedbackId = helper.FeedbackId });




                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return Created(location, feedbackFrontDTO);
            }
            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create feedback Error");
            }
        }

        /// <summary>
        /// Updates feedback
        /// </summary>
        /// <returns>Updating a feedback</returns>
        /// <response code="200">Updates feedback</response>
        /// <response code="404">Neither feedback has been found</response>
        /// <response code="500">Error in updating new feedback</response>
        [HttpPut("{Img}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<FeedbackFrontDTO> UpdateFeedback(string Img)
        {
            try
            {
                logDto.HttpMethod = "PUT";
                logDto.Message = "Update feedback";
                Feedback oldFeedback = feedbackRepository.GetFeedbackByImg(Img);
                if (oldFeedback == null)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NotFound();
                }

                oldFeedback.Resolved = true;


                feedbackRepository.SaveChanges();
                FeedbackFrontDTO feedbackFront = mapper.Map<FeedbackFrontDTO>(oldFeedback);

                feedbackFront.FeedbackCategoryName = feedbackCategoryRepository.GetFeedbackCategoryById(oldFeedback.FeedbackCategoryId).FeedbackCategoryName;

                logDto.Level = "Info";
                loggerService.CreateLog(logDto);

                return Ok(feedbackFront);
            }
            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update feedback Error");
            }
        }

        /// <summary>
        /// Deletes feedback
        /// </summary>
        /// <returns>Adding new feedback</returns>
        /// <response code="200">Deletes feedback</response>
        /// <response code="404">Neither feedback has been found</response>
        /// <response code="500">Error in deleting feedback</response>
        [HttpDelete("{feedbackId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteFeedback(Guid feedbackId)
        {
            try
            {
                logDto.HttpMethod = "DELETE";
                logDto.Message = "Delete feedback";
                Feedback feedback = feedbackRepository.GetFeedbackById(feedbackId);
                if (feedback == null)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NotFound();
                }
                feedbackRepository.DeleteFeedback(feedbackId);
                feedbackRepository.SaveChanges();
                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return NoContent();
            }
            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete feedback Error");
            }
        }

        /// <summary>
        /// Returns all feedbacks by its categories id from database.
        /// </summary>
        /// <returns>List of feedbacks</returns>
        /// <response code="200">Returns list of feedbacks</response>
        /// <response code="204">Nothing to return</response>
        /// <response code="505">Error in getting feedbacks</response>
        [HttpGet("feedbackByCategoryName/{feedbackCategoryName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<FeedbackDTO>> GetFeedbacksByCategory(string feedbackCategoryName)
        {
            try
            {
                logDto.HttpMethod = "GET";
                logDto.Message = "Return all feedbacks by category";
                var feedbacks = feedbackRepository.GetFeedbacksByCategoryName(feedbackCategoryName);

                if (feedbacks == null || feedbacks.Count == 0)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NoContent();
                }
                List<FeedbackDTO> feedbacksDTO = new List<FeedbackDTO>();
                foreach (Feedback feed in feedbacks)
                {
                    FeedbackDTO feedbackDTO = mapper.Map<FeedbackDTO>(feed);
                    feedbackDTO.FeedbackCategoryName = feedbackCategoryRepository.GetFeedbackCategoryById(feed.FeedbackCategoryId).FeedbackCategoryName;

                    feedbacksDTO.Add(feedbackDTO);
                }

                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return Ok(mapper.Map<List<FeedbackDTO>>(feedbacksDTO));
            }

            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Get feedback by category name Error");
            }
        }


        /// <summary>
        /// Returns all feedbacks by its categories id from database.
        /// </summary>
        /// <returns>List of feedbacks</returns>
        /// <response code="200">Returns list of feedbacks</response>
        /// <response code="204">Nothing to return</response>
        /// <response code="505">Error in getting feedbacks</response>
        [HttpGet("feedbackByObjectStoreCheckId/{objectStoreCheckId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<FeedbackDTO>> GetFeedbacksByObjectStoreCheckId(Guid objectStoreCheckId)
        {
            try
            {
                logDto.HttpMethod = "GET";
                logDto.Message = "Return all feedbacks by objectstoreCheckId";
                var feedbacks = feedbackRepository.GetFeedbacksByObjectStoreCheckId(objectStoreCheckId);

                if (feedbacks == null || feedbacks.Count == 0)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NoContent();
                }
                List<FeedbackDTO> feedbacksDTO = new List<FeedbackDTO>();
                foreach (Feedback feed in feedbacks)
                {
                    FeedbackDTO feedbackDTO = mapper.Map<FeedbackDTO>(feed);
                    feedbackDTO.FeedbackCategoryName = feedbackCategoryRepository.GetFeedbackCategoryById(feed.FeedbackCategoryId).FeedbackCategoryName;

                    feedbacksDTO.Add(feedbackDTO);
                }

                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return Ok(mapper.Map<List<FeedbackDTO>>(feedbacksDTO));
            }

            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Get feedback by objectstorecheck Error");
            }
        }

        /// <summary>
        /// Returns all feedbacks from database.
        /// </summary>
        /// <returns>List of feedbacks</returns>
        /// <response code="200">Returns list of feedbacks</response>
        /// <response code="204">Nothing to return</response>
        /// <response code="505">Error in getting all feedbacks</response>
        [HttpGet("unresolvedFeedbacks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<FeedbackFrontDTO>> GetUnresolvedFeedbacks()
        {
            try
            {
                logDto.HttpMethod = "GET";
                logDto.Message = "Return all feedbacks";
                var feedbacks = feedbackRepository.GetUnresolvedFeedbacks();

                if (feedbacks == null || feedbacks.Count == 0)
                {
                    logDto.Level = "Warn";
                    loggerService.CreateLog(logDto);
                    return NoContent();
                }
                List<FeedbackDTO> feedbacksDTO = new List<FeedbackDTO>();
                foreach (Feedback feed in feedbacks)
                {
                    FeedbackDTO feedbackDTO = mapper.Map<FeedbackDTO>(feed);
                    feedbackDTO.FeedbackCategoryName = feedbackCategoryRepository.GetFeedbackCategoryById(feed.FeedbackCategoryId).FeedbackCategoryName;

                    feedbacksDTO.Add(feedbackDTO);
                }



                logDto.Level = "Info";
                loggerService.CreateLog(logDto);
                return Ok(mapper.Map<List<FeedbackFrontDTO>>(feedbacksDTO));
            }

            catch
            {
                logDto.Level = "Error";
                loggerService.CreateLog(logDto);
                return StatusCode(StatusCodes.Status500InternalServerError, "Get unresolver feedbacksF Error");
            }
        }




    }
}
