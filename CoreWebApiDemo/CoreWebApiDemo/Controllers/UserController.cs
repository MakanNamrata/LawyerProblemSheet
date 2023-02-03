using CoreWebApiDemo.DataAccess;
using CoreWebApiDemo.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreWebApiDemo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CourtDBContext _context;

        public UserController(CourtDBContext context)
        {
            _context = context;
        }


        [HttpGet("GetAllUser")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUser()
        {
            return await _context.userModels.ToListAsync();
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(string username, string password, long mobileno, DateTime birthdate)
        {
            UserModel user = new UserModel();
            user.Name = username;
            user.Password = password;
            user.MobileNo = mobileno;
            user.Birthdate = birthdate;
            user.Role = "User";
            _context.userModels.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("RegisterLawyer")]
        public async Task<IActionResult> RegisterLawyer(string username, string password, long mobileno, DateTime birthdate)
        {
            UserModel user = new UserModel();
            user.Name = username;
            user.Password = password;
            user.MobileNo = mobileno;
            user.Birthdate = birthdate;
            user.Role = "Lawyer";
            _context.userModels.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

    }
}
