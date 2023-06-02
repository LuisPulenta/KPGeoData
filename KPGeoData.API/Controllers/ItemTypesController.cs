using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KPGeoData.API.Data;
using KPGeoData.Shared.Entities;

namespace KPGeoData.API.Controllers

{
    [ApiController]
    [Route("/api/itemTypes")]
    public class ItemTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public ItemTypesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.ItemTypes
                .OrderBy(x => x.Name)
                .ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Post(ItemType itemType)
        {
            _context.Add(itemType);
            try
            {
                await _context.SaveChangesAsync();
            return Ok(itemType);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest("Ya existe un ítem con el mismo nombre.");
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
            var itemType = await _context.ItemTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (itemType is null)
            {
                return NotFound();
            }

            return Ok(itemType);
        }

        [HttpPut]
        public async Task<ActionResult> Put(ItemType itemType)
        {
            _context.Update(itemType);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicada"))
                {
                    return BadRequest("Ya existe un ítem con el mismo nombre.");
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

            return Ok(itemType);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var itemType = await _context.ItemTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (itemType == null)
            {
                return NotFound();
            }

            _context.Remove(itemType);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
