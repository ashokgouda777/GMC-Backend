using GMC.Data;
using GMC.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GMC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleMastersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoleMastersController(AppDbContext context)
        {
            _context = context;
        }

       

        [HttpGet("roles")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _context.RoleMasters
                .Select(r => new
                {
                    role_id = r.RoleId,
                    role_desc = r.RoleDesc
                })
                .ToListAsync();

            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleMaster model)
        {
            _context.RoleMasters.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoleMaster model)
        {
            if (id != model.Id) return BadRequest();
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.RoleMasters.FindAsync(id);
            if (data == null) return NotFound();

            _context.RoleMasters.Remove(data);
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }
    }
}
