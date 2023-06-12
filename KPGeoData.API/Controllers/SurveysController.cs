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
            var queryable = _context.Surveys!
                .Include(x => x.Items!)
                .ThenInclude(x => x.ItemPhotos!)
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

        [HttpGet("surveysByCompany/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetsurveysByCompany([FromQuery] PaginationDTO pagination, int id)
        {
            var queryable = _context.Surveys
                .Where(c => c.CompanyId == id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(
                    x => x.Name.ToLower().Contains(pagination.Filter.ToLower())
                         );
            }

            return Ok(await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("totalPages/{id:int}")]
        public async Task<ActionResult> GetPagesAll([FromQuery] PaginationDTO pagination, int id)
        {
            var queryable = _context.Surveys
                .Where(c => c.CompanyId == id)
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
            var survey = await _context.Surveys!.Include(x => x.Items!).ThenInclude(x => x.ItemPhotos).Include(x => x.SurveyEventTypes!)
                .Include(x => x.SurveyEventTypes!)
                .ThenInclude(x => x.EventType)
                .Include(x => x.SurveyItemTypes!)
                .ThenInclude(x => x.ItemType)
                .Include(x => x.SurveyStates!)
                .ThenInclude(x => x.State)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (survey is null)
            {
                return NotFound();
            }

            return Ok(survey);
        }

        [HttpPost]
        public async Task<ActionResult> Post(SurveyDTO surveyDTO)
        {
            try
            {
                Survey newSurvey = new()
                {
                    Name = surveyDTO.Name,
                    Active = surveyDTO.Active,
                    CompanyId = surveyDTO.CompanyId,
                    Date = DateTime.Now,
                    Id = surveyDTO.Id,
                    SurveyEventTypes = new List<SurveyEventType>(),
                    SurveyItemTypes = new List<SurveyItemType>(),
                    SurveyStates = new List<SurveyState>(),
                };

                foreach (var itemTypeIds in surveyDTO.ItemTypeIds!)
                {
                    newSurvey.SurveyItemTypes.Add(new SurveyItemType { ItemType = await _context.ItemTypes.FirstOrDefaultAsync(x => x.Id == itemTypeIds) });
                }
                foreach (var eventTypeIds in surveyDTO.EventTypeIds!)
                {
                    newSurvey.SurveyEventTypes.Add(new SurveyEventType { EventType = await _context.EventTypes.FirstOrDefaultAsync(x => x.Id == eventTypeIds) });
                }
                foreach (var stateIds in surveyDTO.StateIds!)
                {
                    newSurvey.SurveyStates.Add(new SurveyState { State = await _context.States.FirstOrDefaultAsync(x => x.Id == stateIds) });
                }

                _context.Add(newSurvey);
                await _context.SaveChangesAsync();
                return Ok(surveyDTO);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una ciudad con el mismo nombre.");
                }

                return BadRequest(dbUpdateException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }


        [HttpPut]
        public async Task<ActionResult> Put(SurveyDTO surveyDTO)
        {
            try
            {
                var survey = await _context.Surveys
                .Include(x => x.SurveyStates)
                .Include(x => x.SurveyItemTypes)
                .Include(x => x.SurveyEventTypes)
                .FirstOrDefaultAsync(x => x.Id == surveyDTO.Id);
                if (survey == null)
                {
                    return NotFound();
                }

                survey.Name = surveyDTO.Name;
                survey.Active = surveyDTO.Active;
                survey.SurveyStates = surveyDTO.StateIds!.Select(x => new SurveyState { StateId = x }).ToList();
                survey.SurveyEventTypes = surveyDTO.EventTypeIds!.Select(x => new SurveyEventType { EventTypeId = x }).ToList();
                survey.SurveyItemTypes = surveyDTO.ItemTypeIds!.Select(x => new SurveyItemType { ItemTypeId = x }).ToList();

                _context.Update(survey);
                await _context.SaveChangesAsync();
                return Ok(surveyDTO);
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

        [HttpGet("combo/{companyId:int}")]
        public async Task<ActionResult> GetCombo(int companyId)
        {
            return Ok(await _context.Surveys
                .Where(x => x.CompanyId == companyId && x.Active)
                 .OrderBy(c => c.Name)
                 .ToListAsync());
        }

    }


}