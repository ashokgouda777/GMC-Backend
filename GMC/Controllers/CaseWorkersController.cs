using GMC.Data;
using GMC.Model;
using GMC.Model.GMC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CaseWorkersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CaseWorkersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCaseWorker([FromBody] CaseWorkerCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if email already exists
        var exists = _context.CaseWorkers
            .Any(x => x.MobileNumber == dto.MobileNumber);

        if (exists)
            return BadRequest("MobileNumber already registered");
        string generatedId = await GenerateCWUserIdAsync();
        var caseWorker = new CaseWorker
        {
            CountryId = 1,
            StateId = 1,
            CouncilId = 1,
            Name = dto.Name,
            MobileNumber = dto.MobileNumber,
            EmailId = dto.EmailId,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password), // 🔐 HASH
            RoleName = dto.RoleName,
            RoleId = 3,
            Active = true,
            CreatedOn = DateTime.Now,
            CreatedBy = dto.CreatedBy,
            CWUserId= generatedId
        };


        _context.CaseWorkers.Add(caseWorker);
        await _context.SaveChangesAsync();

        var user = new User
        {
            CouncilId = "1",
            CountryId = "1",
            StateId = "1",
            Name = dto.Name,
            UserName = dto.MobileNumber,
            CreatedOn = DateTime.Now,
            MobileNo = dto.MobileNumber,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            userId = generatedId,
            CreatedBy = dto.CreatedBy,
            Role_Id="3"
          
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new
        {
            message = "Case Worker created successfully",
            userId = caseWorker.CWUserId
        });
    }
    private async Task<string> GenerateCWUserIdAsync()
    {
        string id;
        var rnd = new Random();

        do
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int random = rnd.Next(100, 999);
            id = $"CW{timestamp}{random}";

        } while (await _context.CaseWorkers.AnyAsync(x => x.CWUserId == id));

        return id;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetCaseWorkers()
    {
        var caseWorkers = await _context.CaseWorkers
            .Where(x => x.Active == true)
            .Select(x => new CaseWorkerListDto
            {
                UserId = x.CWUserId,
                Name = x.Name,
                EmailId = x.EmailId,
                MobileNumber = x.MobileNumber,
                RoleName = x.RoleName,
                Active = x.Active
            })
            .OrderByDescending(x => x.UserId)
            .ToListAsync();

        return Ok(caseWorkers);
    }

}
