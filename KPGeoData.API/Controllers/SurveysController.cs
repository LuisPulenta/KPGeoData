using KPGeoData.API.Data;
using KPGeoData.API.Helpers;
using KPGeoData.Shared.DTOs;
using KPGeoData.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KPGeoData.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/surveys")]
    public class SurveysController : ControllerBase
    {
        private readonly DataContext _context;

        public SurveysController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Surveys
                .Include(x => x.Items)
                .ThenInclude(x => x.ItemPhotos)
                .ThenInclude(x => x.EventType)
                .Where(x => x.Company!.Id == pagination.Id)
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
            var queryable = _context.Surveys
                .Where(x => x.Company!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }


            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var survey = await _context.Surveys
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (survey is null)
            {
                return NotFound();
            }

            return Ok(survey);
        }

        [HttpGet("full/{id:int}")]
        public async Task<ActionResult> GetFull(int id)
        {
            var survey = await _context.Surveys
                .Include(x => x.Items)
                .ThenInclude(x => x.ItemPhotos)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (survey is null)
            {
                return NotFound();
            }

            return Ok(survey);
        }



        [HttpPost]
        public async Task<ActionResult> Post(Survey survey)
        {
            survey.Date = DateTime.Now;
            _context.Add(survey);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(survey);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest("Ya existe un Relevamiento con el mismo nombre.");
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

        [HttpPut]
        public async Task<ActionResult> Put(Survey survey)
        {
            try
            {
                _context.Update(survey);
                await _context.SaveChangesAsync();
                return Ok(survey);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest("Ya existe un Relevamiento con el mismo nombre.");
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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var afectedRows = await _context.Surveys
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (afectedRows == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }


}