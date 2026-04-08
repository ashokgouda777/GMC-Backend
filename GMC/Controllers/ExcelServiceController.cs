using GMC.Data;
using GMC.Model;
using GMC.Model.GMC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GMC.Controllers
{
    public class ExcelServiceController : Controller
    {
        private readonly AppDbContext _context;
        public ExcelServiceController(AppDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost("uploadexcell")]
    
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("No file uploaded");

            var dt = new DataTable();

            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        var sheet = package.Workbook.Worksheets[0];

                        int rowCount = sheet.Dimension.Rows;
                        int colCount = sheet.Dimension.Columns;

                        // Columns
                        for (int col = 4; col <= colCount; col++)
                        {
                            dt.Columns.Add(sheet.Cells[1, col].Text);
                        }

                        // Rows
                        for (int row = 5; row <= rowCount; row++)
                        {
                            // ============================================================
                            // Complete C# Test Data Variables — Exact Column Mapping
                            // Source: Automation_Package_RMPs_data_12_Dec_2025.xlsx
                            // Sheet: Sheet2  |  Data rows start at row 4 (0-based row 3)
                            // EPPlus column index is 1-based (sheet.Cells[row, col])
                            // ============================================================

                            

                            // ── BASIC IDENTITY ───────────────────────────────────────────
                            var srno = sheet.Cells[row, 1].Text;   // Col  1  – Sr No
                            var regno = sheet.Cells[row, 2].Text;   // Col  2  – Reg No
                            var regdate = sheet.Cells[row, 3].Text;   // Col  3  – Date of Registration
                            var name = sheet.Cells[row, 4].Text;   // Col  4  – Name of RMP
                            var middlename = sheet.Cells[row, 5].Text;   // Col  5  – Maidan name
                            var fathername = sheet.Cells[row, 6].Text;   // Col  6  – So/Do
                            var dob = sheet.Cells[row, 7].Text;   // Col  7  – Date of Birth
                            var gendercode = sheet.Cells[row, 8].Text;   // Col  8  – Gender Code (1=Female, 2=Male)
                            var gender = sheet.Cells[row, 9].Text;   // Col  9  – Gender Name

                            // ── CONTACT DETAILS ──────────────────────────────────────────
                            var mobileno = sheet.Cells[row, 10].Text;   // Col 10  – Mobile No
                            var altmobileno = sheet.Cells[row, 11].Text;   // Col 11  – Alternate Mobile
                            var landlineno = sheet.Cells[row, 12].Text;   // Col 12  – Landline No
                            var email = sheet.Cells[row, 13].Text;   // Col 13  – Email Address

                            // ── ADDRESS ──────────────────────────────────────────────────
                            var addressline1 = sheet.Cells[row, 14].Text;   // Col 14  – Address Line 1
                            var addressline2 = sheet.Cells[row, 15].Text;   // Col 15  – Address Line 2
                            var addressline3 = sheet.Cells[row, 16].Text;   // Col 16  – Address Line 3
                            var city = sheet.Cells[row, 17].Text;   // Col 17  – City
                            var districtcode = sheet.Cells[row, 18].Text;   // Col 18  – District Code
                            var districtname = sheet.Cells[row, 19].Text;   // Col 19  – District Name
                            var statecode = sheet.Cells[row, 20].Text;   // Col 20  – State Code
                            var statename = sheet.Cells[row, 21].Text;   // Col 21  – State Name
                            var countrycode = sheet.Cells[row, 22].Text;   // Col 22  – Country Code
                            var countryname = sheet.Cells[row, 23].Text;   // Col 23  – Country Name
                            var zipcode = sheet.Cells[row, 24].Text;   // Col 24  – PIN / ZIP Code

                            // ── PRIMARY MEDICAL DEGREE ───────────────────────────────────
                            var degreecode = sheet.Cells[row, 25].Text;   // Col 25  – Primary Degree Code
                            var degreename = sheet.Cells[row, 26].Text;   // Col 26  – Primary Degree Name
                            var uincode = sheet.Cells[row, 27].Text;   // Col 27  – University Code (Primary)
                            var uinname = sheet.Cells[row, 28].Text;   // Col 28  – University Name (Primary)
                            var monthofpassing = sheet.Cells[row, 29].Text;   // Col 29  – Month of Passing Code (Primary)
                            var monthofpassingname = sheet.Cells[row, 30].Text;   // Col 30  – Month of Passing Name (Primary)
                            var yearofpassing = sheet.Cells[row, 31].Text;   // Col 31  – Year of Passing (Primary)

                            // ── ADDITIONAL QUALIFICATION 1 ───────────────────────────────
                            var degreecode0 = sheet.Cells[row, 32].Text;   // Col 32  – Add Qual 1 Degree Code
                            var degreename0 = sheet.Cells[row, 33].Text;   // Col 33  – Add Qual 1 Degree Name
                            var uincode0 = sheet.Cells[row, 34].Text;   // Col 34  – Add Qual 1 University Code
                            var uinname0 = sheet.Cells[row, 35].Text;   // Col 35  – Add Qual 1 University Name
                            var monthofpassing0 = sheet.Cells[row, 36].Text;   // Col 36  – Add Qual 1 Month Code
                            var monthofpassingname0 = sheet.Cells[row, 37].Text;   // Col 37  – Add Qual 1 Month Name
                            var yearofpassing0 = sheet.Cells[row, 38].Text;   // Col 38  – Add Qual 1 Year of Passing
                            var createdon0 = sheet.Cells[row, 39].Text;   // Col 39  – Add Qual 1 Registration Date

                            // ── ADDITIONAL QUALIFICATION 2 ───────────────────────────────
                            var degreecode1 = sheet.Cells[row, 40].Text;   // Col 40  – Add Qual 2 Degree Code
                            var degreename1 = sheet.Cells[row, 41].Text;   // Col 41  – Add Qual 2 Degree Name
                            var uincode1 = sheet.Cells[row, 42].Text;   // Col 42  – Add Qual 2 University Code
                            var uinname1 = sheet.Cells[row, 43].Text;   // Col 43  – Add Qual 2 University Name
                            var monthofpassing1 = sheet.Cells[row, 44].Text;   // Col 44  – Add Qual 2 Month Code
                            var monthofpassingname1 = sheet.Cells[row, 45].Text;   // Col 45  – Add Qual 2 Month Name
                            var yearofpassing1 = sheet.Cells[row, 46].Text;   // Col 46  – Add Qual 2 Year of Passing
                            var createdon1 = sheet.Cells[row, 47].Text;   // Col 47  – Add Qual 2 Registration Date

                            // ── ADDITIONAL QUALIFICATION 3 ───────────────────────────────
                            var degreecode2 = sheet.Cells[row, 48].Text;   // Col 48  – Add Qual 3 Degree Code
                            var degreename2 = sheet.Cells[row, 49].Text;   // Col 49  – Add Qual 3 Degree Name
                            var uincode2 = sheet.Cells[row, 50].Text;   // Col 50  – Add Qual 3 University Code
                            var uinname2 = sheet.Cells[row, 51].Text;   // Col 51  – Add Qual 3 University Name
                            var monthofpassing2 = sheet.Cells[row, 52].Text;   // Col 52  – Add Qual 3 Month Code
                            var monthofpassingname2 = sheet.Cells[row, 53].Text;   // Col 53  – Add Qual 3 Month Name
                            var yearofpassing2 = sheet.Cells[row, 54].Text;   // Col 54  – Add Qual 3 Year of Passing
                            var createdon2 = sheet.Cells[row, 55].Text;   // Col 55  – Add Qual 3 Registration Date

                            // ── ADDITIONAL QUALIFICATION 4 ───────────────────────────────
                            var degreecode3 = sheet.Cells[row, 56].Text;   // Col 56  – Add Qual 4 Degree Code
                            var degreename3 = sheet.Cells[row, 57].Text;   // Col 57  – Add Qual 4 Degree Name
                            var uincode3 = sheet.Cells[row, 58].Text;   // Col 58  – Add Qual 4 University Code
                            var uinname3 = sheet.Cells[row, 59].Text;   // Col 59  – Add Qual 4 University Name
                            var monthofpassing3 = sheet.Cells[row, 60].Text;   // Col 60  – Add Qual 4 Month Code
                            var monthofpassingname3 = sheet.Cells[row, 61].Text;   // Col 61  – Add Qual 4 Month Name
                            var yearofpassing3 = sheet.Cells[row, 62].Text;   // Col 62  – Add Qual 4 Year of Passing
                            var createdon3 = sheet.Cells[row, 63].Text;   // Col 63  – Add Qual 4 Registration Date

                            // ── REGISTRATION STATUS & DOCUMENTS ─────────────────────────
                            var regvalidupto = sheet.Cells[row, 64].Text;   // Col 64  – Reg Valid Upto
                            var pancardno = sheet.Cells[row, 65].Text;   // Col 65  – PAN No
                            var adharno = sheet.Cells[row, 66].Text;   // Col 66  – Aadhaar No
                            var statusofregistration = sheet.Cells[row, 67].Text;   // Col 67  – Status (1=Live, 2=Cancelled, 3=Surrendered)
                            var canceldate = sheet.Cells[row, 68].Text;   // Col 68  – Since When (if cancelled/surrendered)
                            var reason = sheet.Cells[row, 69].Text;   // Col 69  – Reason for Cancellation
                            var remark = sheet.Cells[row, 70].Text;   // Col 70  – Remarks
                            string practitionerid = await GenerateIdAsync("P");                    // 🔹 USER TABLE
                            var statusexist = "";
                            
                            try {

                            if (statusofregistration == "1")
                            {
                                statusexist = "A";
                            }
                            else
                            {
                                statusexist = "C";
                            }

                                var practitioner2 = new Practitioner
                                {
                                    PractitionerID = practitionerid,
                                    CountryId = "1",
                                    StateId = "1",
                                    CouncilId = "1",
                                    Name = name,
                                    Password = "not set",
                                    RegistrationFor = "1",
                                    RegistrationNo = regno,
                                    RegistrationDate = ParseDate(regdate),
                                    Title = "Dr",
                                    Gender = gendercode,
                                    SpouseName = fathername,
                                    BirthDate = ParseDate(dob),
                                    BirthPlace = "",
                                    Nationality = countrycode,
                                    Vote = statusofregistration,
                                    EmailID = email,
                                    MobileNumber = mobileno,
                                    CreatedOn = DateTime.Now,
                                    CreatedBy = "Old Record",
                                    status = statusexist,
                                    Remarks= remark,
                                    RegCanceldate= ParseDate(canceldate)

                                };

                            _context.Practitioners.Add(practitioner2);
                            await _context.SaveChangesAsync();

                            var user = new User
                            {
                                CouncilId = "1",
                                CountryId = "1",
                                StateId = "1",
                                Name = name,
                                UserName = regno,
                                MobileNo = mobileno,
                                Password = "not set",
                                userId = practitionerid,
                                Role_Id = "2",
                                CreatedOn = DateTime.Now,
                                CreatedBy = "Old Record"
                            };

                            _context.Users.Add(user);

                           
                                string addressid = await GenerateIdAsync("R");

                                var add = new Address
                                {
                                    AddressID = addressid,                  // Generated ID
                                    ClientID = practitionerid,
                                    AddressType = "R",

                                    CountryId = "1",
                                    StateId = "1",
                                    CouncilId = "1",

                                    Address1 = addressline1,
                                    Address2 = addressline2+","+addressline3,
                                    City = city,
                                    State = statecode,
                                    Zip = zipcode,
                                    Country = countrycode,

                                    Phone1 = mobileno,
                                    Phone2 = landlineno,
                                  
                                    District = districtcode,

                                    CreatedBy = "Old record",
                                    CreatedOn = DateTime.Now
                                };


                                _context.Address.Add(add);
                                await _context.SaveChangesAsync();

                                



                          

                          

                            if (!string.IsNullOrEmpty(degreename))
                            {
                                await AddEducation(practitionerid, degreename, yearofpassing, uincode, monthofpassing);
                            }

                            if (!string.IsNullOrEmpty(degreename0) && degreename0!= "#N/A")
                            {
                                await AddEducation(practitionerid, degreename0, yearofpassing0, uincode0, monthofpassing0);
                            }

                            if (!string.IsNullOrEmpty(degreename1) && degreename1 != "#N/A")
                            {
                                await AddEducation(practitionerid, degreename1, yearofpassing1, uincode1, monthofpassing1);
                            }

                            if (!string.IsNullOrEmpty(degreename2) && degreename2 != "#N/A")
                            {
                                await AddEducation(practitionerid, degreename2, yearofpassing2, uincode2, monthofpassing2);
                            }
                          
                           
                            // Save once
                            await _context.SaveChangesAsync();
                            // 🔹 Save remaining
                            _context.SaveChanges();
                                await Task.Delay(10);
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    string logPath = @"D:\ErrorLogs\import_errors.txt";

                                    // Create folder if not exists
                                    if (!Directory.Exists(@"D:\ErrorLogs"))
                                    {
                                        Directory.CreateDirectory(@"D:\ErrorLogs");
                                    }

                                    string logMessage = $"RegNo: {regno} | Error: {e.Message} | Time: {DateTime.Now}";
                                    System.IO.File.AppendAllText(logPath, logMessage + Environment.NewLine);
                                    // Append to file (Notepad file)
                                 
                                }
                                catch
                                {
                                    // Optional: ignore logging failure
                                }
                            }
                        }
                    }
                }

                return Json(dt);
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }

                private async Task AddEducation(
                string practitionerid,
                string degreecode,
                string yearofpassing,
                string uincode,
                string monthofpassing)
                    {
                        string eid = await GenerateIdAsync("E");

                        var entity = new EducationInfo
                        {
                            CountryId = "1",
                            StateId = "1",
                            CouncilId = "1",
                            EducationID = eid,
                            PractitionerID = practitionerid,

                            EducationName = degreecode,
                            YearOfPassing = yearofpassing,
                            UniversityId = uincode,
                            MonthOfPassing = monthofpassing,

                            IsIssued = "yes",
                            CreatedBy = "Old data",
                            CreatedOn = DateTime.Now,
                            Active = "A"
                        };

                        _context.EducationInfo.Add(entity);
                    }

        private async Task<string> GenerateIdAsync(string type)
        {
            string id;
            var rnd = new Random();

            do
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                int random = rnd.Next(100, 999);
                id = $"{type}{timestamp}{random}";

            } while (await _context.Users.AnyAsync(x => x.userId == id));

            return id;
        }
        private DateTime? ParseDate(string value)
        {
            if (DateTime.TryParse(value, out DateTime d))
                return d;
            return null;
        }
       

    }
}