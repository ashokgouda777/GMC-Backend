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

        [HttpPost("statemasterspost")]
        public async Task<IActionResult> CreateStateMaster([FromBody] StateMaster model)
        {
            try
            {
                if (model == null) return BadRequest(new { message = "Invalid state model" });

                if (string.IsNullOrEmpty(model.StateName))
                {
                    return BadRequest(new { message = "State name is required." });
                }

                if (string.IsNullOrEmpty(model.CountryId))
                {
                    return BadRequest(new { message = "Country selection is required." });
                }

                // Robust ID generation
                if (string.IsNullOrEmpty(model.StateId))
                {
                    var ids = await _context.StateMaster
                        .Select(s => s.StateId)
                        .ToListAsync();


                    int maxId = 0;
                    foreach (var id in ids)
                    {
                        if (int.TryParse(id, out int idInt))
                        {
                            if (idInt > maxId) maxId = idInt;
                        }
                    }
                    model.StateId = (maxId + 1).ToString();
                }

                model.CreatedOn = DateTime.Now;
                model.Active = string.IsNullOrEmpty(model.Active) ? "Yes" : model.Active;

                _context.StateMaster.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "State saved successfully", data = model });
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? $"; Inner: {ex.InnerException.Message}" : "";
                return StatusCode(500, new { message = "Backend Error: " + ex.Message + innerMsg });
            }
        }

        [HttpPut("statemastersupdate/{id}")]
        public async Task<IActionResult> UpdateStateMaster(string id, [FromBody] StateMaster model)
        {
            try
            {
                var state = await _context.StateMaster.FindAsync(id);

                if (state == null) return NotFound(new { message = "State not found" });

                state.StateName = model.StateName;
                state.CountryId = model.CountryId;
                state.Active = model.Active;
                state.UpdatedOn = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "State updated successfully", data = state });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("statemastersdelete/{id}")]
        public async Task<IActionResult> DeleteStateMaster(string id)
        {
            try
            {
                var state = await _context.StateMaster.FindAsync(id);

                if (state == null) return NotFound(new { message = "State not found" });

                _context.StateMaster.Remove(state);
                await _context.SaveChangesAsync();

                return Ok(new { message = "State deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
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
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
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
        [HttpPost("countrymasterspost")]
        public async Task<IActionResult> CreateCountryMaster([FromBody] CountryMaster model)
        {
            try
            {
                if (model == null) return BadRequest(new { message = "Invalid country model" });

                var normalizedName = model.CountryName?.Trim();
                if (string.IsNullOrEmpty(normalizedName))
                {
                    return BadRequest(new { message = "Country name is required." });
                }

                var exists = await _context.CountryMaster.AnyAsync(c => c.CountryName != null && c.CountryName.ToLower() == normalizedName.ToLower());

                if (exists)
                {
                    return BadRequest(new { message = "Country already exists." });
                }


                var lastEntry = await _context.CountryMaster
                    .OrderByDescending(c => c.id)
                    .FirstOrDefaultAsync();

                int nextVal = (lastEntry?.id ?? 0) + 1;
                model.CountryId = nextVal.ToString();

                model.Active = string.IsNullOrEmpty(model.Active) ? "Yes" : model.Active;
                model.CreatedOn = DateTime.Now;

                _context.CountryMaster.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Country saved successfully", data = model });
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? $"; Inner: {ex.InnerException.Message}" : "";
                return StatusCode(500, new { message = "Backend Error: " + ex.Message + innerMsg });
            }
        }


        [HttpDelete("countrymastersdelete/{id}")]
        public async Task<IActionResult> DeleteCountryMaster(int id)
        {
            var country = await _context.CountryMaster.FindAsync(id);

            if (country == null)
                return NotFound(new { message = "Country not found" });

            // Soft delete → make inactive
            country.Active = "No";
            country.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Country deactivated successfully" });
        }
        [HttpPut("countrymastersupdate/{id}")]
        public async Task<IActionResult> UpdateCountryMaster(int id, [FromBody] CountryMaster model)
        {
            if (id != model.id)
                return BadRequest(new { message = "Country ID mismatch" });

            var country = await _context.CountryMaster.FindAsync(id);

            if (country == null)
                return NotFound(new { message = "Country not found" });


            country.CountryName = model.CountryName;
            country.Active = model.Active;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Country updated successfully",
                data = country
            });
        }

       /* [HttpGet("districtmastersget")]
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
                            Active = dc.d.Active ?? "No"
                        })
                    .ToListAsync();

                return Ok(districts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }*/

     /*   [HttpPost("districtmasterspost")]
        public async Task<IActionResult> CreateDistrictMaster([FromBody] DistrictMaster model)
        {
            try
            {
                if (model == null) return BadRequest(new { message = "Invalid district model" });

                if (string.IsNullOrEmpty(model.DistrictId))
                {
                    var ids = await _context.DistrictMaster
                        .Select(d => d.DistrictId)
                        .ToListAsync();

                    int maxId = 0;
                    foreach (var id in ids)
                    {
                        if (int.TryParse(id, out int idInt))
                        {
                            if (idInt > maxId) maxId = idInt;
                        }
                    }
                    model.DistrictId = (maxId + 1).ToString();
                }

                model.CreatedOn = DateTime.Now;
                model.Active = string.IsNullOrEmpty(model.Active) ? "Yes" : model.Active;

                _context.DistrictMaster.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "District saved successfully", data = model });
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? $"; Inner: {ex.InnerException.Message}" : "";
                return StatusCode(500, new { message = "Backend Error: " + ex.Message + innerMsg });
            }
        }*/
/*
        [HttpPut("districtmastersupdate/{id}")]
        public async Task<IActionResult> UpdateDistrictMaster(string id, [FromBody] DistrictMaster model)
        {
            try
            {
                var district = await _context.DistrictMaster.FirstOrDefaultAsync(d => d.DistrictId == id);

                if (district == null) return NotFound("District not found");

                district.DistrictName = model.DistrictName;
                district.StateId = model.StateId;
                district.CountryId = model.CountryId;
                district.Active = model.Active;
                district.UpdatedOn = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "District updated successfully", data = district });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }*/

        [HttpDelete("districtmastersdelete/{id}")]
        public async Task<IActionResult> DeleteDistrictMaster(string id)
        {
            try
            {
                var district = await _context.DistrictMaster.FirstOrDefaultAsync(d => d.DistrictId == id);

                if (district == null) return NotFound("District not found");

                _context.DistrictMaster.Remove(district);
                await _context.SaveChangesAsync();

                return Ok(new { message = "District deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }
        [HttpGet("coursemastersget")]
        public async Task<IActionResult> GetCourseMaster()
        {
            try
            {
                var courses = await _context.CourseMaster
                    .AsNoTracking()
                    .ToListAsync();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

      /*  [HttpPost("coursemasterspost")]
        public async Task<IActionResult> CreateCourseMaster([FromBody] CourseMaster model)
        {
            try
            {
                if (model == null) return BadRequest(new { message = "Invalid course model" });

                if (string.IsNullOrEmpty(model.CourseName))
                {
                    return BadRequest(new { message = "Course name is required." });
                }

                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                }

                model.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                model.Status = string.IsNullOrEmpty(model.Status) ? "Active" : model.Status;

                _context.CourseMaster.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Course saved successfully", data = model });
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? $"; Inner: {ex.InnerException.Message}" : "";
                return StatusCode(500, new { message = "Backend Error: " + ex.Message + innerMsg });
            }
        }*/

     /*   [HttpPut("coursemastersupdate/{id}")]
        public async Task<IActionResult> UpdateCourseMaster(string id, [FromBody] CourseMaster model)
        {
            try
            {
                var course = await _context.CourseMaster.FindAsync(id);
                if (course == null) return NotFound("Course not found");

                course.CourseName = model.CourseName;
                course.ShortcutCode = model.ShortcutCode;
                course.Nomenclature = model.Nomenclature;
                course.AdditionalDegree = model.AdditionalDegree;
                course.Status = model.Status;
                course.Active = model.Active;


                course.UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                await _context.SaveChangesAsync();
                return Ok(new { message = "Course updated successfully", data = course });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }*/

        [HttpDelete("coursemastersdelete/{id}")]
        public async Task<IActionResult> DeleteCourseMaster(string id)
        {
            try
            {
                var course = await _context.CourseMaster.FindAsync(id);
                if (course == null) return NotFound("Course not found");

                _context.CourseMaster.Remove(course);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Course deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    /*    [HttpGet("mdssubjectmastersget")]
        public async Task<IActionResult> GetMdsSubjectMaster()
        {
            try
            {
                var subjects = await _context.MdsSubjectMasters
                    .AsNoTracking()
                    .ToListAsync();
                return Ok(subjects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }*/

        /*[HttpPost("mdssubjectmasterspost")]
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



                _context.MdsSubjectMasters.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Subject saved successfully", data = model });
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? $"; Inner: {ex.InnerException.Message}" : "";
                return StatusCode(500, new { message = "Backend Error: " + ex.Message + innerMsg });
            }
        }*/

        /*[HttpPut("mdssubjectmastersupdate/{id}")]
        public async Task<IActionResult> UpdateMdsSubjectMaster(string id, [FromBody] MdsSubjectMaster model)
        {
            try
            {
                var subject = await _context.MdsSubjectMasters.FirstOrDefaultAsync(s => s.Sub_code == id);
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
        }*/

       /* [HttpDelete("mdssubjectmastersdelete/{id}")]
        public async Task<IActionResult> DeleteMdsSubjectMaster(string id)
        {
            try
            {
                var subject = await _context.MdsSubjectMasters.FirstOrDefaultAsync(s => s.Sub_code == id);
                if (subject == null) return NotFound("Subject not found");

                _context.MdsSubjectMasters.Remove(subject);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Subject deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }*/
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
        [HttpPost("universitypost")]
        public async Task<ActionResult<University>> PostUniversity([FromBody] University university)
        {
            if (university == null)
                return BadRequest("Invalid data");

            if (string.IsNullOrEmpty(university.UniversityCode))
                return BadRequest("University Code is required (max 5 characters).");

            if (university.UniversityCode.Length > 5)
                return BadRequest("University Code cannot exceed 5 characters.");

            university.UniversityId = GenerateUniversityId();
            university.Status = "Active";
            university.CreatedOn = DateTime.Now;

            _context.University.Add(university);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUniversity),
                new { id = university.UniversityId }, university);
        }



        [HttpPut("universityupdate/{id}")]
        public async Task<IActionResult> PutUniversity(string id, [FromBody] University updatedUniversity)
        {
            var university = await _context.University.FindAsync(id);
            if (university == null)
                return NotFound();

            university.UniversityName = updatedUniversity.UniversityName;
            university.UniversityCode = updatedUniversity.UniversityCode;
            university.UpdatedBy = updatedUniversity.UpdatedBy;
            university.UpdatedOn = DateTime.Now;

            if (!string.IsNullOrEmpty(updatedUniversity.Status))
                university.Status = updatedUniversity.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("universitydelete/{id}")]
        public async Task<IActionResult> DeleteUniversity(string id)
        {
            var university = await _context.University.FindAsync(id);

            if (university == null)
                return NotFound();


            university.Status = "D";
            university.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        private string GenerateUniversityId()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"); // milliseconds added
            var random = Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper();

            return $"U{timestamp}{random}";
        }
        [HttpPost("collegecreate")]
        public async Task<IActionResult> CreateCollege([FromBody] College model)
        {
            if (model == null)
                return BadRequest(new { message = "Invalid Data" });


            if (string.IsNullOrEmpty(model.TelNumber) ||
                !System.Text.RegularExpressions.Regex.IsMatch(model.TelNumber, @"^\d{10}$"))
                return BadRequest(new { message = "Mobile number must be exactly 10 digits." });


            if (!string.IsNullOrEmpty(model.Email) &&
                !System.Text.RegularExpressions.Regex.IsMatch(model.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return BadRequest(new { message = "Invalid email format." });

            model.ColId = "COL" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

            model.CreatedOn = DateTime.Now;
            model.UpdatedOn = null;


            model.Status = string.IsNullOrEmpty(model.Status) ? "A" : model.Status;

            await _context.College.AddAsync(model);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "College Created Successfully",
                data = model
            });
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet("collegesget")]
        public async Task<IActionResult> GetColleges()
        {
            var colleges = await _context.College.ToListAsync();
            return Ok(colleges);
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet("collegegetbyid/{id}")]
        public async Task<IActionResult> GetCollegeById(string id)
        {
            var college = await _context.College
                .FirstOrDefaultAsync(x => x.ColId == id);

            if (college == null)
                return NotFound(new { message = "College not found" });

            return Ok(college);
        }
        [HttpPut("collegeupdate/{id}")]
        public async Task<IActionResult> UpdateCollege(string id, [FromBody] College model)
        {
            var existing = await _context.College
                .FirstOrDefaultAsync(x => x.ColId == id);

            if (existing == null)
                return NotFound(new { message = "College not found" });

            if (string.IsNullOrEmpty(model.TelNumber))
                return BadRequest(new { message = "Mobile number is required." });

            if (!System.Text.RegularExpressions.Regex.IsMatch(model.TelNumber, @"^\d{10}$"))
                return BadRequest(new { message = "Mobile number must be exactly 10 digits." });



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
            existing.Status = model.Status ?? existing.Status;
            existing.Country = model.Country;
            existing.State = model.State;
            existing.CollegeCode = model.CollegeCode;

            await _context.SaveChangesAsync();

            return Ok(new

            {
                message = "College Updated Successfully",
                data = existing
            });
        }
        [HttpDelete("collegedelete/{id}")]
        public async Task<IActionResult> DeleteCollege(string id)
        {
            var college = await _context.College
                .FirstOrDefaultAsync(x => x.ColId == id);

            if (college == null)
                return NotFound(new { message = "College not found" });

            college.Status = "Inactive";   // I = Inactive
            college.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "College set to Inactive",
                status = college.Status
            });







        }





    }



}
