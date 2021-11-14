using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothingApi.Infrastructure;
using ClothingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingApi.Controllers
{
    [Route("api/clothingitems")]
    [ApiController]
    public class ClothingItemsController : ControllerBase
    {
        private readonly ClothingContext _context;

        public ClothingItemsController(ClothingContext context)
        {
            _context = context;
        }

        [AuthorizeEnum(Roles.View)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClothingItemDto>>> GetClothingItems()
        {
            return await _context.ClothingItems.ToListAsync();
        }

        [AuthorizeEnum(Roles.View)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ClothingItemDto>> GetClothingItem(Guid id)
        {
            var clothingItem = await _context.ClothingItems.FindAsync(id);

            if (clothingItem == null)
            {
                return NotFound();
            }

            return clothingItem;
        }

        [AuthorizeEnum(Roles.Editor)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClothingItem(Guid id, ClothingItemDto clothingItem)
        {
            if (id != clothingItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(clothingItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClothingItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [AuthorizeEnum(Roles.Editor)]
        [HttpPost]
        public async Task PostClothingItem(List<ClothingItemDto> clothingItems)
        {
            if(clothingItems.Count == 0)
            {
                throw new ArgumentNullException("Please add item(s).");
            }

            foreach (ClothingItemDto clothingItem in clothingItems)
            {
                _context.ClothingItems.Add(clothingItem);
            }
            
            await _context.SaveChangesAsync();
        }

        [AuthorizeEnum(Roles.Manager)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClothingItem(Guid id)
        {
            ClothingItemDto clothingItem = await _context.ClothingItems.FindAsync(id);

            if (clothingItem == null)
            {
                return NotFound();
            }

            _context.ClothingItems.Remove(clothingItem);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClothingItemExists(Guid id)
        {
            return _context.ClothingItems.Any(e => e.Id == id);
        }
    }
}
