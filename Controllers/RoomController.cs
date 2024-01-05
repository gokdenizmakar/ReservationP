using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationP.Models;

namespace ReservationP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RDbContext _context;

        public RoomController()
        {
            _context = new RDbContext();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var rooms = _context.Rooms.ToList();
            return Ok(rooms);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var room = _context.Rooms.FirstOrDefault(x => x.Id == id);
            if (room == null) return NotFound();
            else return Ok(room);
        }

        [HttpPost]
        public IActionResult Create(Room room)
        {
            if (room == null) return BadRequest("Geçersiz veri.");

            room.AddDate = DateTime.Now;
            _context.Rooms.Add(room);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, room);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, Room room)
        {
            if (room == null) return BadRequest();

            var existingRoom = _context.Rooms.Find(id);
            if (existingRoom == null)
            {
                return NotFound();
            }

            existingRoom.Name = room.Name;
            _context.Update(existingRoom);
            _context.SaveChanges();
            return Ok(existingRoom);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);

            if (room == null) return NotFound();
            else
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
                return Ok(room);
            }
        }
    }
}
