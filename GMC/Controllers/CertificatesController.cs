using GMC.Data;
using GMC.Model;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using System.Collections.Concurrent;



namespace GMC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private static int counter = 1;

        public CertificatesController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [AllowAnonymous]
        [HttpGet("printcertificate")]
        public async Task<IActionResult> GenerateReceipt(string rid, string pid)
        {
            var payment = (from r in _context.RenewalHistory
                           join p in _context.Practitioners
                           on r.PractitionerID equals p.PractitionerID
                           where r.RenewalID == rid && r.PractitionerID == pid
                           select new
                           {
                               r.Amount,
                               r.PaymentFor,
                               r.ReceiptNumber,
                               r.ReceiptDate,
                               p.Name,
                               p.RegistrationNo,
                               p.RegistrationDate,
                               p.photo,
                               p.practsign,
                               p.Gender,
                               p.barcode
                           }).FirstOrDefault();

            if (payment == null)
                return NotFound("Receipt not found");


            if (payment.PaymentFor == "LED5"|| payment.PaymentFor == "LED6")


            {
                var practitioner = await _context.Practitioners
                    .FirstOrDefaultAsync(p => p.PractitionerID == pid);



                var title = practitioner.Title;

                string fullName = $"{(title?.EndsWith(".") == true ? title : title + ".")}{practitioner.Name}".Trim().ToUpper();


                if (practitioner == null)
                    return NotFound("Practitioner not found");

                var education = await (from e in _context.EducationInfo
                                       join c in _context.CourseMaster
                                       on e.SubCode equals c.CourseId
                                       join u in _context.University
                                       on e.UniversityId equals u.UniversityId
                                       where e.PractitionerID == pid
                                       select new
                                       {
                                           e.EducationName,
                                           e.MonthOfPassing,
                                           e.YearOfPassing,
                                           c.CourseDescription,
                                           u.UniversityName
                                       })
                       .ToListAsync();




                string qualification = string.Join(", ",
                                education.Select(x => $"{x.EducationName}" + "(" + $"{x.CourseDescription}" + ") (" + x.UniversityName + ") " + GetMonthName(Convert.ToInt32(x.MonthOfPassing)) + "," + x.YearOfPassing));

                string templatePath = Path.Combine(
                    _environment.WebRootPath,
                    "Upload",
                    "certificates",
                    "Registration certificate.pdf"
                );

                using (MemoryStream ms = new MemoryStream())
                {
                    var reader = new PdfReader(templatePath);
                    var writer = new PdfWriter(ms);
                    var pdf = new PdfDocument(reader, writer);
                    var document = new Document(pdf);

                    PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                    // Get page size for grid
                    var pageSize = pdf.GetFirstPage().GetPageSize();
                    float pageWidth = pageSize.GetWidth();
                    float pageHeight = pageSize.GetHeight();

                    // Registration Number
                    document.ShowTextAligned(
                        new Paragraph(practitioner.RegistrationNo ?? "")
                            .SetFont(font)
                            .SetFontSize(11),
                        190, 615,
                        TextAlignment.LEFT
                    );


                    // Name
                    document.ShowTextAligned(
                       new Paragraph(fullName)
                      .SetFont(font)
                      .SetFontSize(11)
                      .SetFontColor(ColorConstants.BLACK),
                      180, 483,
                      TextAlignment.LEFT
                    );

                    // Qualification


                    float[] startX = { 340, 100, 100 };
                    float[] endX = { 510, 510, 510 };
                    float[] startY = { 460, 430, 402 };

                    string text = qualification?.ToUpper() ?? "";

                    // Split text into words (IMPORTANT)
                    string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    int wordIndex = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        if (wordIndex >= words.Length)
                            break;

                        float maxWidth = endX[i] - startX[i];

                        string line = "";

                        while (wordIndex < words.Length)
                        {
                            string nextWord = words[wordIndex];

                            string testLine = string.IsNullOrEmpty(line)
                                ? nextWord
                                : line + " " + nextWord;

                            float textWidth = font.GetWidth(testLine, 11);

                            // If fits → add word
                            if (textWidth <= maxWidth)
                            {
                                line = testLine;
                                wordIndex++;
                            }
                            else
                            {
                                // If even single word doesn't fit → force place it
                                if (string.IsNullOrEmpty(line))
                                {
                                    line = nextWord;
                                    wordIndex++;
                                }
                                break;
                            }
                        }

                        // Draw text line
                        document.ShowTextAligned(
                            new Paragraph(line)
                                .SetFont(font)
                                .SetFontSize(11)
                                .SetFontColor(ColorConstants.BLACK),
                            startX[i],
                            startY[i],
                            TextAlignment.LEFT
                        );
                    }



                    // Registration Date
                    document.ShowTextAligned(
                        new Paragraph(practitioner.RegistrationDate?.ToString("dd MMM yyyy") ?? "")
                            .SetFont(font)
                            .SetFontSize(11)
                            .SetFontColor(ColorConstants.BLACK),
                        160, 222,
                        TextAlignment.LEFT
                    );

                    // PHOTO
                    if (!string.IsNullOrEmpty(practitioner.photo))
                    {
                        string photoPath = Path.Combine(
                            _environment.WebRootPath,
                            "Upload",
                            "PractitionerPhotos",
                            "1", "2", "2",
                            practitioner.photo
                        );

                        if (System.IO.File.Exists(photoPath))
                        {
                            ImageData imgData = ImageDataFactory.Create(photoPath);

                            var img = new Image(imgData)
                                .ScaleAbsolute(105f, 130f)
                                .SetFixedPosition(395f, 535f);

                            document.Add(img);
                        }
                    }

                    // Bar code


                    if (!string.IsNullOrEmpty(practitioner.barcode))
                    {
                        string barcodePath = Path.Combine(
                            _environment.WebRootPath,
                             "Upload",
                            "PractitionerPhotos",
                            "1", "2", "2",
                            practitioner.barcode
                        );

                        if (System.IO.File.Exists(barcodePath))
                        {
                            ImageData barcodeData = ImageDataFactory.Create(barcodePath);

                            var barcodeImg = new Image(barcodeData)
                                .ScaleAbsolute(90f, 90f)   // adjust size
                                .SetFixedPosition(270f, 120f); // bottom position

                            document.Add(barcodeImg);
                        }
                    }



                    document.Close();

                    return File(ms.ToArray(), "application/pdf", "Registration Certificate.pdf");
                }
            }


            else if (payment.PaymentFor == "LED8")

            {
                var practitioner = await _context.Practitioners
                     .FirstOrDefaultAsync(p => p.PractitionerID == pid);

                if (practitioner == null)
                    return NotFound("Practitioner not found");

                var education = await _context.EducationInfo
                    .FirstOrDefaultAsync(e => e.PractitionerID == pid);

                if (education == null)
                    return NotFound("Education info not found");

                var college = await _context.College
                    .FirstOrDefaultAsync(c => c.ColId == education.CollegeID);

                var university = await _context.University
                    .FirstOrDefaultAsync(u => u.UniversityId == education.UniversityId);


                var address = await _context.Address
                    .FirstOrDefaultAsync(a => a.ClientID == practitioner.PractitionerID);

                string cityName = address?.City ?? "";
                string stateName = "";
                if (address != null)
                {
                    var state = await _context.StateMaster
                        .FirstOrDefaultAsync(s => s.StateId == address.StateId);
                    stateName = state?.StateName ?? "";
                }

                string universityName = university?.UniversityName ?? "";
                //string yearOfPassing = education.YearOfPassing?.ToString() ?? "";

                string monthYear = (int.TryParse(education?.MonthOfPassing, out int m) && m >= 1 && m <= 12 && education.YearOfPassing != null)
                   ? $"{System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(m)}'{education.YearOfPassing}"
                   : string.Empty;



                // Format registration date as "MMM yyyy" like "Feb 2026"
                string registrationDate = practitioner.RegistrationDate.HasValue
                    ? practitioner.RegistrationDate.Value.ToString("MMM yyyy")
                    : "";

                string regDateFormat2 = practitioner.RegistrationDate.HasValue
                ? practitioner.RegistrationDate.Value.ToString("dd-MMM-yyyy")
                   : "";

                // Format 3: 16-Feb-2025 (can use same as format2)
                string regDateFormat3 = practitioner.RegistrationDate.HasValue
                    ? practitioner.RegistrationDate.Value.ToString("dd-MMM-yyyy")
                    : "";

                string templatePath = Path.Combine(_environment.WebRootPath, "Upload", "certificates", "PRC certificate.pdf");

                using (MemoryStream ms = new MemoryStream())
                {
                    var reader = new PdfReader(templatePath);
                    var writer = new PdfWriter(ms);
                    var pdf = new PdfDocument(reader, writer);
                    var document = new Document(pdf);

                    PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                    PdfPage page = pdf.GetPage(1);
                    float pageWidth = page.GetPageSize().GetWidth();
                    float pageHeight = page.GetPageSize().GetHeight();



                    // Add practitioner info
                    document.ShowTextAligned(
                        new Paragraph(practitioner.RegistrationNo ?? "")
                        .SetFont(font)
                        .SetFontSize(11),
                        230, 525, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph((practitioner.Name ?? "").ToUpper())
                        .SetFont(font)
                        .SetFontSize(11),
                        330, 488, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    // University and Year of Passing
                    document.ShowTextAligned(
                        new Paragraph(universityName.ToUpper())
                        .SetFont(font)
                        .SetFontSize(11),
                        120, 437, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(monthYear)
                        .SetFont(font)
                        .SetFontSize(11),
                        400, 437, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    // City, State
                    document.ShowTextAligned(
                        new Paragraph($"{cityName}, {stateName}".ToUpper())
                        .SetFont(font)
                        .SetFontSize(11),
                        110, 460, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(regDateFormat2.ToUpper())
                        .SetFont(font)
                        .SetFontSize(11),
                         100, 211, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(regDateFormat3.ToUpper())
                        .SetFont(font)
                        .SetFontSize(11),
                        130, 102, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    if (!string.IsNullOrEmpty(practitioner.barcode))
                    {
                        string barcodePath = Path.Combine(
                            _environment.WebRootPath,
                             "Upload",
                            "PractitionerPhotos",
                            "1", "2", "2",
                            practitioner.barcode
                        );

                        if (System.IO.File.Exists(barcodePath))
                        {
                            ImageData barcodeData = ImageDataFactory.Create(barcodePath);

                            var barcodeImg = new Image(barcodeData)
                                .ScaleAbsolute(90f, 50f)   // adjust size
                                .SetFixedPosition(270f, 120f); // bottom position

                            document.Add(barcodeImg);
                        }
                    }



                    document.Close();
                    return File(ms.ToArray(), "application/pdf", "PRC certificate.pdf");
                }
            }

            else if (payment.PaymentFor == "LED27")
            {
                var practitioner = await _context.Practitioners
                    .FirstOrDefaultAsync(p => p.PractitionerID == pid);

                if (practitioner == null)
                    return NotFound("Practitioner not found");

                var education = await _context.EducationInfo
                    .Where(e => e.PractitionerID == pid)
                    .ToListAsync();

                var council = await _context.JCouncilMaster
                    .FirstOrDefaultAsync(c => c.CouncilId == practitioner.CouncilId);

                var renewal = await _context.RenewalHistory
             .Where(r => r.PractitionerID == pid && !string.IsNullOrEmpty(r.RefNo))
                 .OrderByDescending(r => r.RenewalID)
              .FirstOrDefaultAsync();

                string qualification = string.Join(", ", education.Select(e => e.EducationName));

                string templatePath = Path.Combine(_environment.WebRootPath, "Upload", "certificates", "NOC_Format.pdf");

                string genderWord = practitioner.Gender != null && practitioner.Gender.ToString() == "2" ? "her" : "his";
                string referenceNo = renewal?.RefNo ?? "";
                using (MemoryStream ms = new MemoryStream())
                {
                    var reader = new PdfReader(templatePath);
                    var writer = new PdfWriter(ms);
                    var pdf = new PdfDocument(reader, writer);
                    var document = new Document(pdf);

                    PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                    var pageSize = pdf.GetFirstPage().GetPageSize();
                    float pageWidth = pageSize.GetWidth();
                    float pageHeight = pageSize.GetHeight();

                    // ================= EXISTING CONTENT =================

                    document.ShowTextAligned(
                        new Paragraph(practitioner.RegistrationNo ?? "")
                        .SetFont(font).SetFontSize(10),
                        320, 452, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph((practitioner.Name ?? "").ToUpper())
                        .SetFont(font)
                        .SetFontSize(10),
                        150, 368, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph((practitioner.Name ?? "").ToUpper())
                        .SetFont(font)
                        .SetFontSize(11),
                        150, 474, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(qualification?.ToUpper() ?? "")
                        .SetFont(font)
                        .SetFontSize(10),
                        280, 474, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(practitioner.RegistrationDate?.ToString("dd MMM yyyy") ?? "")
                        .SetFont(font)
                        .SetFontSize(8),
                        394, 452, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(DateTime.Now.ToString("dd MMM yyyy"))
                        .SetFont(font)
                        .SetFontSize(11),
                        415, 647, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );
                    document.ShowTextAligned(
                           new Paragraph(referenceNo)
                          .SetFont(font)
                          .SetFontSize(11),
                            147, 647,
                            1,
                           TextAlignment.LEFT,
                              VerticalAlignment.BOTTOM,
                             0
                    );


                    // ================= GENDER =================

                    document.ShowTextAligned(
                        new Paragraph(genderWord)
                        .SetFont(normalFont)
                        .SetFontSize(10),
                        242, 347, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(genderWord)
                        .SetFont(normalFont)
                        .SetFontSize(9),
                        441, 452, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(genderWord)
                        .SetFont(normalFont)
                        .SetFontSize(10),
                        226, 451, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    // ================= COUNCIL (NEW) =================
                    string councilName = council?.CouncilName ?? "";
                    string councilAddress = council?.Address ?? "";

                    // Council Name (Bold)
                    document.ShowTextAligned(
                        new Paragraph(councilName.ToUpper())
                            .SetFont(normalFont)
                            .SetFontSize(10),
                        122, 590,   // adjust if needed
                        1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );
                    string formattedAddress = councilAddress.ToUpper();

                    Div addressDiv = new Div()
                        .SetFixedPosition(122, 545, 180)
                        .Add(new Paragraph(formattedAddress)
                            .SetFont(normalFont)
                            .SetFontSize(10)
                            .SetMultipliedLeading(1.2f));

                    document.Add(addressDiv);

                    // ================= BARCODE =================

                    if (!string.IsNullOrEmpty(practitioner.barcode))
                    {
                        string barcodePath = Path.Combine(
                            _environment.WebRootPath,
                            "Upload",
                            "PractitionerPhotos",
                            "1", "2", "2",
                            practitioner.barcode
                        );

                        if (System.IO.File.Exists(barcodePath))
                        {
                            ImageData barcodeData = ImageDataFactory.Create(barcodePath);

                            var barcodeImg = new Image(barcodeData)
                                .ScaleAbsolute(90f, 90f)
                                .SetFixedPosition(110f, 120f);

                            document.Add(barcodeImg);
                        }
                    }



                    document.Close();

                    return File(ms.ToArray(), "application/pdf", "Noc-Certificate.pdf");
                }
            }

            else if (payment.PaymentFor == "LED10")
            {
                var practitioner = await _context.Practitioners
                    .FirstOrDefaultAsync(p => p.PractitionerID == pid);

                if (practitioner == null)
                    return NotFound("Practitioner not found");

                var education = await _context.EducationInfo
                    .Where(e => e.PractitionerID == pid)
                    .ToListAsync();
                var renewal = await _context.RenewalHistory
            .Where(r => r.PractitionerID == pid && !string.IsNullOrEmpty(r.GoodStationdingRefNo))
                .OrderByDescending(r => r.RenewalID)
             .FirstOrDefaultAsync();

                string qualification = string.Join(", ", education.Select(e => e.Subject));

                string templatePath = Path.Combine(_environment.WebRootPath,
                    "Upload", "certificates", "Certificate of Good Standing format.pdf");
                // Get gender word
                string genderWord = practitioner.Gender != null && practitioner.Gender.ToString() == "2" ? "her" : "his";
                string referenceNo = renewal?.GoodStationdingRefNo ?? "";

                using (MemoryStream ms = new MemoryStream())
                {
                    var reader = new PdfReader(templatePath);
                    var writer = new PdfWriter(ms);
                    var pdf = new PdfDocument(reader, writer);
                    var document = new Document(pdf);

                    PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                    var page = pdf.GetPage(1);
                    float pageWidth = page.GetPageSize().GetWidth();
                    float pageHeight = page.GetPageSize().GetHeight();


                    document.ShowTextAligned(
                        new Paragraph(practitioner.RegistrationNo ?? "")
                        .SetFont(font).SetFontSize(10),
                        250, 321, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph((practitioner.Name ?? "").ToUpper())
                        .SetFont(font)
                        .SetFontSize(10),
                        150, 342, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph((practitioner.Name ?? "").ToUpper())
                        .SetFont(font)
                        .SetFontSize(11),
                        150, 240, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(qualification?.ToUpper() ?? "")
                        .SetFont(font)
                        .SetFontSize(10),
                        230, 342, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(practitioner.RegistrationDate?.ToString("dd MMM yyyy") ?? "")
                        .SetFont(font)
                        .SetFontSize(10),
                        314, 320, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(DateTime.Now.ToString("dd MMM yyyy"))
                        .SetFont(font)
                        .SetFontSize(11),
                        415, 570, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                      new Paragraph(referenceNo)
                      .SetFont(font)
                      .SetFontSize(11),
                      172, 570, 1,
                      TextAlignment.LEFT,
                      VerticalAlignment.BOTTOM,
                      0
                    );
                    document.ShowTextAligned(
                     new Paragraph(genderWord)
                    .SetFont(normalFont)
                    .SetFontSize(10),
                    175, 218, 1,
                    TextAlignment.LEFT,
                    VerticalAlignment.BOTTOM,
                     0
                   );

                    document.ShowTextAligned(
                     new Paragraph(genderWord)
                    .SetFont(normalFont)
                    .SetFontSize(10),
                    142, 320, 1,
                    TextAlignment.LEFT,
                    VerticalAlignment.BOTTOM,
                     0
                    );

                    document.ShowTextAligned(
                     new Paragraph(genderWord)
                    .SetFont(normalFont)
                    .SetFontSize(10),
                    393, 320, 1,
                    TextAlignment.LEFT,
                    VerticalAlignment.BOTTOM,
                     0
                    );
                    if (!string.IsNullOrEmpty(practitioner.barcode))
                    {
                        string barcodePath = Path.Combine(
                            _environment.WebRootPath,
                             "Upload",
                            "PractitionerPhotos",
                            "1", "2", "2",
                            practitioner.barcode
                        );

                        if (System.IO.File.Exists(barcodePath))
                        {
                            ImageData barcodeData = ImageDataFactory.Create(barcodePath);

                            var barcodeImg = new Image(barcodeData)
                                .ScaleAbsolute(90f, 90f)   // adjust size
                                .SetFixedPosition(150f, 70f); // bottom position

                            document.Add(barcodeImg);
                        }
                    }


                    document.Close();
                    return File(ms.ToArray(), "application/pdf", "GoodstandingCertificate.pdf");
                }
            }

            else
            {
                return BadRequest("Invalid payment type");
            }
        }

        private string GetMonthName(int month)
        {
            if (month < 1 || month > 12)
                return "Invalid Month";

            return new DateTime(2024, month, 1).ToString("MMMM");
        }

        [AllowAnonymous]
        [HttpGet("GenerateReceipt/{receiptnumber}")]
        public IActionResult GenerateReceipt(string receiptnumber)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);

                Document document = new Document(pdf);
                document.SetMargins(10, 100, 0, 100);

                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                // Create table with 2 columns: 1 for logo, 4 for text
                Table headerTable = new Table(2);

                // LOGO CELL (left side)
                string logoPath = Path.Combine(_environment.WebRootPath, "Upload", "Logo", "1", "2", "2", "GMC-logo.png");
                Cell logoCell;

                if (System.IO.File.Exists(logoPath))
                {
                    ImageData imageData = ImageDataFactory.Create(logoPath);
                    Image logo = new Image(imageData).ScaleAbsolute(50f, 50f)
                     .SetMarginRight(10);

                    logoCell = new Cell().Add(logo)
                        .SetBorder(Border.NO_BORDER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE) // vertically align with text
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPaddingRight(20);

                }
                else
                {
                    logoCell = new Cell().SetBorder(Border.NO_BORDER).SetPadding(0);
                }

                headerTable.AddCell(logoCell);

                // TEXT CELL (right of logo, aligned left)
                Cell textCell = new Cell().SetBorder(Border.NO_BORDER)
                    .SetPadding(9)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE); // vertically center with logo

                textCell.Add(new Paragraph("GOA MEDICAL COUNCIL")
                    .SetFont(bold)
                    .SetFontSize(12)
                    .SetMargin(0)
                    .SetTextAlignment(TextAlignment.CENTER));

                textCell.Add(new Paragraph("IInd Floor, Faculty Block, G.M.C. Complex, Bambolim, Goa - 403202")
                    .SetFont(normal)
                    .SetFontSize(8)
                    .SetMargin(0)
                    .SetTextAlignment(TextAlignment.CENTER));

                textCell.Add(new Paragraph("Email : goamedcouncil@rediffmail.com | Phone : 9975208199")
                    .SetFont(normal)
                    .SetFontSize(8)
                    .SetMargin(0)
                    .SetTextAlignment(TextAlignment.CENTER));

                textCell.Add(new Paragraph("RECEIPT")
                    .SetFont(bold)
                    .SetFontSize(11)
                    .SetMarginTop(2)
                    .SetTextAlignment(TextAlignment.CENTER));

                headerTable.AddCell(textCell);

                // Add header table to document
                document.Add(headerTable);

                //---------------- FETCH PAYMENT DATA ----------------//
                var payment = (from r in _context.RenewalHistory
                               join p in _context.Practitioners
                               on r.PractitionerID equals p.PractitionerID
                               where r.ReceiptNumber == receiptnumber
                               select new
                               {
                                   r.Amount,
                                   r.PaymentFor,
                                   r.ReceiptNumber,
                                   r.ReceiptDate,
                                   p.Name,
                                   p.RegistrationNo,
                                   r.Bank,

                               }).FirstOrDefault();

                if (payment == null)
                    return NotFound("Receipt not found");

                decimal amount = Convert.ToDecimal(payment.Amount);
                string amountInWords = NumberToWords((long)amount) + " only";



                //---------------- RECEIPT INFO ----------------//
                Table info = new Table(new float[] { 3, 3 }).UseAllAvailableWidth();
                info.AddCell(new Cell().Add(new Paragraph("Receipt No : " + payment.ReceiptNumber).SetFont(normal).SetFontSize(9)).SetBorder(Border.NO_BORDER));
                info.AddCell(new Cell().Add(new Paragraph("Date : " + Convert.ToDateTime(payment.ReceiptDate).ToString("dd/MM/yyyy")).SetFont(normal).SetFontSize(9)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));
                document.Add(info);

                document.Add(new Paragraph("Received with thanks from : Dr. " + payment.Name).SetFont(normal).SetFontSize(9));
                document.Add(new Paragraph("Rs : " + amount.ToString("0.00") + "   (Rupees : " + amountInWords.ToUpper() + ")").SetFont(normal).SetFontSize(9));
                document.Add(
                          new Paragraph("Drawn on Bank : " + payment.Bank)

                     .SetFont(normal)
                   .SetFontSize(9)
                 );
                //---------------- REGISTRATION + GST ----------------//
                Table reg = new Table(new float[] { 3, 3 }).UseAllAvailableWidth();
                reg.AddCell(new Cell().Add(new Paragraph("Registration No : " + payment.RegistrationNo).SetFont(normal).SetFontSize(9)).SetBorder(Border.NO_BORDER));
                reg.AddCell(new Cell().Add(new Paragraph("GMC GST No : 29AAALK0069K1ZV").SetFont(normal).SetFontSize(9)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));
                document.Add(reg);

                //---------------- PARTICULAR TABLE ----------------//
                var ledgers = _context.GroupLedgerMaster.OrderBy(x => x.LedgerID).Take(31).ToList();

                Table table = new Table(new float[] { 4, 1 }).SetWidth(UnitValue.CreatePercentValue(65)).SetHorizontalAlignment(HorizontalAlignment.CENTER);
                table.SetBorder(new SolidBorder(1));

                table.AddHeaderCell(new Cell().Add(new Paragraph("Particulars").SetFont(bold).SetFontSize(9)).SetTextAlignment(TextAlignment.CENTER).SetBorderBottom(new SolidBorder(1)).SetBorderRight(new SolidBorder(1)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Rs").SetFont(bold).SetFontSize(9)).SetTextAlignment(TextAlignment.CENTER).SetBorderBottom(new SolidBorder(1)));

                decimal total = 0;
                int count = 1;

                foreach (var ledger in ledgers)
                {
                    table.AddCell(new Cell().Add(new Paragraph(count + ". " + ledger.LedgerName).SetFont(normal).SetFontSize(8)).SetBorderRight(new SolidBorder(1)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
                    if (ledger.LedgerID == payment.PaymentFor)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(amount.ToString("0.00")).SetFont(normal).SetFontSize(8)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));
                        total += amount;
                    }
                    else
                        table.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER));
                    count++;
                }

                table.AddCell(new Cell().Add(new Paragraph("")).SetBorderTop(new SolidBorder(1)).SetBorderRight(new SolidBorder(1)));
                table.AddCell(new Cell().Add(new Paragraph("")).SetBorderTop(new SolidBorder(1)));

                decimal gst = total * 0.18m;
                table.AddCell(new Cell().Add(new Paragraph("Total Tax (CGST 9% + SGST 9%)").SetFont(normal).SetFontSize(8)).SetBorderRight(new SolidBorder(1)));
                table.AddCell(new Cell().Add(new Paragraph(gst.ToString("0.00")).SetFont(normal).SetFontSize(8)).SetTextAlignment(TextAlignment.RIGHT));

                decimal grand = total + gst;
                table.AddCell(new Cell().Add(new Paragraph("Total Fee Amount (Including GST)").SetFont(bold).SetFontSize(9)).SetBorderRight(new SolidBorder(1)));
                table.AddCell(new Cell().Add(new Paragraph(grand.ToString("0.00")).SetFont(bold).SetFontSize(9)).SetTextAlignment(TextAlignment.RIGHT));

                document.Add(table);

                //---------------- SIGNATURE ----------------//
                Table sign = new Table(new float[] { 3, 3 }).UseAllAvailableWidth().SetMarginTop(40);
                sign.AddCell(new Cell().Add(new Paragraph("Signature of the Applicant").SetFont(normal).SetFontSize(9)).SetBorder(Border.NO_BORDER));
                sign.AddCell(new Cell().Add(new Paragraph("Signature of the Registrar\nGoa Medical Council").SetFont(normal).SetFontSize(9)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));
                document.Add(sign);




                document.Close();

                return File(stream.ToArray(), "application/pdf", "GoaMedicalCouncilReceipt.pdf");
            }
        }

        //---------------- HELPER METHOD ----------------//
        private static string NumberToWords(long number)
        {
            if (number == 0) return "zero";
            if (number < 0) return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += NumberToWords(number / 10000000) + " crore ";
                number %= 10000000;
            }
            if ((number / 100000) > 0)
            {
                words += NumberToWords(number / 100000) + " lakh ";
                number %= 100000;
            }
            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "") words += "and ";
                string[] unitsMap = { "zero","one","two","three","four","five","six","seven","eight","nine","ten",
                                  "eleven","twelve","thirteen","fourteen","fifteen","sixteen",
                                  "seventeen","eighteen","nineteen" };
                string[] tensMap = { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += "-" + unitsMap[number % 10];
                }
            }

            return words.Trim();
        }

        [HttpGet("GenerateLedgerReport")]
        public async Task<IActionResult> GenerateLedgerReport(
                int? financialYearId,
                string? ledgerId,
                string? fromDate,
                string? toDate)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(stream);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);
                    document.SetMargins(20, 40, 20, 40);

                    PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    PdfFont normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                    // 🔷 FETCH DATA
                    var financialYear = await _context.FinancialYearMaster
                        .Where(f => f.FinYearrId == financialYearId)
                        .Select(f => f.FinancialYear)
                        .FirstOrDefaultAsync();

                    var query = _context.RenewalHistory.AsNoTracking().AsQueryable();

                    DateTime? from = null;
                    DateTime? to = null;

                    if (!string.IsNullOrEmpty(fromDate) && DateTime.TryParse(fromDate, out var fd))
                        from = fd.Date;

                    if (!string.IsNullOrEmpty(toDate) && DateTime.TryParse(toDate, out var td))
                        to = td.Date.AddDays(1).AddTicks(-1);

                    if (from.HasValue)
                        query = query.Where(r => (r.ReceiptDate ?? r.TransactionDate) >= from.Value);

                    if (to.HasValue)
                        query = query.Where(r => (r.ReceiptDate ?? r.TransactionDate) <= to.Value);

                    if (!string.IsNullOrEmpty(ledgerId))
                        query = query.Where(r => r.PaymentFor == ledgerId);

                    var transactions = await query
                        .OrderBy(r => r.ReceiptDate ?? r.TransactionDate)
                        .Select(r => new
                        {
                            Date = r.ReceiptDate ?? r.TransactionDate,
                            GroupReceiptNumber = r.ReceiptNumber,
                            ReceiptNumber = r.ReceiptNumber,
                            Amount = r.Amount
                        })
                        .ToListAsync();

                    // 🔷 HEADER TABLE (LOGO + TEXT)
                    Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 4 }))
                        .UseAllAvailableWidth();

                    string logoPath = Path.Combine(_environment.WebRootPath, "Upload", "Logo", "1", "2", "2", "GMC-logo.png");

                    Cell logoCell;

                    if (System.IO.File.Exists(logoPath))
                    {
                        ImageData imageData = ImageDataFactory.Create(logoPath);
                        Image logo = new Image(imageData)
                            .ScaleAbsolute(50f, 50f)
                            .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                        logoCell = new Cell()
                            .Add(logo)
                            .SetBorder(Border.NO_BORDER)
                            .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    }
                    else
                    {
                        logoCell = new Cell().SetBorder(Border.NO_BORDER);
                    }

                    headerTable.AddCell(logoCell);

                    // 🔷 TEXT CELL
                    Cell textCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);

                    textCell.Add(new Paragraph("GOA MEDICAL COUNCIL")
                        .SetFont(bold)
                        .SetFontSize(13)
                        .SetTextAlignment(TextAlignment.CENTER));

                    textCell.Add(new Paragraph("IInd Floor, Faculty Block, G.M.C. Complex, Bambolim, Goa - 403202")
                        .SetFont(normal)
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER));

                    textCell.Add(new Paragraph("Email : goamedcouncil@rediffmail.com | Phone : 9975208199")
                        .SetFont(normal)
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER));

                    headerTable.AddCell(textCell);

                    document.Add(headerTable);

                    // 🔷 TITLE
                    document.Add(new Paragraph("Ledger Report")
                        .SetFont(bold)
                        .SetFontSize(11)
                        .SetFontColor(ColorConstants.BLUE)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(5));

                    // 🔷 SUBTITLE
                    document.Add(new Paragraph(
                        $"GMC Cash book, Ledger Report for Financial Year {financialYear} and The Period From: {fromDate} To {toDate}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(normal)
                        .SetFontSize(9)
                        .SetMarginBottom(10));

                    // 🔷 TABLE (NO INNER GRID)
                    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 2, 1 }))
                        .UseAllAvailableWidth();

                    table.SetBorder(Border.NO_BORDER);

                    // HEADER ROW
                    string[] headers = { "Date", "Group Receipt Number", "Receipt Number", "Amount" };

                    foreach (var h in headers)
                    {
                        table.AddHeaderCell(new Cell()
                            .Add(new Paragraph(h).SetFont(bold))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBorderTop(new SolidBorder(1))
                            .SetBorderBottom(new SolidBorder(1))
                            .SetBorderLeft(Border.NO_BORDER)
                            .SetBorderRight(Border.NO_BORDER));
                    }

                    // DATA
                    foreach (var item in transactions)
                    {
                        table.AddCell(new Cell()
                            .Add(new Paragraph(item.Date?.ToString("dd/MM/yyyy")))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell()
                            .Add(new Paragraph(item.GroupReceiptNumber))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell()
                            .Add(new Paragraph(item.ReceiptNumber))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell()
                            .Add(new Paragraph(item.Amount ?? ""))
                            .SetTextAlignment(TextAlignment.RIGHT)
                            .SetBorder(Border.NO_BORDER));
                    }

                    document.Add(table);

                    document.Close();

                    return File(stream.ToArray(), "application/pdf", "LedgerReport.pdf");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GenerateDaybookReport")]
        public async Task<IActionResult> GenerateDaybookReport(
            int? financialYearId,
            string? ledgerId,
            string? fromDate,
            string? toDate)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(stream);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);
                    document.SetMargins(20, 40, 20, 40);

                    PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    PdfFont normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                    // 🔷 FETCH DATA
                    var financialYear = await _context.FinancialYearMaster
                        .Where(f => f.FinYearrId == financialYearId)
                        .Select(f => f.FinancialYear)
                        .FirstOrDefaultAsync();

                    var query = _context.RenewalHistory.AsNoTracking().AsQueryable();

                    DateTime? from = null;
                    DateTime? to = null;

                    if (!string.IsNullOrEmpty(fromDate) && DateTime.TryParse(fromDate, out var fd))
                        from = fd.Date;

                    if (!string.IsNullOrEmpty(toDate) && DateTime.TryParse(toDate, out var td))
                        to = td.Date.AddDays(1).AddTicks(-1);

                    if (from.HasValue)
                        query = query.Where(r => (r.ReceiptDate ?? r.TransactionDate) >= from.Value);

                    if (to.HasValue)
                        query = query.Where(r => (r.ReceiptDate ?? r.TransactionDate) <= to.Value);

                    if (!string.IsNullOrEmpty(ledgerId))
                        query = query.Where(r => r.PaymentFor == ledgerId);

                    var transactions = await query
                        .OrderBy(r => r.ReceiptDate ?? r.TransactionDate)
                        .Select(r => new
                        {
                            Date = r.ReceiptDate ?? r.TransactionDate,
                            GroupReceiptNumber = r.ReceiptNumber,
                            ReceiptNumber = r.ReceiptNumber,
                            Amount = r.Amount
                        })
                        .ToListAsync();

                    // 🔷 HEADER TABLE (LOGO + TEXT)
                    Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 4 }))
                        .UseAllAvailableWidth();

                    string logoPath = Path.Combine(_environment.WebRootPath, "Upload", "Logo", "1", "2", "2", "GMC-logo.png");

                    Cell logoCell;

                    if (System.IO.File.Exists(logoPath))
                    {
                        ImageData imageData = ImageDataFactory.Create(logoPath);
                        Image logo = new Image(imageData)
                            .ScaleAbsolute(50f, 50f)
                            .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                        logoCell = new Cell()
                            .Add(logo)
                            .SetBorder(Border.NO_BORDER)
                            .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    }
                    else
                    {
                        logoCell = new Cell().SetBorder(Border.NO_BORDER);
                    }

                    headerTable.AddCell(logoCell);

                    // 🔷 TEXT CELL
                    Cell textCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);

                    textCell.Add(new Paragraph("GOA MEDICAL COUNCIL")
                        .SetFont(bold)
                        .SetFontSize(13)
                        .SetTextAlignment(TextAlignment.CENTER));

                    textCell.Add(new Paragraph("IInd Floor, Faculty Block, G.M.C. Complex, Bambolim, Goa - 403202")
                        .SetFont(normal)
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER));

                    textCell.Add(new Paragraph("Email : goamedcouncil@rediffmail.com | Phone : 9975208199")
                        .SetFont(normal)
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER));

                    headerTable.AddCell(textCell);

                    document.Add(headerTable);

                    // 🔷 TITLE
                    document.Add(new Paragraph("DayBook Report")
                        .SetFont(bold)
                        .SetFontSize(11)
                        .SetFontColor(ColorConstants.BLUE)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(5));

                    // 🔷 SUBTITLE
                    document.Add(new Paragraph(
                        $"GMC Cash book, Ledger Report for Financial Year {financialYear} and The Period From: {fromDate} To {toDate}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(normal)
                        .SetFontSize(9)
                        .SetMarginBottom(10));

                    // 🔷 TABLE (NO INNER GRID)
                    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 2, 1 }))
                        .UseAllAvailableWidth();

                    table.SetBorder(Border.NO_BORDER);

                    // HEADER ROW
                    string[] headers = { "Date", "Group Receipt Number", "Receipt Number", "Amount" };

                    foreach (var h in headers)
                    {
                        table.AddHeaderCell(new Cell()
                            .Add(new Paragraph(h).SetFont(bold))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBorderTop(new SolidBorder(1))
                            .SetBorderBottom(new SolidBorder(1))
                            .SetBorderLeft(Border.NO_BORDER)
                            .SetBorderRight(Border.NO_BORDER));
                    }

                    // DATA
                    foreach (var item in transactions)
                    {
                        table.AddCell(new Cell()
                            .Add(new Paragraph(item.Date?.ToString("dd/MM/yyyy")))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell()
                            .Add(new Paragraph(item.GroupReceiptNumber))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell()
                            .Add(new Paragraph(item.ReceiptNumber))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell()
                            .Add(new Paragraph(item.Amount ?? ""))
                            .SetTextAlignment(TextAlignment.RIGHT)
                            .SetBorder(Border.NO_BORDER));
                    }

                    document.Add(table);

                    document.Close();

                    return File(stream.ToArray(), "application/pdf", "LedgerReport.pdf");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]

        [HttpGet("GeneraterenewalReport")]
        public async Task<IActionResult> GeneraterenewalReport(
          int? financialYearId,
         string? ledgerId,
          string? fromDate,
          string? toDate)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(stream);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);
                    document.SetMargins(20, 40, 20, 40);

                    PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    PdfFont normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                    // 🔷 Financial Year
                    var financialYear = await _context.FinancialYearMaster
                        .Where(f => f.FinYearrId == financialYearId)
                        .Select(f => f.FinancialYear)
                        .FirstOrDefaultAsync();

                    var query = _context.RenewalHistory.AsNoTracking().AsQueryable();

                    // 🔥 Date Filter
                    DateTime? from = null;
                    DateTime? to = null;

                    if (!string.IsNullOrEmpty(fromDate) && DateTime.TryParse(fromDate, out var fd))
                        from = fd.Date;

                    if (!string.IsNullOrEmpty(toDate) && DateTime.TryParse(toDate, out var td))
                        to = td.Date.AddDays(1).AddTicks(-1);

                    if (from.HasValue)
                        query = query.Where(r => (r.ReceiptDate ?? r.TransactionDate) >= from.Value);

                    if (to.HasValue)
                        query = query.Where(r => (r.ReceiptDate ?? r.TransactionDate) <= to.Value);

                    if (!string.IsNullOrEmpty(ledgerId))
                        query = query.Where(r => r.PaymentFor == ledgerId);

                    // 🔥 JOIN DATA
                    var transactions = await (
                        from r in query

                        join p in _context.Practitioners
                            on r.PractitionerID equals p.PractitionerID into pr
                        from p in pr.DefaultIfEmpty()

                        join u in _context.Users
                            on p.PractitionerID equals u.userId into ur
                        from u in ur.DefaultIfEmpty()

                        join f in _context.FeesReceipt
                            on r.ReceiptNumber equals f.ReceiptNumber into fr
                        from f in fr.DefaultIfEmpty()



                        orderby (r.ReceiptDate ?? r.TransactionDate)

                        select new
                        {
                            RegNo = p != null ? p.RegistrationNo ?? "" : "",
                            Name = u != null ? u.Name ?? "" : "",

                            Date = r.ReceiptDate ?? r.TransactionDate,
                            GroupReceiptNumber = r.ReceiptNumber ?? "",
                            ReceiptNumber = r.ReceiptNumber ?? "",

                            Amount = r.Amount,
                            Remitted = f != null ? (f.Remited_NotRemited ?? "No") : "No",
                            RemittedDate = f != null ? f.Remitted_Date : (DateTime?)null,
                            Narration = f != null ? f.Remarks ?? "" : ""
                        }
                    ).ToListAsync();

                    // 🔷 HEADER TABLE (LOGO + TEXT)
                    Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 4 }))
                        .UseAllAvailableWidth();

                    string logoPath = Path.Combine(_environment.WebRootPath, "Upload", "Logo", "1", "2", "2", "GMC-logo.png");

                    Cell logoCell;

                    if (System.IO.File.Exists(logoPath))
                    {
                        ImageData imageData = ImageDataFactory.Create(logoPath);
                        Image logo = new Image(imageData)
                            .ScaleAbsolute(50f, 50f)
                            .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                        logoCell = new Cell()
                            .Add(logo)
                            .SetBorder(Border.NO_BORDER)
                            .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    }
                    else
                    {
                        logoCell = new Cell().SetBorder(Border.NO_BORDER);
                    }

                    headerTable.AddCell(logoCell);

                    Cell textCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);

                    textCell.Add(new Paragraph("GOA MEDICAL COUNCIL")
                        .SetFont(bold)
                        .SetFontSize(13)
                        .SetTextAlignment(TextAlignment.CENTER));

                    textCell.Add(new Paragraph("IInd Floor, Faculty Block, G.M.C. Complex, Bambolim, Goa - 403202")
                        .SetFont(normal)
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER));

                    textCell.Add(new Paragraph("Email : goamedcouncil@rediffmail.com | Phone : 9975208199")
                        .SetFont(normal)
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER));

                    headerTable.AddCell(textCell);

                    document.Add(headerTable);

                    // 🔷 TITLE
                    document.Add(new Paragraph("Renewal Report")
                        .SetFont(bold)
                        .SetFontSize(11)
                        .SetFontColor(ColorConstants.BLUE)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(5));

                    // 🔷 SUBTITLE
                    document.Add(new Paragraph(
                        $"GMC Cash book, Ledger Report for Financial Year {financialYear ?? ""} and The Period From: {fromDate} To {toDate}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(normal)
                        .SetFontSize(9)
                        .SetMarginBottom(10));

                    // 🔷 TABLE
                    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 2, 2, 2, 1, 1, 2, 2 }))
                        .UseAllAvailableWidth();

                    table.SetBorder(Border.NO_BORDER);

                    string[] headers =
                    {
                 "Reg No",
                 "Name",
                 "Date",
                 "Group Receipt Number",
                 "Receipt Number",
                "Amount",
                "Remitted",
                "Remitted Date",
                "Narration"
                   };

                    foreach (var h in headers)
                    {
                        table.AddHeaderCell(new Cell()
                            .Add(new Paragraph(h).SetFont(bold))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBorderTop(new SolidBorder(1))
                            .SetBorderBottom(new SolidBorder(1))
                            .SetBorderLeft(Border.NO_BORDER)
                            .SetBorderRight(Border.NO_BORDER));
                    }

                    // 🔷 DATA
                    foreach (var item in transactions)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(item.RegNo))
                            .SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell().Add(new Paragraph(item.Name))
                            .SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell().Add(new Paragraph(item.Date?.ToString("dd/MM/yyyy") ?? ""))
                            .SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell().Add(new Paragraph(item.GroupReceiptNumber))
                            .SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell().Add(new Paragraph(item.ReceiptNumber))
                            .SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell().Add(new Paragraph(item.Amount != null ? item.Amount.ToString() : ""))
                            .SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell().Add(new Paragraph(item.Remitted))
                            .SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell().Add(new Paragraph(item.RemittedDate?.ToString("dd/MM/yyyy") ?? ""))
                            .SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell().Add(new Paragraph(item.Narration))
                            .SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER));
                    }

                    document.Add(table);
                    document.Close();

                    return File(stream.ToArray(), "application/pdf", "renewalReport.pdf");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [AllowAnonymous]
        [HttpGet("noc-report")]
        public async Task<IActionResult> GetNocReport(
       [FromQuery] string fromDate,
       [FromQuery] string toDate)
        {
            DateTime from = DateTime.Parse(fromDate);
            DateTime to = DateTime.Parse(toDate);

            var data = await (
                from r in _context.RenewalHistory

                join p in _context.Practitioners
                    on r.PractitionerID equals p.PractitionerID

                join l in _context.GroupLedgerMaster
                    on r.PaymentFor equals l.LedgerID

                join c in _context.JCouncilMaster
                    on p.CouncilId equals c.CouncilId into pc
                from c in pc.DefaultIfEmpty()

                where r.PaymentFor == "LED27"

                select new
                {
                    RegistrationNo = p.RegistrationNo,
                    RegistrationDate = p.RegistrationDate,
                    Type = r.Type,

                    ReceiptNumber = r.ReceiptNumber,
                    RenewalDate = r.RenewalDate,
                    Amount = r.Amount,

                    CouncilName = c.CouncilName,
                    CounsilAdress = c.Address
                }
            ).ToListAsync();

            return Ok(data);
        }



    }


}