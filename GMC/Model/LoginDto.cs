using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GMC.Model
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class CaseWorkerCreateDto
    {
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CouncilId { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public string CreatedBy { get; set; }
    }
    public class CaseWorkerListDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string RoleName { get; set; }
        public bool Active { get; set; }
    }



    public class PractitionerCreate
    {
        public string? CountryId { get; set; }
        public string? StateId { get; set; }
        public string? CouncilId { get; set; }
        public string? RegistrationNo { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? RegistrationType { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public string? ChangeOfName { get; set; }
        public string? SpouseName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirthPlace { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? Vote { get; set; }
        public string? EmailID { get; set; }
        public string? MobileNumber { get; set; }
        public string? BloodGroup { get; set; }
        public string? CreatedBy { get; set; }
        public string? RegistrationFor { get; set; }

        public string Status { get; set; }
    }

    public class AddressDto
    {
        public string ClientID { get; set; }
        public string AddressType { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public string? phone1 { get; set; }
        public string? phone2 { get; set; }
        public string? PlaceType { get; set; }
        public string? CreatedBy { get; set; }
    }





    public class AddEducationInfo
    {
        public string EducationID { get; set; }
        public string PractitionerID { get; set; }
        public string? EducationName { get; set; }
        public string? YearOfPassing { get; set; }
        public string? CollegeID { get; set; }
        public string? UniversityId { get; set; }
        public string? Subject { get; set; }
        public string? MonthOfPassing { get; set; }
        public string? SubCode { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? CompletedAt { get; set; }
        public string? CertificateNo { get; set; }
        public DateTime? CertificateDate { get; set; }
        public string? ADSerialno { get; set; }
    }



    public class RenewalHistoryDto
    {


        public string PractitionerID { get; set; }

        public string? Type { get; set; }

        public string? ReceiptNumber { get; set; }

        public string? GroupID { get; set; }
        public string? AccountNo { get; set; }
        public string? Financial_Year { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string? FeeItemname { get; set; }
        public DateTime? RenewalDate { get; set; }
        public DateTime? ValidUpto { get; set; }

        public string? Amount { get; set; }
        public string PaymentFor { get; set; }
        public string? Bank { get; set; }          // ✅ ADD
        public string? DD_ChequeNO { get; set; }   // ✅ ADD
        public DateTime? DD_ChequeDate { get; set; } // ✅ ADD

        public string? CreatedBy { get; set; }
    }


    public class FeesReceiptCreateDto
    {

        public string? GroupID { get; set; }
        public string? LedgerID { get; set; }
        public string? FeeTmplID { get; set; }
        public string? AccountNo { get; set; }
        public decimal? ReceiptNumber { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public decimal? Amount { get; set; }
        public string? Type { get; set; }
        public string? CourseCode { get; set; }
        public string? FinancialYear { get; set; }
        public decimal? Opening_Balance { get; set; }
        public decimal? Closing_Balance { get; set; }
        public string? Remarks { get; set; }
        public string? Remited_NotRemited { get; set; }
        public string? Contra_Remarks { get; set; }
        public DateTime? Remitted_Date { get; set; }
        public string? Course_Category { get; set; }
        public DateTime? NextPymntDate { get; set; }
        public decimal? SubReceiptNo { get; set; }
        public int? receipt { get; set; }
        public string? ipaddress { get; set; }
        public string? ProgramId { get; set; }
        public decimal? TAX_Amnt { get; set; }
    }
    public class SignatureRequest
    {
        public string Base64Signature { get; set; }
    }


    public class RazorPayPaymentAttemptDto
    {

        public string PractitionerId { get; set; }
        public string Order_id { get; set; }
        public string Pay_id { get; set; }
        public string ReceiptNo { get; set; }
        public string Order_status { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public string LedgerId { get; set; }
        public string FinYear { get; set; }
        public string CouncilName { get; set; }
        public string CouncilAddress { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string DummyReceiptNo { get; set; }
        public string CreatedBy { get; set; }
        public string Pids { get; set; }
        public string ProgramId { get; set; }
        public string ProgramName { get; set; }
    }


    public class PractitionerViewModel
    {
        public string RegistrationNo { get; set; }
        public string RegistrationFor { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BloodGroup { get; set; }
        public string BirthPlace { get; set; }
        public string Nationality { get; set; }
        public string MobileNumber { get; set; }
        public string EmailID { get; set; }

        public string PermanentAddress { get; set; }
        public string ProfessionalAddress { get; set; }

        public string Photo { get; set; }
        public string Sign { get; set; }
        public string Thumb { get; set; }
        public string Barcode { get; set; }
    }
}
