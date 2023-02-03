using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CoreWebApiDemo.DataAccess;
using CoreWebApiDemo.Model;
using dotenv.net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreWebApiDemo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionaryController : ControllerBase
    {
        private readonly CourtDBContext _context;

        public QuestionaryController(CourtDBContext context)
        {
            _context = context;
        }

        private async Task<string> AttecheMedia(IFormFile file)
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            cloudinary.Api.Secure = true;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = true
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.ToString();

        }

        [HttpPost("AskQuestion")]
        public async Task<IActionResult> AskQuestion(string question, string description, IFormFile file, int userid, int lawyerid)
        {
            QuestionaryModel add_question = new QuestionaryModel();
            add_question.Question = question;
            add_question.Description = description;
            add_question.Media = await AttecheMedia(file);
            add_question.isPicked = false;
            
            var lawyer = _context.userModels.Where(user => user.Role.ToLower() == "Lawyer".ToLower() && user.Id == lawyerid).FirstOrDefault();
            if (lawyer == null)
            {
                return Problem("No lawyer found of this id. Try again.");
            }
            if (lawyer.Id == lawyerid)
            {
                add_question.LawyerId = lawyerid;
            }

            var user = _context.userModels.Where(user=> user.Role.ToLower() == "User".ToLower() && user.Id == userid).FirstOrDefault();
            if (user == null)
            {
                return Problem("No user found of this id. Try again.");
            }
            if (user.Id == userid)
            {
                add_question.UserId = userid;
            }
            else
            {
                return Problem("Invalid User Id. Try Again.");
            }
            
            _context.questionaryModels.Add(add_question);
            await _context.SaveChangesAsync();
            return Ok(add_question);
        }

        [HttpGet("QuestionsNotPicked")]
        public async Task<IActionResult> QuestionsNotPicked()
        {
            var questions = from question in _context.questionaryModels
                            where question.LawyerId == null || 
                                    question.isPicked == false
                            select new
                            {
                                question.Id,
                                question.Question,
                                question.Description,
                                question.Media,
                                question.LawyerId,
                                question.UserId,
                                question.isPicked
                            };
            return Ok(questions);
        }

        /// <summary>
        /// Returns questions those are picked by lawyer.
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="lawyerid"></param>
        /// <returns></returns>
        [HttpPut("PickQuestion")]
        public async Task<ActionResult<QuestionaryModel>> PickQuestion(int questionId, int lawyerid)
        {
            var question = await _context.questionaryModels.FindAsync(questionId);
            if (question == null)
            {
                return NotFound("Not found");
            }
            if (question.Id == questionId)
            {
                question.isPicked = true;
                var lawyer = _context.userModels.Where(user => user.Role.ToLower() == "Lawyer".ToLower() && user.Id == lawyerid).FirstOrDefault();
                if (lawyer == null)
                {
                    return NotFound();
                }
                if(question.LawyerId == null)
                {
                    question.LawyerId = lawyerid;
                }
                else
                {
                    return Problem("Question is already picked by someone.");
                }
                _context.Entry(question).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return Ok("Question is picked please kindly answer to the question.");
            //return CreatedAtAction("AddAnswer", new { id = questionId }, question);
            //return RedirectToAction("AddAnswer",new { questionId });
        }

        /// <summary>
        /// Add answer to picked question.
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="answer"></param>
        /// <param name="lawyerid"></param>
        /// <returns></returns>
        [HttpPost("AddAnswer")]
        public async Task<IActionResult> AddAnswer(int questionId, string answer, int lawyerid)
        {
            AnswerModel AddAnswer = new AnswerModel();
            AddAnswer.Answer = answer;

            var question = await _context.questionaryModels.FindAsync(questionId);
            if (question == null)
            {
                return NotFound("Not found");
            }
            if (question.Id == questionId)
            {
                question.isPicked = true;
                AddAnswer.QuestionId = questionId;
                _context.Entry(question).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            var lawyer = _context.userModels.Where(user => user.Role.ToLower() == "Lawyer".ToLower() && user.Id == lawyerid).FirstOrDefault();
            if (lawyer == null)
            {
                question.LawyerId = lawyerid;
            }
            if (lawyer.Id == lawyerid)
            {
                AddAnswer.LawyerId = lawyerid;
            }

            _context.answerModels.Add(AddAnswer);
            await _context.SaveChangesAsync();

            ConversationModel conversation = new ConversationModel();
            conversation.QuestionId = questionId;
            conversation.AnswerId = AddAnswer.Id;
            _context.conversationModels.Add(conversation);
            await _context.SaveChangesAsync();

            return Ok("Question is picked and answered." + AddAnswer);
        }

        [HttpPut("ReassignQuestion")]
        public async Task<IActionResult> ReassignQuestion(int questionid, int lawyerid)
        {
            var question = await _context.questionaryModels.FindAsync(questionid);
            var lawyer = _context.userModels.Where(user => user.Role.ToLower() == "Lawyer".ToLower() && user.Id == lawyerid).ToListAsync();
            
            if (lawyer == null)
            {
                question.LawyerId = lawyerid;
            }
            if (lawyer.Id != null)
            {
                question.LawyerId = lawyerid;
            }

            _context.Entry(question).State = EntityState.Modified;
            return Ok("Question is reassigned to another lawyer.");
        }
            /*[HttpPut("PickQuestion")]
            public async Task<IActionResult> PickQuestion(int questionId, string answer, int lawyerid)
            {
                AnswerModel AddAnswer = new AnswerModel();
                AddAnswer.Answer = answer;

                var question = _context.questionaryModels.FindAsync(questionId);
                if(question == null)
                {
                    return NotFound("Not found");
                }
                if (question.Result.Id == questionId)
                {
                    question.Result.isPicked = true;
                    AddAnswer.QuestionId = questionId;
                    _context.Entry(question).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                var lawyer = _context.userModels.Where(user => user.Role.ToLower() == "Lawyer".ToLower() && user.Id == lawyerid).FirstOrDefault();
                if (lawyer == null)
                {
                    question.Result.LawyerId = lawyerid;
                }
                if (lawyer.Id == lawyerid)
                {
                    AddAnswer.LawyerId = lawyerid;
                }

                _context.answerModels.Add(AddAnswer);
                _context.SaveChangesAsync();

                ConversationModel conversation = new ConversationModel();
                conversation.QuestionId = questionId;
                conversation.AnswerId = AddAnswer.Id;
                return Ok("Question is picked and answered." + AddAnswer);
            }*/

        }
}
