using GMC.Data;
using GMC.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GMC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MasterDataController(AppDbContext context)
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

        [HttpGet("registrationfor")]
        public async Task<IActionResult> registrationfor()
        {
            var roles = await _context.RegistrationFor
                .Select(r => new
                {
                    regid = r.RegId,
                    registrationfor = r.RegForName
                })
                .ToListAsync();

            return Ok(roles);
        }
        [HttpGet("titlesmaster")]
        public async Task<IActionResult> titlesmaster()
        {
            var title = await _context.TitleMaster
                .Select(r => new
                {
                    TitleId = r.TitleId,
                    TitleName = r.TitleName
                })
                .ToListAsync();

            return Ok(title);
        }

        [HttpGet("gendermaster")]
        public async Task<IActionResult> gendermaster()
        {
            var gen = await _context.GenderMaster
                .Select(r => new
                {
                    GenderId = r.GenderId,
                    GenderName = r.GenderName
                })
                .ToListAsync();

            return Ok(gen);
        }
        [HttpGet("blodgroupmaster")]
        public async Task<IActionResult> blodgroupmaster()
        {
            var blod = await _context.BloodGroupTypeMaster
                .Select(r => new
                {
                    BloodGroupCode = r.BloodGroupCode,
                    BloodGroupDescription = r.BloodGroupDescription
                })
                .ToListAsync();

            return Ok(blod);
        }

        [HttpGet("nationalitymaster")]
        public async Task<IActionResult> nationalitymaster()
        {
            var nation = await _context.NationalityMaster
                .Select(r => new
                {
                    NationalityId = r.NationalityId,
                    Nationality = r.Nationality
                })
                .ToListAsync();

            return Ok(nation);
        }

        [HttpGet("eligiblity")]
        public async Task<IActionResult> eligiblitynationalitymaster()
        {
            var nation = await _context.PractitionerEligiblity
                .Select(r => new
                {
                    EligibiltyId = r.EligibiltyId,
                    Eligibilty = r.Eligibilty
                })
                .ToListAsync();

            return Ok(nation);
        }

        [HttpGet("countrymaster")]
        public async Task<IActionResult> countrymaster()
        {
            var nation = await _context.CountryMaster
                .Select(r => new
                {
                    CountryId = r.CountryId,
                    CountryName = r.CountryName
                })
                .ToListAsync();

            return Ok(nation);
        }
        [HttpGet("statemaster")]
        public async Task<IActionResult> statemaster(string countryId)
        {
            if (string.IsNullOrEmpty(countryId))
            {
                return BadRequest("CountryId is required");
            }

            var states = await _context.StateMaster
                .Where(r => r.CountryId == countryId)
                .Select(r => new
                {
                    StateId = r.StateId,
                    StateName = r.StateName
                })
                .ToListAsync();

            return Ok(states);
        }


        [HttpGet("districtmaster")]
        public async Task<IActionResult> districtmaster(string stateId)
        {
            var nation = await _context.DistrictMaster
                .Where(r => r.StateId == stateId)
                .Select(r => new
                {
                    DistrictId = r.DistrictId,
                    DistrictName = r.DistrictName
                })
                .ToListAsync();

            return Ok(nation);
        }

        [HttpGet("coursemaster")]
        public async Task<IActionResult> coursemaster()
        {
            var nation = await _context.CourseMaster
                
                .Select(r => new
                {
                    CourseId = r.CourseId,
                    CourseDescription = r.CourseShortCode
                })
                .ToListAsync();

            return Ok(nation);
        }
        [HttpGet("uiniversitys")]
        public async Task<IActionResult> uiniversitys()
        {
            var nation = await _context.University

                .Select(r => new
                {
                    UniversityId = r.UniversityId,
                    UniversityName = r.UniversityName
                })
                .ToListAsync();

            return Ok(nation);
        }
       
        [HttpGet("colleges")]
        public async Task<IActionResult> GetColleges(string universityId)
        {
            var colleges = await _context.College
                .Where(r => r.UniversityName == universityId)
                .Select(r => new
                {
                  colid=r.ColId,
                  colname= r.ColName

                })
                .ToListAsync();

            return Ok(colleges);
        }

        [HttpPost("create-council")]
        public async Task<IActionResult> CreateCouncil([FromBody] JCouncilMaster model)
        {
            if (model == null)
                return BadRequest("Invalid data.");

            // Optional: Check duplicate CouncilId
            var exists = await _context.JCouncilMaster
                .AnyAsync(x => x.CouncilId == model.CouncilId);

            if (exists)
                return BadRequest("CouncilId already exists.");

            model.CreatedOn = DateTime.Now;

            _context.JCouncilMaster.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Council created successfully",
                data = model
            });
        }
        [AllowAnonymous]
        [HttpGet("GetLedger")]
        public async Task<IActionResult> GetLedger()
        {
            var ledgerList = await (
                from gl in _context.GroupLedgerMaster
                join lf in _context.LedgerFeeItemLink
                    on gl.LedgerID equals lf.LedgerID
                join fi in _context.FeesItems
                    on Convert.ToInt32(lf.FeeItemID) equals fi.FeeItemId
                where gl.LedgerStatus=="A"
                select new
                {
                    LedgerID = gl.LedgerID,
                    LedgerName = gl.LedgerDescription??"",
                    FeeItemID = fi.FeeItemId,
                    FeeItemName = fi.FeeItemName,
                    FeeAmount = fi.FeeAmount,
                    CertificateType = lf.Certificate_Type,
                    Status = lf.Status
                }
            ).ToListAsync();

            return Ok(ledgerList);
        }

        [HttpGet("financialyear")]
        public async Task<IActionResult> financialyear()
        {
            var nation = await _context.Financial_Year

                .Select(r => new
                {
                    years = r.FinancialYear,
                    
                })
                .ToListAsync();

            return Ok(nation);
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> accounts()
        {
            var nation = await _context.CBAccountMaster

                .Select(r => new
                {
                    account = r.AccountNo,

                })
                .ToListAsync();

            return Ok(nation);
        }



        [HttpGet("cbgroups")]
        public async Task<IActionResult> cbgroups()
        {
            var nation = await _context.CBGroupMaster

                .Select(r => new
                {
                    account = r.GroupID,

                })
                .ToListAsync();

            return Ok(nation);
        }
    }
}
