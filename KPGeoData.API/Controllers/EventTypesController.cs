using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KPGeoData.API.Data;
using KPGeoData.Shared.Entities;

namespace KPGeoData.API.Controllers

{
    [ApiController]
    [Route("/api/eventTypes")]
    public class EventTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public EventTypesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.EventTypes
                .OrderBy(x => x.Name)
                .ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Post(EventType eventType)
        {
            _context.Add(eventType);
            try
            {
                await _context.SaveChangesAsync();
            return Ok(eventType);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest("Ya existe un evento con el mismo nombre.");
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
            var eventType = await _context.EventTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (eventType is null)
            {
                return NotFound();
            }

            return Ok(eventType);
        }

        [HttpPut]
        public async Task<ActionResult> Put(EventType eventType)
        {
            _context.Update(eventType);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest("Ya existe un evento con el mismo nombre.");
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

            return Ok(eventType);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var eventType = await _context.EventTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (eventType == null)
            {
                return NotFound();
            }

            _context.Remove(eventType);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
