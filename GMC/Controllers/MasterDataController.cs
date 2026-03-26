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
                    colid = r.ColId,
                    colname = r.ColName

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

        [HttpGet("GetLedger")]
        public async Task<IActionResult> GetLedger()
        {
            var ledgerList = await (
                from gl in _context.GroupLedgerMaster
                join lf in _context.LedgerFeeItemLink
                    on gl.LedgerID equals lf.LedgerID
                join fi in _context.FeesItems
                    on Convert.ToInt32(lf.FeeItemID) equals fi.FeeItemId
                where gl.LedgerStatus == "A"
                select new
                {
                    LedgerID = gl.LedgerID,
                    LedgerName = gl.LedgerDescription ?? "",
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
        [HttpGet("statemastersget")]
        public async Task<IActionResult> GetStateMaster()
        {
            try
            {

                var states = await _context.StateMaster
                    .AsNoTracking()
                    .Join(_context.CountryMaster,
                        s => s.CountryId,
                        c => c.CountryId,
                        (s, c) => new
                        {
                            StateId = s.StateId,
                            CountryId = s.CountryId,
                            CountryName = c.CountryName ?? "Unknown",
                            StateName = s.StateName ?? "",
                            Active = s.Active ?? "No"
                        })
                    .ToListAsync();

                return Ok(states);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPost("statemaster-save")]
        public async Task<IActionResult> CreateOrUpdateState([FromBody] StateMaster model)
        {
            try
            {
                if (model == null)
                    return BadRequest(new { message = "Invalid state model" });

                if (string.IsNullOrEmpty(model.StateName))
                    return BadRequest(new { message = "State name is required." });

                if (string.IsNullOrEmpty(model.CountryId))
                    return BadRequest(new { message = "Country selection is required." });

                // 🔍 Check existing
                var existing = await _context.StateMaster
                    .FirstOrDefaultAsync(s => s.StateId == model.StateId);

                // =========================
                // ✅ INSERT
                // =========================
                if (existing == null)
                {
                    // Duplicate check
                    var duplicate = await _context.StateMaster
                        .AnyAsync(s => s.StateName.ToLower() == model.StateName.ToLower()
                                    && s.CountryId == model.CountryId);

                    if (duplicate)
                        return BadRequest(new { message = "State already exists for this country." });

                    // Generate StateId
                    var ids = await _context.StateMaster
                        .Select(s => s.StateId)
                        .ToListAsync();

                    int maxId = 0;
                    foreach (var id in ids)
                    {
                        if (int.TryParse(id, out int idInt) && idInt > maxId)
                            maxId = idInt;
                    }

                    model.StateId = (maxId + 1).ToString();
                    model.CreatedOn = DateTime.Now;
                    model.Active = "A";


                    _context.StateMaster.Add(model);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "State created successfully",
                        data = model
                    });
                }

                // =========================
                // 🔁 UPDATE
                // =========================
                var duplicateName = await _context.StateMaster
                    .AnyAsync(s => s.StateId != model.StateId &&
                                   s.StateName.ToLower() == model.StateName.ToLower() &&
                                   s.CountryId == model.CountryId);

                if (duplicateName)
                    return BadRequest(new { message = "State name already exists for this country." });

                existing.StateName = model.StateName;
                existing.CountryId = model.CountryId;
                existing.Active = model.Active ?? existing.Active;
                existing.UpdatedOn = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "State updated successfully",
                    data = existing
                });
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? $"; Inner: {ex.InnerException.Message}" : "";
                return StatusCode(500, new
                {
                    message = "Backend Error: " + ex.Message + innerMsg
                });
            }
        }

        [HttpGet("eligiblity")]
        public async Task<IActionResult> eligiblitynationalitymaster()
        {
            var result = await _context.PractitionerEligiblity
                .AsNoTracking()
                .Select(r => new
                {
                    EligibiltyId = r.EligibiltyId,
                    Eligibilty = r.Eligibilty ?? ""
                })
                .ToListAsync();

            return Ok(result);
        }
        [HttpPost("eligiblity")]
        public async Task<IActionResult> AddEligiblity([FromBody] PractitionerEligiblity model)
        {
            if (model == null)
                return BadRequest("Invalid data");



            _context.PractitionerEligiblity.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Eligiblity added successfully",
                data = model
            });
        }

        [HttpGet("countrymastersget")]
        public async Task<IActionResult> GetCountryMaster()
        {
            var countries = await _context.CountryMaster
                .AsNoTracking()
                .Select(c => new
                {
                    id = c.id,
                    CountryId = c.CountryId,
                    CountryName = c.CountryName,
                    Active = c.Active
                })
                .ToListAsync();

            return Ok(countries);
        }

        [HttpPost("countrymaster-save")]
        public async Task<IActionResult> CreateOrUpdateCountry([FromBody] CountryMaster model)
        {
            try
            {
                if (model == null)
                    return BadRequest(new { message = "Invalid country model" });

                var normalizedName = model.CountryName?.Trim();
                if (string.IsNullOrEmpty(normalizedName))
                    return BadRequest(new { message = "Country name is required." });


                var existing = await _context.CountryMaster
                    .FirstOrDefaultAsync(c => c.id == model.id);


                // ✅ INSERT
                // =========================
                if (existing == null)
                {
                    var duplicate = await _context.CountryMaster
                        .AnyAsync(c => c.CountryName.ToLower() == normalizedName.ToLower());

                    if (duplicate)
                        return BadRequest(new { message = "Country already exists." });

                    var lastEntry = await _context.CountryMaster
                        .OrderByDescending(c => c.id)
                        .FirstOrDefaultAsync();

                    int nextVal = (lastEntry?.id ?? 0) + 1;

                    model.CountryId = nextVal.ToString();
                    model.Active = string.IsNullOrEmpty(model.Active) ? "Yes" : model.Active;
                    model.CreatedOn = DateTime.Now;

                    _context.CountryMaster.Add(model);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "Country created successfully",
                        data = model
                    });
                }


                // 🔁 UPDATE
                // =========================
                var duplicateName = await _context.CountryMaster
                    .AnyAsync(c => c.id != model.id &&
                                   c.CountryName.ToLower() == normalizedName.ToLower());

                if (duplicateName)
                    return BadRequest(new { message = "Country name already exists." });

                existing.CountryName = model.CountryName;
                existing.Active = model.Active ?? existing.Active;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Country updated successfully",
                    data = existing
                });
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? $"; Inner: {ex.InnerException.Message}" : "";
                return StatusCode(500, new
                {
                    message = "Backend Error: " + ex.Message + innerMsg
                });
            }
        }

        [HttpGet("districtmastersget")]
        public async Task<IActionResult> GetDistrictMaster()
        {
            try
            {
                var districts = await _context.DistrictMaster
                    .AsNoTracking()
                    .Join(_context.CountryMaster,
                        d => d.CountryId,
                        c => c.CountryId,
                        (d, c) => new { d, c })
                    .Join(_context.StateMaster,
                        dc => dc.d.StateId,
                        s => s.StateId,
                        (dc, s) => new
                        {
                            DistrictId = dc.d.DistrictId,
                            CountryId = dc.d.CountryId,
                            CountryName = dc.c.CountryName ?? "Unknown",
                            StateId = dc.d.StateId,
                            StateName = s.StateName ?? "Unknown",
                            DistrictName = dc.d.DistrictName ?? "",

                        })
                    .ToListAsync();

                return Ok(districts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("districtmaster-save")]
        public async Task<IActionResult> CreateOrUpdateDistrict([FromBody] DistrictMaster model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new { message = "Invalid district model" });
                }

                if (string.IsNullOrEmpty(model.DistrictName))
                {
                    return BadRequest(new { message = "District name is required." });
                }

                if (string.IsNullOrEmpty(model.StateId))
                {
                    return BadRequest(new { message = "State is required." });
                }

                if (string.IsNullOrEmpty(model.CountryId))
                {
                    return BadRequest(new { message = "Country is required." });
                }

                // 🔍 Check if exists
                var existing = await _context.DistrictMaster
                    .FirstOrDefaultAsync(d => d.DistrictId == model.DistrictId);

                // =========================
                // ✅ INSERT
                // =========================
                if (existing == null)
                {

                    var duplicate = await _context.DistrictMaster
                        .AnyAsync(d => d.DistrictName.ToLower() == model.DistrictName.ToLower()
                                    && d.StateId == model.StateId);

                    if (duplicate)
                    {
                        return BadRequest(new { message = "District already exists for this state." });
                    }

                    // 🔢 Generate DistrictId
                    var ids = await _context.DistrictMaster
                        .Select(d => d.DistrictId)
                        .ToListAsync();

                    int maxId = 0;

                    foreach (var id in ids)
                    {
                        if (int.TryParse(id, out int idInt))
                        {
                            if (idInt > maxId)
                                maxId = idInt;
                        }
                    }

                    model.DistrictId = (maxId + 1).ToString();
                    model.CreatedOn = DateTime.Now;
                    model.Status = "A";
                    model.CreatedBy = "Admin";

                    _context.DistrictMaster.Add(model);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "District created successfully",
                        data = model
                    });
                }
                else
                {
                    // =========================
                    // 🔁 UPDATE
                    // =========================

                    var duplicateName = await _context.DistrictMaster
                        .AnyAsync(d => d.DistrictId != model.DistrictId &&
                                       d.DistrictName.ToLower() == model.DistrictName.ToLower() &&
                                       d.StateId == model.StateId);

                    if (duplicateName)
                    {
                        return BadRequest(new { message = "District name already exists for this state." });
                    }

                    existing.DistrictName = model.DistrictName;
                    existing.StateId = model.StateId;
                    existing.CountryId = model.CountryId;

                    existing.UpdatedOn = DateTime.Now;

                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "District updated successfully",
                        data = existing
                    });
                }
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? $"; Inner: {ex.InnerException.Message}" : "";

                return StatusCode(500, new
                {
                    message = "Backend Error: " + ex.Message + innerMsg
                });
            }
        }


        [HttpGet("coursemastersget")]
        public async Task<IActionResult> GetCourseMaster()
        {
            try
            {
                var courses = await _context.CourseMaster
                    .AsNoTracking()
                    .Select(c => new
                    {
                        courseName = c.CourseDescription,
                        Nomenclature = c.CourseNomeclature,
                        ShortcutCode = c.CourseShortCode,
                        additionalDegree = c.AdditionalDegree,
                        status = c.Status,
                        courseid = c.CourseId
                    })
                    .ToListAsync();

                return Ok(new
                {
                    message = "Courses fetched successfully",
                    count = courses.Count,
                    data = courses
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error fetching courses",
                    error = ex.Message
                });
            }
        }
        [AllowAnonymous]
        [HttpPost("coursemaster-save")]
        public async Task<IActionResult> CreateOrUpdateCourse([FromBody] CourseMaster model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new { message = "Invalid course model" });
                }

                if (string.IsNullOrEmpty(model.CourseDescription))
                {
                    return BadRequest(new { message = "Course name is required." });
                }

                // 🔍 Check existing
                var existing = await _context.CourseMaster
                    .FirstOrDefaultAsync(c => c.CourseId == model.CourseId);

                // =========================
                // ✅ INSERT
                // =========================
                if (existing == null)
                {
                    // Generate Id if not provided
                    model.CourseId = string.IsNullOrEmpty(model.CourseId)
                        ? Guid.NewGuid().ToString()
                        : model.CourseId;


                    model.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    model.Status = "A";

                    _context.CourseMaster.Add(model);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "Course created successfully",
                        data = model
                    });
                }
                else
                {



                    existing.CourseDescription = model.CourseDescription;
                    existing.CourseShortCode = model.CourseShortCode;
                    existing.CourseNomeclature = model.CourseNomeclature;
                    existing.AdditionalDegree = model.AdditionalDegree;



                    existing.UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "Course updated successfully",
                        data = existing
                    });
                }
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? $"; Inner: {ex.InnerException.Message}" : "";

                return StatusCode(500, new
                {
                    message = "Backend Error: " + ex.Message + innerMsg
                });
            }
        }



        [HttpGet("mdssubjectmastersget")]
        public async Task<IActionResult> GetMdsSubjectMaster()
        {
            try
            {
                var subjects = await _context.MdsSubjectMaster
                    .AsNoTracking()
                    .ToListAsync();
                return Ok(subjects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("mdssubjectmasterspost")]
        public async Task<IActionResult> CreateMdsSubjectMaster([FromBody] MdsSubjectMaster model)
        {
            try
            {
                if (model == null) return BadRequest(new { message = "Invalid subject model" });

                if (string.IsNullOrEmpty(model.Sub_name))
                {
                    return BadRequest(new { message = "Subject name is required." });
                }

                if (string.IsNullOrEmpty(model.Sub_code))
                {
                    model.Sub_code = Guid.NewGuid().ToString().Substring(0, 8);
                }

                model.CreatedOn = DateTime.Now;
                model.ActiveStatus = string.IsNullOrEmpty(model.ActiveStatus) ? "Active" : model.ActiveStatus;



                _context.MdsSubjectMaster.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Subject saved successfully", data = model });
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? $"; Inner: {ex.InnerException.Message}" : "";
                return StatusCode(500, new { message = "Backend Error: " + ex.Message + innerMsg });
            }
        }

        [HttpPut("mdssubjectmastersupdate/{id}")]
        public async Task<IActionResult> UpdateMdsSubjectMaster(string id, [FromBody] MdsSubjectMaster model)
        {
            try
            {
                var subject = await _context.MdsSubjectMaster.FirstOrDefaultAsync(s => s.Sub_code == id);
                if (subject == null) return NotFound("Subject not found");

                subject.Sub_name = model.Sub_name;
                subject.ShortCode = model.ShortCode;
                subject.CourseId = model.CourseId;
                subject.ActiveStatus = model.ActiveStatus;




                subject.UpdatedBy = model.UpdatedBy;
                subject.UpdatedOn = DateTime.Now;

                await _context.SaveChangesAsync();
                return Ok(new { message = "Subject updated successfully", data = subject });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("mdssubjectmastersdelete/{id}")]
        public async Task<IActionResult> DeleteMdsSubjectMaster(string id)
        {
            try
            {
                var subject = await _context.MdsSubjectMaster.FirstOrDefaultAsync(s => s.Sub_code == id);
                if (subject == null) return NotFound("Subject not found");

                _context.MdsSubjectMaster.Remove(subject);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Subject deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }
        [HttpGet("universityget/{id}")]
        public async Task<ActionResult<University>> GetUniversity(string id)
        {
            var university = await _context.University
                .Where(u => u.UniversityId == id)
                .FirstOrDefaultAsync();

            if (university == null)
                return NotFound();

            return university;
        }
        [AllowAnonymous]
        [HttpGet("universitymastersget")]
        public async Task<IActionResult> GetUniversities()
        {
            try
            {
                var universities = await _context.University
                    .AsNoTracking()
                    .Select(u => new
                    {
                        university_id = u.UniversityId,
                        university_name = u.UniversityName,
                        university_code = u.UniversityCode,
                        status = u.Status ?? "A"
                    })
                    .ToListAsync();

                return Ok(universities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpPost("universitypost")]
        public async Task<IActionResult> PostUniversity([FromBody] University model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Invalid data");

                if (string.IsNullOrEmpty(model.UniversityCode))
                    return BadRequest("University Code is required.");



                // 🔍 Check if exists
                var existing = await _context.University
                    .FirstOrDefaultAsync(x => x.UniversityId == model.UniversityId);

                if (existing == null)
                {
                    string uinid = GenerateUniversityId();
                    University uin = new University
                    {
                        CouncilId = "1",
                        StateId = "1",
                        UniversityCode = model.UniversityCode,
                        CountryId = "1",
                        UniversityName = model.UniversityName,
                        Status = "A",
                        UniversityId = uinid,
                        CreatedBy = "Admin",
                        CreatedOn = DateTime.Now

                    };

                    // ✅ INSERT
                    _context.University.Add(uin);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "University created successfully",
                        id = model.UniversityId
                    });
                }
                else
                {
                    // ✅ UPDATE
                    existing.UniversityName = model.UniversityName;
                    existing.UniversityCode = model.UniversityCode;
                    existing.UpdatedBy = model.UpdatedBy;
                    existing.UpdatedOn = DateTime.Now;

                    if (!string.IsNullOrEmpty(model.Status))
                        existing.Status = model.Status;

                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "University updated successfully",
                        id = existing.UniversityId
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        private string GenerateUniversityId()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"); // milliseconds added
            var random = Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper();

            return $"U{timestamp}{random}";
        }
        [AllowAnonymous]
        [HttpGet("collegesget")]
        public async Task<IActionResult> GetColleges()
        {
            var colleges = await _context.College
                .AsNoTracking()
                .Select(c => new
                {
                    collegeId = c.ColId,
                    collegeName = c.ColName,
                    universityName = c.UniversityName,
                    status = c.Status ?? "A"
                })
                .ToListAsync();

            return Ok(new
            {
                message = "Colleges fetched successfully",
                count = colleges.Count,
                data = colleges
            });
        }


        [HttpGet("collegegetbyid/{id}")]
        public async Task<IActionResult> GetCollegeById(string id)
        {
            var college = await _context.College
                .FirstOrDefaultAsync(x => x.ColId == id);

            if (college == null)
                return NotFound(new { message = "College not found" });

            return Ok(college);
        }
        private string GeneratecollegeId()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"); // milliseconds added
            var random = Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper();

            return $"COL{timestamp}{random}";
        }
        [AllowAnonymous]
        [HttpPost("college-save")]
        public async Task<IActionResult> CreateOrUpdateCollege([FromBody] College model)
        {
            if (model == null)
                return BadRequest(new { message = "Invalid Data" });

            // Mobile validation
            if (string.IsNullOrEmpty(model.TelNumber) ||
                !System.Text.RegularExpressions.Regex.IsMatch(model.TelNumber, @"^\d{10}$"))
                return BadRequest(new { message = "Mobile number must be exactly 10 digits." });

            // Email validation
            if (!string.IsNullOrEmpty(model.Email) &&
                !System.Text.RegularExpressions.Regex.IsMatch(model.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return BadRequest(new { message = "Invalid email format." });

            var existing = await _context.College
                .FirstOrDefaultAsync(x => x.ColId == model.ColId);

            if (existing == null)
            {
                // ✅ INSERT
                string colId = GeneratecollegeId();

                College col = new College
                {

                    CouncilId = "1",
                    ColId = colId,
                    ColName = model.ColName,
                    ColAddress = model.ColAddress,
                    District = model.District,
                    Type = model.Type,
                    PrincipalName = model.PrincipalName,
                    TelNumber = model.TelNumber,
                    UniversityName = model.UniversityName,
                    Email = model.Email,
                    Country = model.Country,
                    State = model.State,
                    CollegeCode = model.CollegeCode,
                    Status = "A",
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.Now
                };

                _context.College.Add(col);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "College Created Successfully",
                    id = col.ColId
                });
            }
            else
            {
                // ✅ UPDATE
                existing.ColName = model.ColName;
                existing.ColAddress = model.ColAddress;
                existing.District = model.District;
                existing.Type = model.Type;
                existing.PrincipalName = model.PrincipalName;
                existing.TelNumber = model.TelNumber;
                existing.UniversityName = model.UniversityName;
                existing.Email = model.Email;
                existing.UpdatedBy = model.UpdatedBy;
                existing.UpdatedOn = DateTime.Now;
                existing.Country = model.Country;
                existing.State = model.State;
                existing.CollegeCode = model.CollegeCode;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "College Updated Successfully",
                    id = existing.ColId
                });

            }

        }

    }
}