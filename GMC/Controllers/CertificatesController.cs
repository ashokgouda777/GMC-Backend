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
                               p.Gender
                           }).FirstOrDefault();

            if (payment == null)
                return NotFound("Receipt not found");










            if (payment.PaymentFor == "LED5")


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
                                education.Select(x => $"{x.EducationName}"+"("+$"{x.CourseDescription}"+") ("+x.UniversityName+") " + GetMonthName(Convert.ToInt32(x.MonthOfPassing))+","+x.YearOfPassing));

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


                    /*    document.ShowTextAligned(
                            new Paragraph(qualification?.ToUpper() ?? "")
                                .SetFont(font)
                                .SetFontSize(11)
                                .SetFontColor(ColorConstants.BLACK),
                            100, 430,
                            TextAlignment.LEFT
                        );*/

                    /*  var qualificationText = qualification?.ToUpper() ?? "";

                      var para = new Paragraph(qualificationText)
                          .SetFont(font)
                          .SetFontSize(11)
                          .SetFontColor(ColorConstants.BLACK)
                          .SetMultipliedLeading(2.0f) // line spacing
                           .SetFirstLineIndent(260);
                      para.SetFixedPosition(
                          100,   // X (start position)
                          425,   // Y (adjust slightly below your label)
                          410    // WIDTH → IMPORTANT for wrapping
                      );

                      document.Add(para);*/

                    float[] startX = { 340, 100, 100 };
                    float[] endX = { 510, 510, 510 };
                    float[] startY = { 460, 430, 402 };

                    string text = qualification?.ToUpper() ?? "";
                    string remaining = text;

                    for (int i = 0; i < 3; i++)
                    {
                        if (string.IsNullOrEmpty(remaining)) break;

                        float maxWidth = endX[i] - startX[i];

                        int cutIndex = remaining.Length;

                        // Find how much text fits in this line
                        while (cutIndex > 0 &&
                               font.GetWidth(remaining.Substring(0, cutIndex), 11) > maxWidth)
                        {
                            cutIndex--;
                        }

                        if (cutIndex == 0) cutIndex = 1;


                        string fittedText = remaining.Substring(0, cutIndex);

                        // Draw this part
                        document.ShowTextAligned(
                            new Paragraph(fittedText)
                                .SetFont(font)
                                .SetFontSize(11)
                                .SetFontColor(ColorConstants.BLACK),
                            startX[i],
                            startY[i],
                            TextAlignment.LEFT
                        );

                        // Remaining text goes to next line
                        remaining = remaining.Substring(cutIndex).TrimStart();
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

                    // SIGNATURE
                    if (!string.IsNullOrEmpty(practitioner.practsign))
                    {
                        string signPath = Path.Combine(
                            _environment.WebRootPath,
                            "Upload",
                            "PractitionerPhotos",
                            "1", "2", "2",
                            practitioner.practsign
                        );

                        if (System.IO.File.Exists(signPath))
                        {
                            ImageData imgData = ImageDataFactory.Create(signPath);

                            var img = new Image(imgData)
                                .ScaleAbsolute(182f, 31f)
                                .SetFixedPosition(153f, 511f);

                            document.Add(img);
                        }
                    }


                    document.Close();

                    return File(ms.ToArray(), "application/pdf", "Certificate.pdf");
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

                string qualification = string.Join(", ", education.Select(e => e.EducationName));

                string templatePath = Path.Combine(_environment.WebRootPath, "Upload", "certificates", "NOC Format.pdf");

                string genderWord = practitioner.Gender != null && practitioner.Gender.ToString() == "2" ? "her" : "his";

                string prefix = "/" + DateTime.Now.ToString("MM/yyyy") + "/";
                string referenceNo = prefix + counter.ToString("D3");
                counter++;

                using (MemoryStream ms = new MemoryStream())
                {
                    var reader = new PdfReader(templatePath);
                    var writer = new PdfWriter(ms);
                    var pdf = new PdfDocument(reader, writer);
                    var document = new Document(pdf);

                    PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                    // Get page size for grid
                    var pageSize = pdf.GetFirstPage().GetPageSize();
                    float pageWidth = pageSize.GetWidth();
                    float pageHeight = pageSize.GetHeight();

                    document.ShowTextAligned(
                        new Paragraph(practitioner.RegistrationNo ?? "")
                        .SetFont(font).SetFontSize(10),
                        320, 450, 1,
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
                        150, 470, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(qualification?.ToUpper() ?? "")
                        .SetFont(font)
                        .SetFontSize(10),
                        280, 470, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(practitioner.RegistrationDate?.ToString("dd MMM yyyy") ?? "")
                        .SetFont(font)
                        .SetFontSize(9),
                        380, 450, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                        new Paragraph(DateTime.Now.ToString("dd MMM yyyy"))
                        .SetFont(font)
                        .SetFontSize(11),
                        386, 647, 1,
                        TextAlignment.LEFT,
                        VerticalAlignment.BOTTOM,
                        0
                    );

                    document.ShowTextAligned(
                      new Paragraph(referenceNo)
                      .SetFont(font)
                      .SetFontSize(11),
                      172, 647, 1,
                      TextAlignment.LEFT,
                      VerticalAlignment.BOTTOM,
                      0
                    );
                    document.ShowTextAligned(
                     new Paragraph(genderWord)
                    .SetFont(normalFont)
                    .SetFontSize(10),
                    176, 346, 1,
                    TextAlignment.LEFT,
                    VerticalAlignment.BOTTOM,
                     0
                   );

                    document.ShowTextAligned(
                     new Paragraph(genderWord)
                    .SetFont(normalFont)
                    .SetFontSize(10),
                    438, 450, 1,
                    TextAlignment.LEFT,
                    VerticalAlignment.BOTTOM,
                     0
                    );

                    document.ShowTextAligned(
                     new Paragraph(genderWord)
                    .SetFont(normalFont)
                    .SetFontSize(9),
                    225, 450, 1,
                    TextAlignment.LEFT,
                    VerticalAlignment.BOTTOM,
                     0
                    );



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

                string qualification = string.Join(", ", education.Select(e => e.Subject));

                string templatePath = Path.Combine(_environment.WebRootPath,
                    "Upload", "certificates", "Certificate of Good Standing format.pdf");
                // Get gender word
                string genderWord = practitioner.Gender != null && practitioner.Gender.ToString() == "2" ? "her" : "his";


                string prefix = "/" + DateTime.Now.ToString("MM/yyyy") + "/";



                string referenceNo = prefix + counter.ToString("D3");

                counter++;


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
                                   r.Bank
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
    }

}