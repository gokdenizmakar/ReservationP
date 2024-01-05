using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationP.Models;

namespace ReservationP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly RDbContext _context;

        public ReservationController()
        {
            _context = new RDbContext();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var reservations = _context.Reservations.Include(x => x.Room).Include(x => x.Client.Company).ToList();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var reservation = _context.Reservations.Include(x => x.Room).Include(x => x.Client.Company).FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }


        [HttpPost]
        public IActionResult Create(ReservationM reservation)
        {
            if (reservation == null)
                return BadRequest("Invalid data");

            reservation.AddDate = DateTime.Now;
            Client client = _context.Clients.Find(reservation.ClientId);
            Room room = _context.Rooms.Find(reservation.RoomId);
            if (room == null || client == null) return BadRequest();
            reservation.Room = room;
            reservation.RoomId = room.Id;
            reservation.Client = client;
            reservation.ClientId = client.Id;

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, reservation);
        }

        [HttpPut("{id}")]
        public IActionResult Update(ReservationM reservation)
        {
            var ER = _context.Reservations.FirstOrDefault(r => r.Id == reservation.Id);

            if (ER == null)
                return NotFound();

            Client client = _context.Clients.Find(reservation.ClientId);
            Room room = _context.Rooms.Find(reservation.RoomId);
            if (room == null || client == null) return BadRequest();
            ER.Room = room;
            ER.RoomId = room.Id;
            ER.Client = client;
            ER.ClientId = client.Id;
            ER.RoomId = reservation.RoomId;
            ER.ClientId = reservation.ClientId;

            _context.Reservations.Update(ER);
            _context.SaveChanges();

            return Ok(ER);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();

            return Ok(reservation);
        }
    }
}
