using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarRWA.Data;
using RentACarRWA.Models;

namespace RentACarRWA.Controllers
{
    [ApiController]
    [Route("api/cars")]
    public class CarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _context.Cars.ToListAsync();
            return Ok(cars);
        }


        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveCar([FromBody] ReserveRequest request)
        {
            var car = await _context.Cars.FindAsync(request.CarId);
            if (car == null)
                return NotFound("Auto nije pronađeno.");

            if (car.IsReserved)
                return BadRequest("Auto je već rezervisano.");

            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return BadRequest("Korisnik nije pronađen.");

            car.IsReserved = true;
            car.UserId = user.Id;

            await _context.SaveChangesAsync();
            return Ok(car);
        }
        [HttpGet("list")]
        public IActionResult ListAll()
        {
            var cars = _context.Cars.ToList();
            return Ok(cars);
        }

    }

    public class ReserveRequest
    {
        public int CarId { get; set; }
        public int UserId { get; set; }
    }
}
