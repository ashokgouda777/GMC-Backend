using GMC.Data;
using GMC.Model;
using GMC.Model.GMC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GMC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EducationController(AppDbContext context)
        {
            _context = context;
        }

        // POST : Insert or Update
        [HttpPost("save")]
        public async Task<IActionResult> SaveEducation([FromBody] AddEducationInfo model)
        {
            if (model == null)
                return BadRequest("Invalid data");

            // INSERT
            if (string.IsNullOrEmpty(model.EducationID))
            {
                string eid = await GenerateEIdAsync();

                // DTO → Entity mapping
                var entity = new EducationInfo
                {   CountryId = "1",
                    StateId="1",
                    CouncilId="1",
                    EducationID = eid,
                    PractitionerID = model.PractitionerID,
                    EducationName = model.EducationName,
                    YearOfPassing = model.YearOfPassing,
                    CollegeID = model.CollegeID,
                    UniversityId = model.UniversityId,
                    Subject = model.Subject,
                    MonthOfPassing = model.MonthOfPassing,
                    SubCode = model.SubCode,
                    CompletedAt = model.CompletedAt,
                    CertificateNo = model.CertificateNo,
                    CertificateDate = model.CertificateDate,
                    ADSerialno = model.ADSerialno,
                    IsIssued="yes",
                    CreatedBy = model.CreatedBy,
                    CreatedOn = DateTime.Now,
                    Active="A"
                };

                _context.EducationInfo.Add(entity);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Inserted successfully",
                    EducationID = eid
                });
            }

            // UPDATE
            var existing = await _context.EducationInfo
                .FirstOrDefaultAsync(x => x.EducationID == model.EducationID);

            if (existing == null)
                return NotFound("Record not found");

            // Update entity from DTO
            
            existing.PractitionerID = model.PractitionerID;
            existing.EducationName = model.EducationName;
            existing.YearOfPassing = model.YearOfPassing;
            existing.CollegeID = model.CollegeID;
            existing.UniversityId = model.UniversityId;
            existing.Subject = model.Subject;
            existing.MonthOfPassing = model.MonthOfPassing;
            existing.SubCode = model.SubCode;
            existing.CompletedAt = model.CompletedAt;
            existing.CertificateNo = model.CertificateNo;
            existing.CertificateDate = model.CertificateDate;
            existing.ADSerialno = model.ADSerialno;
            existing.UpdatedBy = model.UpdatedBy;
            existing.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Updated successfully",
                EducationID = existing.EducationID
            });
        }



        private async Task<string> GenerateEIdAsync()
        {
            string id;
            var rnd = new Random();

            do
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                int random = rnd.Next(100, 999);
                id = $"E{timestamp}{random}";

            } while (await _context.Users.AnyAsync(x => x.userId == id));

            return id;
        }




        [AllowAnonymous]
        [HttpGet("get/{practitionerId}")]
        public async Task<IActionResult> GetEducationInfo(string practitionerId)
        {
                    var data = await (
             from edu in _context.EducationInfo

                 // College LEFT JOIN
             join c in _context.College
                 on edu.CollegeID equals c.ColId into cGroup
             from c in cGroup.DefaultIfEmpty()

                 // University LEFT JOIN
             join u in _context.University
                 on edu.UniversityId equals u.UniversityId into uGroup
             from u in uGroup.DefaultIfEmpty()


             where edu.PractitionerID == practitionerId && edu.Active=="A"

                select new
                {
                    edu.EducationID,
                    edu.PractitionerID,               
                    edu.EducationName,
                    edu.YearOfPassing,
                    edu.CollegeID,

                    c.ColName,
                    u.UniversityName,
                    edu.Subject,
                    edu.MonthOfPassing,
                    edu.SubCode,
                    edu.CompletedAt,
                    edu.CertificateNo,
                    edu.CertificateDate,
                    edu.ADSerialno,
                    edu.CreatedBy,
                    edu.CreatedOn,
                    edu.UpdatedBy,
                    edu.UpdatedOn
                }
            ).ToListAsync();

            if (data.Count == 0)
                return NotFound("No records found");

            return Ok(data);
        }




        [HttpPost("deleteeducation")]
        public async Task<IActionResult> deleteeducation(string educationid,string UpdatedBy)
        {
            if (string.IsNullOrEmpty(educationid))
                return BadRequest("Invalid data");

           

            // UPDATE
            var existing = await _context.EducationInfo
                .FirstOrDefaultAsync(x => x.EducationID == educationid);

            if (existing == null)
                return NotFound("Record not found");

            // Update entity from DTO

            existing.Active ="N";
            existing.UpdatedBy = UpdatedBy;
            existing.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Updated successfully",
                EducationID = existing.EducationID
            });
        }



    }
}
