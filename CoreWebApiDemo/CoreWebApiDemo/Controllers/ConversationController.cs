using CoreWebApiDemo.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreWebApiDemo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly CourtDBContext _context;

        public ConversationController(CourtDBContext context)
        {
            _context = context;
        }

        [HttpGet("MyConversation")]
        public async Task<IActionResult> MyConversation(int userid)
        {
            var user = await _context.userModels.FindAsync(userid);
            var questionary = await _context.questionaryModels.Where(data => data.UserId == user.Id).FirstOrDefaultAsync();

            var conversation = from data in _context.conversationModels
                               join question in _context.questionaryModels on data.QuestionId equals question.Id
                               where data.QuestionId == questionary.Id
                               select new
                               {
                                   question.Question,
                                   data.Answer.Answer
                               };

            return Ok(conversation);
        }


    }
}
