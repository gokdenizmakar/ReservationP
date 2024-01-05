using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationP.Models;

namespace Reservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly RDbContext _context;

        public CompanyController()
        {
            _context = new RDbContext();
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _context.Companies.ToList();
            return Ok(companies);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var company = _context.Companies.FirstOrDefault(x => x.Id == id);
            if (company == null) return NotFound();
            else return Ok(company);
        }

        [HttpPost]
        public IActionResult Create(Company company)
        {
            if (company == null) return BadRequest("Şirket Bulunamadı!");

            company.AddDate = DateTime.Now;
            _context.Companies.Add(company);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, company);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, Company company)
        {
            if (company == null) return BadRequest();

            var EC = _context.Companies.Find(id);
            if (EC == null)
            {
                return NotFound();
            }

            EC.Name = company.Name;
            EC.Address = company.Address;
            _context.Update(EC);
            _context.SaveChanges();
            return Ok(EC);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var company = _context.Companies.FirstOrDefault(r => r.Id == id);

            if (company == null) return NotFound();
            else
            {
                _context.Companies.Remove(company);
                _context.SaveChanges();
                return Ok(company);
            }
        }
    }
}
