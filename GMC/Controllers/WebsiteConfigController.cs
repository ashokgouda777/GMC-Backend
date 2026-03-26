using GMC.Data;
using GMC.Model;
using GMC.Model.GMC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class WebsiteConfigController : ControllerBase
{
    private readonly AppDbContext _context;

    public WebsiteConfigController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ GET by Id
    [AllowAnonymous]
    [HttpGet("{UserId}")]
    public async Task<IActionResult> GetById(string UserId)
    {
        var data = await _context.SiteSettings
                                 .FirstOrDefaultAsync(x => x.UserId == UserId);

        if (data == null)
        {
            // Return default settings
            var defaultData = new SiteSettings
            {
                UserId = UserId,
                PrimaryColor = "#F57C00",
                SecondaryColor = "#FF9800",
                TextColor = "#333333",
                BackgroundColor = "#FFF3E0",
                ButtonColor = "#F57C00",
                SidebarColor = "#FFFFFF",
                SidebarText = "#BF360C",
                SidebarActiveColor = "#F57C00",
                TopbarBg = "#FFFFFF",
                NavbarBg = "#F57C00",
                NavbarText = "#FFFFFF",
                CardBg = "#FFFFFF",
                TableHeaderBg = "#FFE0B2",
                TableHeaderText = "#E65100",
                TableBodyBg = "#FFFFFF",
                TableBodyText = "#333333",
                TableBtnColor = "#F57C00"
            };

            return Ok(defaultData);
        }

        return Ok(data);
    }

    // ✅ POST (Insert or Update)
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Post(SiteSettings model)
    {
        if (string.IsNullOrEmpty(model.UserId))
            return BadRequest("UserId is required");

        // Check if record already exists
        var existing = await _context.SiteSettings
                                     .FirstOrDefaultAsync(x => x.UserId == model.UserId);

        if (existing != null)
        {
            // 🔹 UPDATE
            _context.Entry(existing).CurrentValues.SetValues(model);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Updated successfully", data = existing });
        }
        else
        {
            // 🔹 INSERT
            _context.SiteSettings.Add(model);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Inserted successfully", data = model });
        }
    }

}
