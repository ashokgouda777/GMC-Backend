using GMC.Data;
using GMC.Model;
using GMC.Model.GMC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GMC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PractitionersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        
        public PractitionersController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ✅ CREATE PRACTITIONER

        [HttpPost("create")]
        public async Task<IActionResult> CreatePractitioner([FromBody] PractitionerCreate dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🔍 Check Mobile Exists
            var mobileExists = await _context.Practitioners
                .AnyAsync(x => x.MobileNumber == dto.MobileNumber);

          
            if (mobileExists)
                return BadRequest("Mobile number already registered");


            string registrationno;
            DateTime date;

            if(!string.IsNullOrEmpty(dto.RegistrationNo))
            {
                var existregno = await _context.Practitioners
               .AnyAsync(x => x.RegistrationNo == dto.RegistrationNo);
                if (existregno)
                {
                    return BadRequest("Registration number already exist");
                }
                else
                {
                    registrationno = dto.RegistrationNo;
                    date = dto.RegistrationDate ?? DateTime.Now;
                }
                   
            }
            else
            {
                string regno;

                var regForParam = new SqlParameter("@RegistrationFor", dto.RegistrationFor);

                var nextNo = await _context.Database
                    .SqlQueryRaw<int>(@"
        SELECT 
            CASE 
                WHEN ISNULL(MAX(TRY_CAST(RegistrationNo AS INT)),0) < 1000 
                THEN 1000
                ELSE ISNULL(MAX(TRY_CAST(RegistrationNo AS INT)),0) + 1
            END AS Value
        FROM tbl_practitioner
        WHERE RegistrationFor = @RegistrationFor", regForParam)
                    .FirstAsync();

                registrationno = nextNo.ToString();
                date = DateTime.Now;
            }
            // 🔐 Generate password hash ONCE

            string bdte = dto.BirthDate?.ToString("dd-MM-yyyy");
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(bdte);

            // 🔑 Generate Practitioner UserId
            string practitionerid = await GeneratePractitionerUserIdAsync();

            // 🧾 Begin Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var practitioner2 = new Practitioner
                {
                    PractitionerID = practitionerid,
                    CountryId = "1",
                    StateId = "1",
                    CouncilId = "1",
                    Name = dto.Name,
                    Password = hashedPassword,
                    RegistrationFor = dto.RegistrationFor,
                    RegistrationNo = registrationno,
                    RegistrationDate = date,
                    Title = dto.Title,
                    Gender = dto.Gender,
                    BloodGroup = dto.BloodGroup,
                    ChangeOfName = dto.ChangeOfName,
                    SpouseName = dto.SpouseName,
                    BirthDate = dto.BirthDate,
                    BirthPlace = dto.BirthPlace,
                    Nationality = dto.Nationality,
                    Vote = dto.Vote,
                    EmailID = dto.EmailID,
                    MobileNumber = dto.MobileNumber,
                    CreatedOn = DateTime.Now,
                    CreatedBy = dto.CreatedBy,
                    status="A"
                };

                _context.Practitioners.Add(practitioner2);
                await _context.SaveChangesAsync();

                var user = new User
                {
                    CouncilId = "1",
                    CountryId = "1",
                    StateId = "1",
                    Name = dto.Name,
                    UserName = dto.MobileNumber,
                    MobileNo = dto.MobileNumber,
                    Password = bdte,
                    userId = practitionerid,
                    Role_Id = "2",
                    CreatedOn = DateTime.Now,
                    CreatedBy = dto.CreatedBy
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Practitioner created successfully",
                    practitionerId = practitioner2.PractitionerID
                 
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPut("update/{pid}")]
        public async Task<IActionResult> UpdatePractitioner(string pid, [FromBody] PractitionerCreate dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🔍 Find existing practitioner
            var practitioner = await _context.Practitioners
                .FirstOrDefaultAsync(x => x.PractitionerID == pid);

            if (practitioner == null)
                return NotFound("Practitioner not found");

            // 🔍 Check Mobile Exists (Exclude current record)
            var mobileExists = await _context.Practitioners
                .AnyAsync(x => x.MobileNumber == dto.MobileNumber
                            && x.PractitionerID != pid);


            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 🔄 Update fields
                practitioner.Name = dto.Name;
                if (!string.IsNullOrEmpty(dto.RegistrationFor))
                {
                    practitioner.RegistrationFor = dto.RegistrationFor;
                }
                if (!string.IsNullOrEmpty(dto.RegistrationNo))
                {
                    practitioner.RegistrationNo = dto.RegistrationNo;
                    practitioner.RegistrationDate = dto.RegistrationDate;
                }

                practitioner.Title = dto.Title;
                practitioner.Gender = dto.Gender;
                practitioner.BloodGroup = dto.BloodGroup;
                practitioner.ChangeOfName = dto.ChangeOfName;
                practitioner.SpouseName = dto.SpouseName;
                practitioner.BirthDate = dto.BirthDate;
                practitioner.BirthPlace = dto.BirthPlace;
                practitioner.Nationality = dto.Nationality;
                practitioner.Vote = dto.Vote;
                practitioner.EmailID = dto.EmailID;
                practitioner.MobileNumber = dto.MobileNumber;
                practitioner.status = "A";
                practitioner.UpdatedBy = dto.CreatedBy;
                practitioner.UpdatedOn = DateTime.Now;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Practitioner updated successfully",
                    practitionerId = practitioner.PractitionerID
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }


        // 🔑 Generate Unique Practitioner ID
        private async Task<string> GeneratePractitionerUserIdAsync()
        {
            string id;
            var rnd = new Random();

            do
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                int random = rnd.Next(100, 999);
                id = $"P{timestamp}{random}";

            } while (await _context.Users.AnyAsync(x => x.userId == id));

            return id;
        }




        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetPractitionerlist(
       string registartionfor,
       int pageNumber = 1,
       int pageSize = 10)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.Practitioners
                .Where(x =>  x.RegistrationFor == registartionfor);

            // Total count for pagination
            var totalRecords = await query.CountAsync();

            var prclist = await query
                .OrderByDescending(x => x.RegistrationNo)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new
                {
                    PractitionerID = x.PractitionerID,
                    CountryId = "1",
                    StateId = "1",
                    CouncilId = "1",
                    Name = x.Name,
                    RegistrationFor = x.RegistrationFor,
                    RegistrationNo = x.RegistrationNo,
                    RegistrationDate = x.RegistrationDate,
                    Title = x.Title,
                    Gender = x.Gender,
                    BloodGroup = x.BloodGroup,
                    ChangeOfName = x.ChangeOfName,
                    SpouseName = x.SpouseName,
                    BirthDate = x.BirthDate,
                    BirthPlace = x.BirthPlace,
                    Nationality = x.Nationality,
                    Vote = x.Vote,
                    EmailID = x.EmailID,
                    MobileNumber = x.MobileNumber,
                    CreatedOn = DateTime.Now,
                    CreatedBy = x.CreatedBy,
                    status = x.status
                })
                .ToListAsync();

            var result = new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                Data = prclist
            };

            return Ok(result);
        }




        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchPractitioners(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return BadRequest("Search text is required");
            }

            searchText = searchText.Trim();

            var data = await _context.Practitioners
                .Where(x => 
                       ((x.Name ?? "").Contains(searchText) ||
                        (x.RegistrationNo ?? "").Contains(searchText) ||
                        (x.MobileNumber ?? "").Contains(searchText) ||
                        (x.EmailID ?? "").Contains(searchText)))
                .OrderByDescending(x => x.RegistrationNo)
                .Select(x => new
                {
                    PractitionerID = x.PractitionerID,
                    CountryId = "1",
                    StateId = "1",
                    CouncilId = "1",
                    Name = x.Name,
                    RegistrationFor = x.RegistrationFor,
                    RegistrationNo = x.RegistrationNo,
                    RegistrationDate = x.RegistrationDate,
                    Title = x.Title,
                    Gender = x.Gender,
                    BloodGroup = x.BloodGroup,
                    ChangeOfName = x.ChangeOfName,
                    SpouseName = x.SpouseName,
                    BirthDate = x.BirthDate,
                    BirthPlace = x.BirthPlace,
                    Nationality = x.Nationality,
                    Vote = x.Vote,
                    EmailID = x.EmailID,
                    MobileNumber = x.MobileNumber,
                    CreatedOn = DateTime.Now,
                    CreatedBy = x.CreatedBy,
                    status = x.status
                })
                .ToListAsync();

            return Ok(data);
        }




        [AllowAnonymous]
        [HttpGet("practitioner/{id}")]
        public async Task<IActionResult> Practitionerinfo(string id)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}/Upload/PractitionerPhotos/1/2/2";
            if (string.IsNullOrEmpty(id))
                return BadRequest("Practitioner ID is required");

            var prc = await (
              from p in _context.Practitioners

                 // Nationality LEFT JOIN
                 join c in _context.NationalityMaster
                     on p.Nationality equals c.NationalityId into cGroup
                 from c in cGroup.DefaultIfEmpty()

                     // Gender LEFT JOIN
                 join g in _context.GenderMaster
                     on p.Gender equals g.GenderId into gGroup
                 from g in gGroup.DefaultIfEmpty()

                     // Eligibility LEFT JOIN
                 join v in _context.PractitionerEligiblity
                     on Convert.ToInt32(p.Vote) equals v.EligibiltyId into vGroup
                 from v in vGroup.DefaultIfEmpty()

                     // Blood Group LEFT JOIN
                 join b in _context.BloodGroupTypeMaster
                     on p.BloodGroup equals b.BloodGroupCode into bGroup
                 from b in bGroup.DefaultIfEmpty()

                     // RegistrationFor LEFT JOIN
                 join r in _context.RegistrationFor
                     on p.RegistrationFor equals r.RegId into rGroup
                 from r in rGroup.DefaultIfEmpty()

                 where p.PractitionerID == id
                 select new
               {
                   PractitionerID = p.PractitionerID,
                   CountryId = "1",
                   StateId = "1",
                   CouncilId = "1",
                   Name = p.Name,
                   RegistrationFor = r.RegForName,
                   RegistrationNo = p.RegistrationNo,
                   RegistrationDate = p.RegistrationDate,
                   Title = p.Title,
                   Gender = g.GenderName,
                   BloodGroup = b.BloodGroupDescription,
                   ChangeOfName = p.ChangeOfName,
                   SpouseName =p.SpouseName,
                   BirthDate = p.BirthDate,
                   BirthPlace = p.BirthPlace,
                   Nationality = c.Nationality,
                   Vote = v.Eligibilty,
                   EmailID = p.EmailID,
                   MobileNumber = p.MobileNumber,
                   CreatedOn = p.CreatedOn,
                   CreatedBy = p.CreatedBy,
                   photo= string.IsNullOrEmpty(p.photo) ? "" : baseUrl+"/"+p.photo,
                   thumb= string.IsNullOrEmpty(p.thumb) ? "" : baseUrl + "/" + p.thumb,
                   sign= string.IsNullOrEmpty(p.practsign) ? "" : baseUrl + "/" + p.practsign,
                   barcode = string.IsNullOrEmpty(p.barcode) ? "" : baseUrl + "/" + p.barcode,
               }
               ).FirstOrDefaultAsync();


            if (prc == null)
                return NotFound("Invalid Practitioner ID");

            return Ok(prc);
        }



        [HttpPost("upload")]
        public async Task<IActionResult> UploadFilepp(IFormFile file, string pid, string type)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            var practitioner = await _context.Practitioners
            .FirstOrDefaultAsync(x => x.PractitionerID == pid);

            if (practitioner == null)
                return NotFound("Practitioner not found.");

            string uploadPath = Path.Combine(_environment.ContentRootPath, "wwwroot/Upload/PractitionerPhotos/1/2/2");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            if (type == "photo")
            {
                string fileName = pid + "_photo" + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }



                practitioner.photo = fileName;

                await _context.SaveChangesAsync();

            }
            if (type == "sign")
            {
                string fileName = pid + "_sign" + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }



                practitioner.practsign = fileName;

                await _context.SaveChangesAsync();


            }
            if (type == "thumb")
            {
                string fileName = pid + "_thumb" + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }



                practitioner.thumb = fileName;

                await _context.SaveChangesAsync();


            }
            return Ok(new
            {
                message = type + " uploaded successfully",
                result = "success"
            });
        }

        // ✅ Generate Barcode (only pid parameter)
        [AllowAnonymous]
        [HttpGet("generatebarcode")]
        public async Task<IActionResult> GenerateBarcode(string pid)
        {
            if (string.IsNullOrWhiteSpace(pid))
                return BadRequest("PID is required");

            // ✅ Generate Full URL for QR
            string qrContent = $"https://localhost:44377/PractitionerView?pid={pid}";

            // ✅ Get Practitioner
            var practitioner = await _context.Practitioners
                .FirstOrDefaultAsync(x => x.PractitionerID == pid);

            if (practitioner == null)
                return NotFound("Practitioner not found");

            // ✅ Create QR Code
            var writer = new ZXing.SkiaSharp.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 500,
                    Height = 500,
                    Margin = 2
                }
            };

            using var bitmap = writer.Write(qrContent);

            // ✅ Dynamic Folder Path (better than hardcoding 1/2/2)
            string uploadPath = Path.Combine(_environment.ContentRootPath, "wwwroot/Upload/PractitionerPhotos/1/2/2");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string fileName = $"{pid}_barcode.png";
            string filePath = Path.Combine(uploadPath, fileName);

            // ✅ Save QR Image
            using (var image = SkiaSharp.SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SkiaSharp.SKEncodedImageFormat.Png, 100))
            using (var stream = System.IO.File.Open(filePath, FileMode.Create))
            {
                data.SaveTo(stream);
            }

            // ✅ Update Database
            practitioner.barcode = fileName;
            _context.Practitioners.Update(practitioner);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "QR Code generated successfully",
                ScanUrl = qrContent,
                FileName = fileName
            });
        }

        // ✅ Clean URL endpoint
        [AllowAnonymous]
        [HttpGet("/practitionerdetailes")]
        public IActionResult GetBarcode(string pid)
        {
            if (string.IsNullOrEmpty(pid))
                return BadRequest("PID is required");

            string filePath = Path.Combine(
                _environment.WebRootPath,
                "Upload",
                "Barcodes",
                pid + ".png"
            );

            if (!System.IO.File.Exists(filePath))
                return NotFound("Barcode not found");

            var bytes = System.IO.File.ReadAllBytes(filePath);

            return File(bytes, "image/png");
        }


        [AllowAnonymous]
        [HttpPost("makeperment")]
        public async Task<IActionResult> MakePerment(string practitionerId)
        {
            if (string.IsNullOrEmpty(practitionerId))
                return BadRequest("PractitionerId is required");

            var practitioner = await _context.Practitioners
                .FirstOrDefaultAsync(x => x.PractitionerID == practitionerId);
            string regno;
            string prcarchive = practitioner.RegistrationNo;

            var regForParam = new SqlParameter("@RegistrationFor", "1");

            var nextNo = await _context.Database
                .SqlQueryRaw<int>(@"
        SELECT 
            CASE 
                WHEN ISNULL(MAX(TRY_CAST(RegistrationNo AS INT)),0) < 1000 
                THEN 1000
                ELSE ISNULL(MAX(TRY_CAST(RegistrationNo AS INT)),0) + 1
            END AS Value
        FROM tbl_practitioner
        WHERE RegistrationFor = @RegistrationFor", regForParam)
                .FirstAsync();

           string registrationno = nextNo.ToString();
          
            if (practitioner == null)
                return NotFound("Practitioner not found");


            // ✅ Update RegistrationFor = 1 (Permanent)
            practitioner.RegistrationFor = "1";
            practitioner.RegistrationNo = registrationno;
            practitioner.PrcNoArchive = prcarchive;
            // ✅ Optional: update date
            practitioner.RegistrationDate = DateTime.Now;
            practitioner.UpdatedBy = "Admin";
            practitioner.UpdatedOn = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registration updated to permanent successfully",
                practitionerId = practitionerId
            });
        }


      

        [AllowAnonymous]
        [HttpPost("uploaddocument")]
        public async Task<IActionResult> UploadDocument(
        [FromQuery] string pid, string DocumentType,string DocumentName,
        IFormFile file)
        {
            if (string.IsNullOrEmpty(pid) ||  file == null || file.Length == 0)
                return BadRequest("Invalid details or file payload.");

            var practitioner = await _context.Practitioners
                .FirstOrDefaultAsync(x => x.PractitionerID == pid);

            if (practitioner == null)
                return NotFound("Practitioner not found.");

            // 📁 Folder Path (your structure)
            string uploadPath = Path.Combine(_environment.ContentRootPath, "wwwroot/Upload/Practitionerdocuments/1/2/2");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // 📄 File Name
            string safeOriginalName = Path.GetFileNameWithoutExtension(file.FileName).Replace("_", "-");
            string fileExtension = Path.GetExtension(file.FileName);

            string fileName = pid  + DateTime.Now.Ticks + "_" + safeOriginalName + fileExtension;
            string filePath = Path.Combine(uploadPath, fileName);

            // 💾 Save File
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 🔥 Generate DocumentId (YOUR METHOD)
            var documentId = await GenerateDocumentIdAsync();

            // 📌 Save to DB
            var doc = new PractitionerDocumentDetails
            {
                DocumentId = documentId,
                PractitionerID = pid,
                DocumentName = DocumentName,
                DocumentPath = "/Upload/Practitionerdocuments/1/2/2/" + fileName,
                DocumentSize = file.Length.ToString(),
                DocumentType= DocumentType,
                FileExtn = fileExtension,
                CreatedBy = pid,
                CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                PhotoYN = "N",
                BCodeYN = "N",
                Sync_Yn = "N"
            };

            _context.PractitionerDocumentDetails.Add(doc);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Document uploaded successfully",
                result = "success",
                documentId = documentId,
                filePath = doc.DocumentPath
            });
        }



        private async Task<string> GenerateDocumentIdAsync()
        {
            string id;
            var rnd = new Random();

            do
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                int random = rnd.Next(100, 999);
                id = $"D{timestamp}{random}";

            } while (await _context.PractitionerDocumentDetails
                        .AnyAsync(x => x.DocumentId == id));

            return id;
        }

        [AllowAnonymous]
        [HttpGet("getdocuments")]
        public async Task<IActionResult> GetDocuments([FromQuery] string pid)
        {
            try
            {
                if (string.IsNullOrEmpty(pid))
                    return BadRequest(new
                    {
                        result = "error",
                        message = "PractitionerId is required"
                    });

                var baseUrl = $"{Request.Scheme}://{Request.Host}";

                var data = await _context.PractitionerDocumentDetails
                    .Where(x => x.PractitionerID == pid)
                    .OrderByDescending(x => x.CreatedOn)
                    .ToListAsync();

                // 🔥 Group by DocumentName
                var groupedData = data
                    .GroupBy(x => x.DocumentName)
                    .Select(g => new
                    {
                        DocumentName = g.Key,
                        Documents = g.Select(x => new
                        {
                            x.DocumentId,
                            x.PractitionerID,
                            x.DocumentType,
                            x.DocumentSize,
                            x.FileExtn,
                            x.CreatedOn,

                            DocumentUrl = !string.IsNullOrEmpty(x.DocumentPath) &&
                                          x.DocumentPath.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                                ? x.DocumentPath
                                : baseUrl + (x.DocumentPath ?? "")
                        }).ToList()
                    }).ToList();

                return Ok(new
                {
                    result = "success",
                    count = groupedData.Count,
                    data = groupedData
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    result = "error",
                    message = ex.Message
                });
            }
        }


    }

}

