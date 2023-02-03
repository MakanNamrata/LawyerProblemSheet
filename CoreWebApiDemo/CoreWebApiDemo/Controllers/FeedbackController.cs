using CoreWebApiDemo.DataAccess;
using CoreWebApiDemo.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApiDemo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly CourtDBContext _context;

        public FeedbackController(CourtDBContext context)
        {
            _context = context;
        }

        [HttpPost("GiveFeedbackToLawyer")]
        public async Task<IActionResult> GiveFeedbackToLawyer(string? feedback, int? rate, int lawyerid, int userid)
        {

            FeedbackModel addFeedback = new FeedbackModel();
            addFeedback.Feedback = feedback;
            addFeedback.rate = rate;

            var lawyer = _context.userModels.Where(user => user.Role.ToLower() == "Lawyer".ToLower() && user.Id == lawyerid).FirstOrDefault();
            if (lawyer == null)
            {
                return Problem("No lawyer found of this id. Try again.");
            }
            if (lawyer.Id == lawyerid)
            {
                addFeedback.LawyerId = lawyerid;
            }

            var user = _context.userModels.Where(user => user.Role.ToLower() == "User".ToLower() && user.Id == userid).FirstOrDefault();
            if (user == null)
            {
                return Problem("No user found of this id. Try again.");
            }
            if (user.Id == userid)
            {
                addFeedback.UserId = userid;
            }
            
            _context.feedbackModels.Add(addFeedback);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("AverageRating")]
        public async Task<ActionResult> AverageRating(int lawyerid)
        {
            var Rate = from feedback in _context.feedbackModels
                        where feedback.LawyerId == lawyerid
                        select feedback.rate;
            var average = Rate.Average();
            return Ok(average);
        }

    }
}
