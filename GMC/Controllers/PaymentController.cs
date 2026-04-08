
using GMC.Data;
using GMC.Model;
using GMC.Model.GMC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly AppDbContext _context;
    //live keys
    // private String username = "rzp_live_SRrMhCI27G1AyG";
    // private String password = "Huk97QdTN78bheNu1LrXTlXB";

    //test keys

    private String username = "rzp_test_SRrKcod305RxKm";
    private String password = "l9xBi4T1GLCjG0jPWnIicFfw";
    public PaymentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("CreateRenewal")]
    public async Task<IActionResult> CreateRenewal(RenewalHistoryDto model)
    {
        try
        {
            if (model == null)
                return BadRequest("Invalid data");

            // Generate Renewal ID
            string renewalId = await GeneratrenewalidAsync();
            string trnno = await GenerateTransactionNumber();
            // ✅ Generate Receipt Number correctly
            string receiptNumber = await GenerateReceiptNumberAsync();

            var user = await _context.Practitioners
              .Where(r => r.PractitionerID == model.PractitionerID)
              .Select(r => new
              {
                  Name = r.Name,
                  RegistrationNo = r.RegistrationNo
              })
              .FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest("Practitioner not found");
            }

            var remarks = "Payment for " +model.FeeItemname +
                          ", Name: " + user.Name +
                          ", RegNo: " + user.RegistrationNo +
                          ", Bank: " + model.Bank +
                          ", TransactionID: " + trnno +
                          ", Transaction Date: " + model.ReceiptDate?.ToString("dd-MM-yyyy");


            var entity = new RenewalHistory
            {
                RenewalID = renewalId,

                CountryId = "1",
                StateId = "1",
                CouncilId = "1",

                PractitionerID = model.PractitionerID,

                Type = "Off line",

                ReceiptNumber = receiptNumber,   // ✅ FIXED
                ReceiptDate = DateTime.Now,

                RenewalDate = model.RenewalDate,
                TransactionNo= trnno,

                Amount = model.Amount.ToString(),

                PaymentFor = model.PaymentFor,
                DD_ChequeDate=model.DD_ChequeDate,
                DD_ChequeNO=model.DD_ChequeNO,
                Bank=model.Bank,
                CreatedBy = model.CreatedBy,
                CreatedOn = DateTime.Now,

                Status = "A"
            };

            _context.RenewalHistory.Add(entity);

            await _context.SaveChangesAsync();


            var entity2 = new FeesReceipt
            {
                CountryId = "1",
                StateId ="1",
                CouncilId = "1",
                GroupID = model.GroupID,
                LedgerID = model.PaymentFor,               
                AccountNo = model.AccountNo,
                AutoReceiptNo = receiptNumber,
                ReceiptNumber = receiptNumber,
                ReceiptDate = model.ReceiptDate ?? DateTime.Now,
                Amount = decimal.TryParse(model.Amount, out var result2) ? result2 : 0,
                Type = "R",             
                FinancialYear = model.Financial_Year,              
                Remarks = remarks,
                CreatedBy = model.CreatedBy,
                CreatedOn = DateTime.Now
            };

            _context.FeesReceipt.Add(entity2);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Renewal created successfully",
                RenewalID = renewalId,
                ReceiptNumber = receiptNumber,
          
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    private async Task<string> GenerateReceiptNumberAsync()
    {
        // Get current financial year start
        int year = DateTime.Now.Month >= 4
            ? DateTime.Now.Year
            : DateTime.Now.Year - 1;

        string financialYear = year.ToString();

        // Get last receipt number for current financial year
        var lastReceipt = await _context.RenewalHistory
            .Where(x => x.ReceiptNumber.StartsWith(financialYear))
            .OrderByDescending(x => x.ReceiptNumber)
            .Select(x => x.ReceiptNumber)
            .FirstOrDefaultAsync();

        int nextNumber = 1;

        if (!string.IsNullOrEmpty(lastReceipt))
        {
            string lastIncrement = lastReceipt.Substring(4); // get last 5 digits
            nextNumber = int.Parse(lastIncrement) + 1;
        }

        // format: financialYear + 5 digit increment
        return financialYear + nextNumber.ToString("D5");
    }

    private async Task<string> GenerateTransactionNumber()
    {
        return "TRN-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
    }
    private async Task<string> GeneratrenewalidAsync()
    {
        string id;
        var rnd = new Random();

        do
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int random = rnd.Next(100, 999);
            id = $"R{timestamp}{random}";

        } while (await _context.CaseWorkers.AnyAsync(x => x.CWUserId == id));

        return id;
    }




   
    [HttpGet("paymentdetails")]
    public async Task<IActionResult> districtmaster(string pid)
    {
        var paymentDetails = await (
    from renewal in _context.RenewalHistory

    

    join fee in _context.GroupLedgerMaster
         on renewal.PaymentFor equals fee.LedgerID.ToString()

    join receipt in _context.FeesReceipt
        on renewal.ReceiptNumber equals receipt.ReceiptNumber

    join practitioner in _context.Practitioners
        on renewal.PractitionerID equals practitioner.PractitionerID

    where renewal.PractitionerID == pid
    orderby renewal.CreatedOn descending

    select new
    {
        fee.LedgerDescription,
        renewal.Type,
        renewal.ReceiptNumber,
        renewal.ReceiptDate,
        renewal.TransactionNo,
        renewal.TransactionDate,
        renewal.Bank,
        renewal.Amount,
        renewal.RenewalID
    }).ToListAsync();




        return Ok(paymentDetails);
    }


    [AllowAnonymous]
    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder(int amount, string pid, string paymentfor,string? councilid)
    {
        RazorpayClient client = new RazorpayClient(username, password);

        Dictionary<string, object> options = new Dictionary<string, object>();
        options.Add("amount", amount * 100);
        options.Add("currency", "INR");
        options.Add("receipt", Guid.NewGuid().ToString());

        Order order = client.Order.Create(options);

        var model = new RazorPayPaymentAttempt
        {
            CountryId = "1",
            StateId = "1",
            CouncilId = "1",
            PractitionerId = pid,
            Order_id = order["id"].ToString(),
            Amount = amount,
            Currency = "INR",
            Order_status = "CREATED",
            LedgerId = paymentfor,
            CreatedOn = DateTime.Now,
            CouncilName= councilid
        };

        _context.RazorPayPaymentAttempt.Add(model);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            orderId = order["id"].ToString(),
            amount = amount * 100,
            currency = "INR"
        });
    }



    private string GetNextReferenceNumber(string refNumber)
    {
        if (string.IsNullOrEmpty(refNumber))
            throw new ArgumentException("Reference number cannot be null");

        var parts = refNumber.Split('/');

        if (parts.Length < 4)
            throw new ArgumentException("Invalid reference number format");

        // Last part = running number
        var lastPart = parts[^1];

        if (!int.TryParse(lastPart, out int number))
            throw new ArgumentException("Invalid numeric part in reference number");

        int nextNumber = number + 1;

        // Keep same padding (0001 → 0002)
        string incremented = nextNumber.ToString(new string('0', lastPart.Length));

        parts[^1] = incremented;

        return string.Join("/", parts);
    }








    [AllowAnonymous]
    [HttpPost("Webhook")]
    public async Task<IActionResult> Webhook()
    {
        string body = await new StreamReader(Request.Body).ReadToEndAsync();

        string signature = Request.Headers["X-Razorpay-Signature"];

        bool isValid = VerifyWebhookSignature(body, signature, password);

        if (!isValid)
        {
            return BadRequest("Invalid signature");
        }

        var json = JObject.Parse(body);

        string eventType = json["event"]?.ToString();

        if (eventType == "payment.captured")
        {
            string paymentId = json["payload"]["payment"]["entity"]["id"]?.ToString();
            string orderId = json["payload"]["payment"]["entity"]["order_id"]?.ToString();
           // string amount = json["payload"]["payment"]["entity"]["amount"]?.ToString();


            var paymentAttempt = await _context.RazorPayPaymentAttempt
                    .Where(x => x.Order_id == orderId)
                    .FirstOrDefaultAsync();
            if (paymentAttempt == null)
            {
                return BadRequest("Payment attempt not found");
            }


            var paymentfor = await _context.GroupLedgerMaster
                     .Where(x => x.LedgerID == paymentAttempt.LedgerId)
                     .FirstOrDefaultAsync();

            string genrefno = "";

            if (paymentAttempt.LedgerId== "LED27"|| paymentAttempt.LedgerId == "LED35")
            {

                var lastRef = await _context.RenewalHistory
                                .Where(x => x.RefNo != null)
                                .OrderByDescending(x => x.CreatedOn)   // ✅ latest record
                                .Select(x => x.RefNo)
                                .FirstOrDefaultAsync();

                genrefno = GetNextReferenceNumber(lastRef);
            }
          

                try
                {

                    // Generate Renewal ID
                    string renewalId = await GeneratrenewalidAsync();

                    // ✅ Generate Receipt Number correctly
                    string receiptNumber = await GenerateReceiptNumberAsync();

                    var user = await _context.Practitioners
                      .Where(r => r.PractitionerID == paymentAttempt.PractitionerId)
                      .Select(r => new
                      {
                          Name = r.Name,
                          RegistrationNo = r.RegistrationNo
                      })
                      .FirstOrDefaultAsync();

                    if (user == null)
                    {
                        return BadRequest("Practitioner not found");
                    }

                    var remarks = "Payment for " + paymentfor.LedgerDescription +
                                  ", Name: " + user.Name +
                                  ", RegNo: " + user.RegistrationNo +
                                  ", Bank: " + "ACC" +
                                  ", TransactionID: " + paymentId +
                                  ", Transaction Date: " + DateTime.Now.ToString("dd-MM-yyyy");


                    var entity = new RenewalHistory
                    {
                        RenewalID = renewalId,

                        CountryId = "1",
                        StateId = "1",
                        CouncilId = "1",

                        PractitionerID = paymentAttempt.PractitionerId,

                        Type = "Online",

                        ReceiptNumber = receiptNumber,   // ✅ FIXED
                        ReceiptDate = DateTime.Now,

                        RenewalDate = DateTime.Now,
                        TransactionNo = paymentId,

                        Amount = paymentAttempt.Amount.ToString(),

                        PaymentFor = paymentAttempt.LedgerId,
                        RefNo= genrefno,
                        Bank = "ACC",
                        CreatedBy = "WEBHOOK",
                        CreatedOn = DateTime.Now,

                        Status = "A"
                    };

                    _context.RenewalHistory.Add(entity);

                    await _context.SaveChangesAsync();


                    var entity2 = new FeesReceipt
                    {
                        CountryId = "1",
                        StateId = "1",
                        CouncilId = "1",

                        LedgerID = paymentAttempt.LedgerId,
                        AccountNo = "Acc1",
                        AutoReceiptNo = receiptNumber,
                        ReceiptNumber = receiptNumber,
                        ReceiptDate = DateTime.Now,
                        Amount = decimal.TryParse(paymentAttempt.Amount.ToString(), out var result2) ? result2 : 0,
                        Type = "R",

                        Remarks = remarks,
                        CreatedBy = "webhook",
                        CreatedOn = DateTime.Now
                    };

                    _context.FeesReceipt.Add(entity2);
                    await _context.SaveChangesAsync();


                    paymentAttempt.Pay_id = paymentId;
                    paymentAttempt.ReceiptNo = receiptNumber;
                    paymentAttempt.UpdatedOn = DateTime.Now;
                    paymentAttempt.UpdatedBy = "Webhook";
                    paymentAttempt.Order_status = "Captured";
                    paymentAttempt.UpdatedOn = DateTime.Now;

                    // Save update
                    _context.RazorPayPaymentAttempt.Update(paymentAttempt);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "Renewal created successfully",
                        RenewalID = renewalId,
                        ReceiptNumber = receiptNumber,

                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }

            // TODO: Update payment status in DB
        }

        return Ok();
    }

    private bool VerifyWebhookSignature(string payload, string signature, string secret)
    {
        var secretBytes = Encoding.UTF8.GetBytes(secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);

        using (var hmac = new HMACSHA256(secretBytes))
        {
            var hash = hmac.ComputeHash(payloadBytes);
            var generatedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return true;//generatedSignature == signature;
        }
    }



    [AllowAnonymous]
    [HttpGet("cashbooksearch")]
    public async Task<IActionResult> cashbooksearch(string? receiptNumber)
    {
        var query =
            from renewal in _context.RenewalHistory

            join fee in _context.GroupLedgerMaster
                on renewal.PaymentFor equals fee.LedgerID.ToString()

            join receipt in _context.FeesReceipt
                on renewal.ReceiptNumber equals receipt.ReceiptNumber

            join feelink in _context.LedgerFeeItemLink
                on renewal.PaymentFor equals feelink.LedgerID.ToString()

            join feeitm in _context.FeesItems
                on feelink.FeeItemID equals feeitm.FeeItemId.ToString()

            join practitioner in _context.Practitioners
                on renewal.PractitionerID equals practitioner.PractitionerID

            select new
            {
                feeitm.FeeItemName,
                fee.LedgerDescription,
                receipt.FinancialYear,
                receipt.AccountNo,
                receipt.Remarks,
                renewal.Type,
                renewal.ReceiptNumber,
                renewal.ReceiptDate,
                renewal.TransactionNo,
                renewal.TransactionDate,
                renewal.Bank,
                renewal.Amount,
                renewal.RenewalID,
                renewal.CreatedBy,
                renewal.UpdatedBy,
                renewal.CreatedOn
            };

     
        // 🔥 Special cases for receiptNumber
        if (!string.IsNullOrEmpty(receiptNumber))
        {
            if (receiptNumber.ToLower() == "first")
            {
                var firstData = await query
                    .OrderBy(x => x.CreatedOn) // oldest
                    .Take(1)
                    .ToListAsync();

                return Ok(firstData);
            }
            else if (receiptNumber.ToLower() == "last")
            {
                var lastData = await query
                    .OrderByDescending(x => x.CreatedOn) // latest
                    .Take(1)
                    .ToListAsync();

                return Ok(lastData);
            }
            else
            {
                // Normal receipt filter
                query = query.Where(x => x.ReceiptNumber == receiptNumber);
            }
        }

        // 📌 If NO filters → return TOP 1 latest
        if (string.IsNullOrEmpty(receiptNumber))
        {
            var topData = await query
                .OrderByDescending(x => x.CreatedOn)
                .Take(1)
                .ToListAsync();

            return Ok(topData);
        }

        // 📌 Return filtered data
        var filteredData = await query
            .OrderByDescending(x => x.CreatedOn)
            .ToListAsync();

        return Ok(filteredData);
    }


}
