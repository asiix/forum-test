using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForumUsers.Data;
using ForumUsers.Model;
using Microsoft.AspNetCore.Authorization;
using NuGet.ProjectModel;
using System.Security.Cryptography;
using ForumUsers.Authentication;

namespace ForumUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ForumUsersContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(ForumUsersContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        // GET: api/Users
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (!User.IsInRole("Admin"))
                return Unauthorized("The user does not have the necessary permission");

            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
            
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            Cryptography cryptography = new Cryptography();
            var (salt, hash) = cryptography.GenerateEncryptedKeys(user.PasswordHash);
            user.PasswordSalt = salt;
            user.PasswordHash = hash;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if(!User.IsInRole("User") && !User.IsInRole("Admin"))
                return Unauthorized("User must be logged in");

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<User> RetrieveUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);
        }

        // LOGIN
        // POST: api/Users/Login
        [HttpPost("Login")]
        public async Task<ActionResult<User>> LoginUser(User model)
        {
            User user = await RetrieveUserByEmail(model.EmailAddress);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            //AUTH LOGIC
            Cryptography cryptography = new Cryptography();
            bool canLogin = cryptography.ConfrontKeys(model.PasswordSalt, user.PasswordSalt, user.PasswordHash);
            if (canLogin == true)
            {
                string token = JwtManager.GenerateJwtToken(user, _configuration);
                Response.Cookies.Append("jwtCookie", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
                return Ok("Logged in");
            }
            else
                return BadRequest("Wrong credentials");
        }

        // LOGOUT
        // POST: api/Users/Logout
        [HttpPost("Logout")]
        public Task<ActionResult> LogoutUser()
        {
            Response.Cookies.Delete("jwtCookie");
            return Task.FromResult<ActionResult>(Ok("Successful logout"));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
