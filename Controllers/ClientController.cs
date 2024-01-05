using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationP.Models;

namespace Reservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly RDbContext _context;
        public ClientController()
        {
            _context = new RDbContext();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var clients = _context.Clients.Include(x => x.Company).ToList();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var client = _context.Clients.Include(x => x.Company).FirstOrDefault(x => x.Id == id);
            if (client == null) return NotFound();
            else return Ok(client);
        }

        [HttpPost]
        public IActionResult Create(Client client)
        {
            if (client == null) return BadRequest("Geçersiz veri.");

            Company company = _context.Companies.Find(client.CompanyId);
            if (company == null) return BadRequest("Şirket geçersiz.");

            client.AddDate = DateTime.Now;
            client.Company = company;
            _context.Clients.Add(client);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, client);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Client client)
        {
            if (client == null) return BadRequest();

            var existingClient = _context.Clients.Find(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            existingClient.Name = client.Name;
            existingClient.Surname = client.Surname;
            existingClient.BirthDate = client.BirthDate;
            existingClient.Address = client.Address;
            existingClient.Email = client.Email;

            Company company = _context.Companies.Find(client.CompanyId);
            if (company == null) return BadRequest("Şirket Bulunamadı!");
            existingClient.CompanyId = company.Id;
            existingClient.Company = company;

            _context.Update(existingClient);
            _context.SaveChanges();
            return Ok(existingClient);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var client = _context.Clients.FirstOrDefault(c => c.Id == id);

            if (client == null) {
                return NotFound();
            }
            else
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
                return Ok(client);
            }
        }
    }
}
