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
    [Route("/api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly DataContext _context;

        public ItemsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination, DataContext _context)
        {
            var queryable = _context.Items!
                .Include(x => x.ItemPhotos!)
                .ThenInclude(x => x.EventType)
                .Where(x => x.Survey!.Id == pagination.Id)
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
            var queryable = _context.Items
                .Where(x => x.Survey!.Id == pagination.Id)
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
            var item = await _context.Items
                .Include(x => x.ItemPhotos)
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
            item.Date = DateTime.Now;
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