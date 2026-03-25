using GMC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GMC.Controllers
{
    [AllowAnonymous]
    public class PractitionerViewController : Controller
    {
        private readonly AppDbContext _context;

        public PractitionerViewController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ VIEW PAGE
        public IActionResult Index(string pid)
        {
            ViewBag.PID = pid;
            return View();
        }

        // =========================================
        // ✅ BASIC DETAILS API
        // =========================================
        [HttpGet("practitioner/basic/{pid}")]
        public async Task<IActionResult> GetBasic(string pid)
        {
            var data = await (
                from p in _context.Practitioners
                join rf in _context.RegistrationFor on p.RegistrationFor equals rf.RegId
                join g in _context.GenderMaster on p.Gender equals g.GenderId
                join n in _context.NationalityMaster on p.Nationality equals n.NationalityId
                join b in _context.BloodGroupTypeMaster on p.BloodGroup equals b.BloodGroupCode
                where p.PractitionerID == pid && p.status == "A"
                select new
                {
                    p.PractitionerID,
                    p.RegistrationNo,
                    RegForName = rf.RegForName,
                    p.Name,
                    Gender = g.GenderName,
                    p.BirthDate,
                    p.BirthPlace,
                    Nationality = n.Nationality,
                    BloodGroup = b.BloodGroupDescription,
                    p.MobileNumber,
                    p.EmailID,
                    Photo = string.IsNullOrEmpty(p.photo) ? "" : "/Upload/PractitionerPhotos/1/2/2/" + p.photo,
                    Sign = string.IsNullOrEmpty(p.practsign) ? "" : "/Upload/PractitionerPhotos/1/2/2/" + p.practsign,
                    thumb = string.IsNullOrEmpty(p.thumb) ? "" : "/Upload/PractitionerPhotos/1/2/2/" + p.thumb,

                }
            ).FirstOrDefaultAsync();

            return Ok(data);
        }

        // =========================================
        // ✅ ADDRESS API
        // =========================================
        [HttpGet("practitioner/address/{pid}")]
        public async Task<IActionResult> GetAddress(string pid)
        {
            var data = await (
                from a in _context.Address

                join d in _context.DistrictMaster
                    on a.District equals d.DistrictId into dJoin
                from d in dJoin.DefaultIfEmpty()

                join s in _context.StateMaster
                    on a.StateId equals s.StateId into sJoin
                from s in sJoin.DefaultIfEmpty()

                join c in _context.CountryMaster
                    on a.Country equals c.CountryId into cJoin
                from c in cJoin.DefaultIfEmpty()

                where a.ClientID == pid

                select new
                {
                    typeaddress = a.AddressType == "P" ? "Permanent" : "Residencial",
                    a.Address1,
                    a.Address2,
                    District = d != null ? d.DistrictName : "",
                    State = s != null ? s.StateName : "",
                    Country = c != null ? c.CountryName : "",
                    a.Zip,

                    // ✅ Full Address (ready for UI)
                    FullAddress =
                        a.Address1 + ", " +
                        a.Address2 + ", " +
                        (d != null ? d.DistrictName + ", " : "") +
                        (s != null ? s.StateName + ", " : "") +
                        (c != null ? c.CountryName + ", " : "") +
                        a.Zip
                }
            ).ToListAsync();

            return Ok(data);
        }
        // =========================================
        // ✅ EDUCATION API
        // =========================================
        [HttpGet("practitioner/education/{pid}")]
        public async Task<IActionResult> GetEducation(string pid)
        {
            if (string.IsNullOrEmpty(pid))
                return BadRequest("PID is required");

            var data = await (
                from e in _context.EducationInfo

                    // 🔹 Course Join (Optional)
                join c in _context.CourseMaster
                    on e.EducationName equals c.CourseId into cJoin
                from c in cJoin.DefaultIfEmpty()
                    // col join
                join cl in _context.College
                  on e.CollegeID equals cl.ColId into clJoin
                from cl in clJoin.DefaultIfEmpty()


                    // 🔹 University Join (Optional)
                join u in _context.University
                    on e.UniversityId equals u.UniversityId into uJoin
                from u in uJoin.DefaultIfEmpty()

                where e.PractitionerID == pid

                select new
                {
                    // Raw fields
                    e.EducationName,
                    e.CollegeID,
                    e.YearOfPassing,

                    // Joined fields
                    Course = c != null ? e.CollegeID : "",
                    University = u != null ? u.UniversityName : "",

                    // 🔥 Full display (UI ready)
                    FullEducation =
                        (e.EducationName ?? "") +
                        (string.IsNullOrEmpty(cl.ColName) ? "" : " - " + cl.ColName) +
                        (u != null ? ", " + u.UniversityName : "") +
                        (string.IsNullOrEmpty(e.YearOfPassing.ToString()) ? "" : " (" + e.YearOfPassing + ")")
                }

            ).ToListAsync();

            if (data == null || data.Count == 0)
                return NotFound("Education not found");

            return Ok(data);
        }
    }
}