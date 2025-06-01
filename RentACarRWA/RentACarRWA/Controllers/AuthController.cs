using Microsoft.AspNetCore.Mvc;
using RentACarRWA.Data;
using RentACarRWA.Models;

namespace RentACarRWA.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                return BadRequest("Korisnik već postoji.");
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Username == login.Username &&
                u.PasswordHash == login.PasswordHash);

            if (user == null)
                return BadRequest("Pogrešno korisničko ime ili lozinka.");

            return Ok(user);
        }
    }
}
