using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KPGeoData.API.Data;
using KPGeoData.Shared.Entities;
using KPGeoData.Shared.DTOs;
using KPGeoData.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Azure.Core;
using KPGeoData.Shared.Helpers;

namespace KPGeoData.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IFilesHelper _filesHelper;

        public CompaniesController(DataContext context, IFilesHelper filesHelper)
        {
            _context = context;
            _filesHelper = filesHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Companies
                .Include(x => x.Surveys)
                .Where(x => x.Id != 1)
                .OrderBy(x => x.Name)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }


            return Ok(await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Companies.AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }


        [HttpPost]
        public async Task<ActionResult> Post(Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Foto
            var imageUrl = string.Empty;
            byte[] imageArray = Convert.FromBase64String(company.Photo);
            var stream = new MemoryStream(imageArray);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var folder = "wwwroot\\images\\Logos";
            var fullPath = $"~/images/Logos/{file}";
            var response = _filesHelper.UploadPhoto(stream, folder, file);

            if (response)
            {
                imageUrl = fullPath;
                company.Photo = imageUrl;
            }

            _context.Add(company);
            try
            {
                await _context.SaveChangesAsync();
            return Ok(company);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest("Ya existe una empresa con el mismo nombre.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var company = await _context.Companies
                .Include(x => x.Surveys)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (company is null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Company company)

        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Company oldCompany = await _context.Companies.FirstOrDefaultAsync(o=>o.Id == company.Id);
                        
            //Foto
            string imageUrl = string.Empty;
            if (company.Photo != null && company.Photo.Length > 0)
            {
                imageUrl = string.Empty;
                byte[] imageArray = Convert.FromBase64String(company.Photo);
                var stream = new MemoryStream(imageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot\\images\\Logos";
                var fullPath = $"~/images/Logos/{file}";
                var response = _filesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    imageUrl = fullPath;
                    oldCompany.Photo = imageUrl;
                }
            }
            

            oldCompany!.Active = company.Active;
            oldCompany!.Address=company.Address;
            oldCompany!.Contact = company.Contact;
            oldCompany.CUIT = company.Contact;
            oldCompany!.EMail = company.EMail;
            oldCompany!.Phone = company.Phone;
            oldCompany!.Name = company.Name;
            
            _context.Update(oldCompany);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest("Ya existe una empresa con el mismo nombre.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok(company);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Remove(company);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("combo")]
        public async Task<ActionResult> GetCombo()
        {
            return Ok(await _context.Companies
                  .OrderBy(c => c.Name)
                  .Where(c => c.Active && c.Id!=1)
                  .ToListAsync());
        }


    }
}
