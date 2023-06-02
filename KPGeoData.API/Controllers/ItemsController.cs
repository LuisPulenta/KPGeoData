using KPGeoData.API.Data;
using KPGeoData.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KPGeoData.API.Controllers
{
   
        [ApiController]
        [Route("/api/items")]
        public class ItemsController : ControllerBase
        {
            private readonly DataContext _context;

            public ItemsController(DataContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult> Get()
            {
                return Ok(await _context.Items
                    .ToListAsync());
            }

            [HttpGet("{id:int}")]
            public async Task<ActionResult> Get(int id)
            {
                var item = await _context.Items
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (item is null)
                {
                    return NotFound();
                }

                return Ok(item);
            }


        [HttpGet("full/{id:int}")]
        public async Task<ActionResult> GetFull(int id)
        {
            var item = await _context.Items
                .Include(x => x.ItemPhotos)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }



        [HttpPost]
            public async Task<ActionResult> Post(Item item)
            {
                item.Date=DateTime.Now;
                _context.Add(item);
                try
                {
                    await _context.SaveChangesAsync();
                    return Ok(item);
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException!.Message.Contains("duplicada"))
                    {
                        return BadRequest("Ya existe un Item con el mismo nombre.");
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
            public async Task<ActionResult> Put(Item item)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                    return Ok(item);
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException!.Message.Contains("duplicada"))
                    {
                        return BadRequest("Ya existe un Item con el mismo nombre.");
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
                var afectedRows = await _context.Items
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
