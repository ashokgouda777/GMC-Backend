namespace GMC.Model
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace GMC.Models
    {
        [Table("tbl_Users")]
        public class User
        {
            [Key]
            public int Id { get; set; }   // EF needs primary key (SQL script has none)

            public string? CountryId { get; set; }
            public string? StateId { get; set; }
            public string? CouncilId { get; set; }
            public int? Site_Id { get; set; }

            [Required]
            [MaxLength(50)]
            public string UserName { get; set; } = null!;

            public string? Password { get; set; }
            public string? Status { get; set; }
            public string? Name { get; set; }
            public string? Role_Id { get; set; }
            public string? Email_Id { get; set; }
            public string? MobileNo { get; set; }

            public DateTime? CreatedOn { get; set; }
            public string? CreatedBy { get; set; }
            public string? UpdatedBy { get; set; }
            public DateTime? UpdatedOn { get; set; }

            public string? District { get; set; }
            public string? Photo { get; set; }
            public int? usercount { get; set; }
            public string? Firsttime_login { get; set; }
            public string? userId { get; set; }
            public string? RegType { get; set; }
            public string? typeUser { get; set; }
            public string? ipaddress { get; set; }
        }
    }
    [Table("tbl_practitioner")]
    public class Practitioner
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string PractitionerID { get; set; } = null!;

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
        public string? LoginID { get; set; }
        public string? Password { get; set; }

        public string? PhotoPath { get; set; }
        public string? FirstLogin { get; set; }
        public string? photo { get; set; }
        public string? practsign { get; set; }
        public string? thumb { get; set; }
        public string? barcode { get; set; }
        public string? declaration { get; set; }
        public string? Revwr_Editr_YN { get; set; }
        public string? status { get; set; }
        public string? BloodGroup { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string? Supplementary { get; set; }
        public string? RegistrationFor { get; set; }
        public string? PrintStatus { get; set; }
        public string? Class { get; set; }
        public string? Registerstatus { get; set; }
        public string? RefreshRegistration { get; set; }
        public string? PaymentStatus { get; set; }
        public string? Synch { get; set; }
        public string? PRC_payment_ReceiptNO { get; set; }
        public string? IsOnline { get; set; }

        public string? tem_regNo { get; set; }
        public string? tem_practitionerId { get; set; }
        public string? cme_photo { get; set; }

        public string? FromStatecode { get; set; }
        public string? ToStateCode { get; set; }
        public string? IsInliue { get; set; }
        public string? YearofRegistration { get; set; }
        public string? HaveUserNamePassword { get; set; }

        public string? TR_MBBS { get; set; }
        public string? TR_Non_MBBS { get; set; }
        public string? TR_SuperSpeciality { get; set; }

        public string? PrcNoArchive { get; set; }
        public string? deleteyn { get; set; }
        public string? deletedby { get; set; }

        public string? Approveby { get; set; }
        public DateTime? ApproveOn { get; set; }
        public string? Approve1 { get; set; }
        public DateTime? Approve1date { get; set; }

        public string? Address { get; set; }
        public string? photo1 { get; set; }
        public string? photo2 { get; set; }
        public string? Proffession { get; set; }
        public string? ipaddress { get; set; }

        public string? Obj_Ren { get; set; }
        public string? CME_Renw { get; set; }
    }

    [Table("tbl_Role_Master")]
    public class RoleMaster
    {
        [Key]
        public int Id { get; set; }  // EF needs primary key

        public string? CountryId { get; set; }
        public string? StateId { get; set; }
        public string? CouncilId { get; set; }
        public string? RoleId { get; set; }
        public string? RoleDesc { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }


    [Table("tbl_case_workers")]
    public class CaseWorker
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        public int CouncilId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        
        public string CWUserId { get; set; }

        [Required]
        [MaxLength(15)]
        public string MobileNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string EmailId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        public int RoleId { get; set; }

        public bool Active { get; set; } = true;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? UpdatedBy { get; set; }
    }


    [Table("tbl_RegistrationForMaster")]
    public class RegistrationForMaster
    {
        [Key]
        public int Id { get; set; }
        public string? CountryId { get; set; }
        public string? StateId { get; set; }
        public string? CouncilId { get; set; }
        public string? RegId { get; set; }
        public string? RegForName { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }


    [Table("tbl_title_master")]
    public class TitleMaster
    {
        [Key]
        public int TitleId { get; set; }

        [Column("TitleName")]
        [StringLength(100)]
        public string? TitleName { get; set; }

        public bool IsActive { get; set; } = true;

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }

    [Table("tbl_GenderMaster")]
    public class GenderMaster
    {
        public string? CountryId { get; set; }

        public string? StateId { get; set; }

        public string? CouncilId { get; set; }

        [Key] // Assuming GenderId is unique
        public string? GenderId { get; set; }

        public string? GenderName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? UpdatedBy { get; set; }
    }

    [Table("tbl_blood_group_type_master")]
    public class BloodGroupTypeMaster
    {
        public string? CountryId { get; set; }

        public string? StateId { get; set; }

        public string? CouncilId { get; set; }

        [Key] // Assuming BloodGroupId is unique
        public int? BloodGroupId { get; set; }

        public string? BloodGroupCode { get; set; }

        public string? BloodGroupDescription { get; set; }

        public string? BloodGroupDetails { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? Active { get; set; }
    }




[Table("tbl_masterNationality")]
    public class NationalityMaster
    {
        public string? CountryId { get; set; }

        public string? StateId { get; set; }

        public string? CouncilId { get; set; }

        [Key]   // ⭐ MUST BE PRESENT
        [Column(TypeName = "nvarchar(50)")]
        public string NationalityId { get; set; } = Guid.NewGuid().ToString();

        public string? Nationality { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? Status { get; set; }
    }


    [Keyless]
    [Table("tbl_PractitionerEligiblity")]
    public class PractitionerEligiblity
    {
        public string? CountryId { get; set; }
        public string? StateId { get; set; }
        public string? CouncilId { get; set; }
        public int? EligibiltyId { get; set; }
        public string? Eligibilty { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }



[Table("tbl_J_Country_Master")]
    public class CountryMaster
    {
        [Key]
        public int id { get;set; }
        public string CountryId { get; set; }

        public string? CountryName { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? Active { get; set; }
    }




[Table("tbl_wesiteconfig")]
    public class SiteSettings
    {
        [Key]
        public string UserId { get; set; }

        [MaxLength(20)]
        public string PrimaryColor { get; set; }

        [MaxLength(20)]
        public string SecondaryColor { get; set; }

        [MaxLength(20)]
        public string TextColor { get; set; }

        [MaxLength(20)]
        public string BackgroundColor { get; set; }

        [MaxLength(20)]
        public string ButtonColor { get; set; }

        // Navigation & Sidebar
        [MaxLength(20)]
        public string SidebarColor { get; set; }

        [MaxLength(20)]
        public string SidebarText { get; set; }

        [MaxLength(20)]
        public string SidebarActiveColor { get; set; }

        [MaxLength(20)]
        public string TopbarBg { get; set; }

        [MaxLength(20)]
        public string NavbarBg { get; set; }

        [MaxLength(20)]
        public string NavbarText { get; set; }

        // Content & Tables
        [MaxLength(20)]
        public string CardBg { get; set; }

        [MaxLength(20)]
        public string TableHeaderBg { get; set; }

        [MaxLength(20)]
        public string TableHeaderText { get; set; }

        [MaxLength(20)]
        public string TableBodyBg { get; set; }

        [MaxLength(20)]
        public string TableBodyText { get; set; }

        [MaxLength(20)]
        public string TableBtnColor { get; set; }
    }

[Table("tbl_address")]
    public class Address
    {
        [Column(TypeName = "nvarchar(50)")]
        public string? CountryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? StateId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CouncilId { get; set; }

        [Key]
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string AddressID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string ClientID { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string AddressType { get; set; } = "R";   // Default Value

        [Column(TypeName = "varchar(MAX)")]
        public string? Address1 { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Address2 { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? City { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? State { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Zip { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Country { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string? Phone1 { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string? Phone2 { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? PlaceType { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? District { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }



    [Table("tbl_J_State_Master")]
    public class StateMaster
    {
        [Key] // ⚠ If composite key, configure in DbContext (see below)
        [Column("StateId", TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string StateId { get; set; }

        [Column("CountryId", TypeName = "nvarchar(50)")]
        [Required]
        [StringLength(50)]
        public string CountryId { get; set; }

        [Column("StateName", TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string? StateName { get; set; }

        [Column("CreatedBy", TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string? CreatedBy { get; set; }

        [Column("CreatedOn")]
        public DateTime? CreatedOn { get; set; }

        [Column("UpdatedBy", TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string? UpdatedBy { get; set; }

        [Column("UpdatedOn")]
        public DateTime? UpdatedOn { get; set; }

        [Column("Active", TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string? Active { get; set; }
    }




[Table("tbl_district_master")]
    public class DistrictMaster
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CountryId { get; set; }

        [Column(Order = 1)]
        [StringLength(50)]
        public string StateId { get; set; }

        [Column(Order = 2)]
        [StringLength(50)]
        public string CouncilId { get; set; }

        [Column(Order = 3)]
        [StringLength(50)]
        public string DistrictId { get; set; }

        [StringLength(50)]
        public string? DistrictName { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(50)]
        public string? State { get; set; }

        public int? Districtcount { get; set; }

        [StringLength(25)]
        public string? AppYN { get; set; }

        [StringLength(50)]
        public string? AppCenter { get; set; }
    }

[Table("tbl_education_info")]
    public class EducationInfo
    {
        public string? CountryId { get; set; }

        public string? StateId { get; set; }

        public string? CouncilId { get; set; }

        [Key]
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string EducationID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string PractitionerID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? EducationName { get; set; }

        [Column(TypeName = "varchar(4)")]
        public string? YearOfPassing { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CollegeID { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UniversityId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Subject { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? TempYOP { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string? TempCollege { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? MonthOfPassing { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string? TempUniversity { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string? TempEducationName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? SubCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CompletedAt { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string? CouncilCode { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? CertificateNo { get; set; }

        public DateTime? CertificateDate { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? ADSerialno { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string? IsIssued { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LedgerVerified { get; set; }
    }


[Table("tbl_CourseMaster")]
    public class CourseMaster
    {
        public string? CountryId { get; set; }

        public string? CouncilId { get; set; }

        public string? StateId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CourseDescription { get; set; }

        [Key]   // Assuming CourseId as Primary Key
        [Column(TypeName = "nvarchar(50)")]
        public string? CourseId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CourseShortCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Status { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedBy { get; set; }

        // In DB it is nvarchar(50), so keeping string
        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedOn { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? UpdatedBy { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? UpdatedOn { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string? AdditionalDegree { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? CourseNomeclature { get; set; }
    }

[Table("tbl_university")]
    public class University
    {
        public string? CountryId { get; set; }

        public string? StateId { get; set; }

        public string? CouncilId { get; set; }

        [Key]
        [Required]
        [Column("university_id", TypeName = "varchar(50)")]
        public string UniversityId { get; set; }

        [Column("university_name", TypeName = "varchar(250)")]
        public string? UniversityName { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Status { get; set; }

        [Column("university_code", TypeName = "varchar(5)")]
        public string? UniversityCode { get; set; }
    }



[Table("tbl_colleges")]
    public class College
    {
        public string? CountryId { get; set; }

        public string? StateId { get; set; }

        public string? CouncilId { get; set; }

        [Key]
        [Required]
        [Column("col_id", TypeName = "varchar(50)")]
        public string ColId { get; set; }

        [Column("col_name", TypeName = "varchar(250)")]
        public string? ColName { get; set; }

        [Column("col_address", TypeName = "varchar(500)")]
        public string? ColAddress { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? District { get; set; }

        [Column("type", TypeName = "varchar(25)")]
        public string? Type { get; set; }

        [Column("Principal_name", TypeName = "varchar(250)")]
        public string? PrincipalName { get; set; }

        [Column("tel_number", TypeName = "varchar(50)")]
        public string? TelNumber { get; set; }

        [Column("university_name", TypeName = "varchar(250)")]
        public string? UniversityName { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string? Email { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Status { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UserId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Password { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? FirsttimeLogin { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Photo { get; set; }

        public int? Usercount { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Country { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? State { get; set; }

        [Column("college_code", TypeName = "varchar(5)")]
        public string? CollegeCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Myid { get; set; }
    }


[Table("tbl_J_Council_Master")]
    public class JCouncilMaster
    {
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string CountryId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string StateId { get; set; }

        [Key]   // Assuming CouncilId as Primary Key
        [Column(TypeName = "nvarchar(50)")]
        public string? CouncilId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CouncilName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? Address { get; set; }   // nvarchar(max)

        [Column(TypeName = "nvarchar(50)")]
        public string? EmailId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Phoneno { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Website { get; set; }

        public string? Logo { get; set; }  // nvarchar(max)

        public string? BannerName { get; set; }  // nvarchar(max)

        public string? Banner { get; set; }  // nvarchar(max)

        [Column(TypeName = "nvarchar(50)")]
        public string? ShortCode { get; set; }

        [Column("city", TypeName = "nvarchar(50)")]
        public string? City { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string? Address2 { get; set; }

        [Column(TypeName = "varchar(8)")]
        public string? ZipCode { get; set; }
    }



[Table("tbl_renewal_history")]
    public class RenewalHistory
    {
        [Column(TypeName = "nvarchar(50)")]
        public string? CountryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? StateId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CouncilId { get; set; }

        [Key]
        [Column(TypeName = "varchar(50)")]
        public string RenewalID { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string PractitionerID { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? Type { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? ReceiptNumber { get; set; }

        public DateTime? ReceiptDate { get; set; }

        public DateTime? RenewalDate { get; set; }

        public DateTime? ValidUpto { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Bank { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? DD_ChequeNO { get; set; }

        public DateTime? DD_ChequeDate { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? TransactionNo { get; set; }

        public DateTime? TransactionDate { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Amount { get; set; }

        [Column(TypeName = "char(1)")]
        public string? Status { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string PaymentFor { get; set; } = "R";

        [Column(TypeName = "varchar(250)")]
        public string? internship_institution { get; set; }

        public DateTime? internship_from { get; set; }

        public DateTime? internship_to { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? address { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? RefNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? DCIRefNo { get; set; }

        public DateTime? DCIDate { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? SincID { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Fin_yr_code { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? slNo { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? GroupID { get; set; }

        public decimal? AutoReceiptNo { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? AccountNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public DateTime? councilRegdate { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? GoodStationdingRefNo { get; set; }

        [Column(TypeName = "varchar(300)")]
        public string? Comments { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? councilname { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? counciladdre { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? OnlineGoodStandingApprove { get; set; }

        public decimal? TAX_Amnt { get; set; }
    }



[Table("tbl_renewal_items")]
    public class RenewalItems
    {
        [Column(TypeName = "nvarchar(50)")]
        public string? CountryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? StateId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CouncilId { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [Key]
        public string RenewalID { get; set; }

        [Required]
        public int FeeItemId { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string PractitionerID { get; set; }

        public int? FeeAmount { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }


[Table("tbl_Group_LedgerMaster")]
    public class GroupLedgerMaster
    {
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string CouncilId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string StateId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CountryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? GroupID { get; set; }

        [Key]
        [Column(TypeName = "nvarchar(50)")]
        public string LedgerID { get; set; }

        [Required]
        public int LedgerCount { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? LedgerName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? LedgerDescription { get; set; }

        [Column(TypeName = "nchar(10)")]
        public string? Type { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string? Certificate_Type { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string? LedgerStatus { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public string? Status { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }


[Table("tbl_Financial_Year_Master")]
    public class FinancialYearMaster
    {
        [Key]
        public int FinYearrId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? YearCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? FinancialYear { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? UpdatedBy { get; set; }

        // In database UpdatedOn is nvarchar(50), so string
        [Column(TypeName = "nvarchar(50)")]
        public string? UpdatedOn { get; set; }
    }



[Table("tbl_Fees_Items")]
    public class FeesItems
    {
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string CouncilId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? StateId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CountryId { get; set; }

        [Key]
        public int FeeItemId { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? FeeItemName { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal? FeeAmount { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string? Status { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

[Table("tbl_Fees_Receipt")]
    public class FeesReceipt
    {
        [Column(TypeName = "nvarchar(50)")]
        public string? CountryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? StateId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CouncilId { get; set; }

        public string? GroupID { get; set; }

        public string? LedgerID { get; set; }

        public string? FeeTmplID { get; set; }

        public string? AccountNo { get; set; }

        public string? ReceiptNumber { get; set; }

        [Key]
        public string AutoReceiptNo { get; set; }

        public DateTime? ReceiptDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string? Type { get; set; }

        public string? CourseCode { get; set; }

        public string? FinancialYear { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Opening_Balance { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Closing_Balance { get; set; }

        public string? Remarks { get; set; }

        [Column(TypeName = "nvarchar(2)")]
        public string? Remited_NotRemited { get; set; }

        public string? Contra_Remarks { get; set; }

        public DateTime? Remitted_Date { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? Cancel_status { get; set; }

        public string? Cancel_Reason { get; set; }

        public DateTime? Cancel_date { get; set; }

        public string? Course_Category { get; set; }

        public DateTime? NextPymntDate { get; set; }

        public decimal? SubReceiptNo { get; set; }

        public int? receipt { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? ipaddress { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? ProgramId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TAX_Amnt { get; set; }
    }




[Table("tbl_Ledger_FeeItem_Link")]
    public class LedgerFeeItemLink
    {
        public string? CountryId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string CouncilId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string StateId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? LedgerID { get; set; }

        [Key]   // ✅ ADD THIS
        [Column(TypeName = "nvarchar(50)")]
        public string FeeItemID { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string? Certificate_Type { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Status { get; set; }
    }




    [Table("tbl_CB_Account_Master")]
    public class CBAccountMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }   // Only NOT NULL column

        public string? CouncilId { get; set; }
        public string? StateId { get; set; }
        public string? CountryId { get; set; }

        public int? AccountId { get; set; }

        public string? AccountCode { get; set; }
        public string? AccountNo { get; set; }
        public string? AccountName { get; set; }
        public string? AccountDescription { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string? Status { get; set; }
    }



[Table("tbl_CB_Group_Master")]
    public class CBGroupMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }   // Only NOT NULL column

        public string? CouncilId { get; set; }
        public string? StateId { get; set; }
        public string? CountryId { get; set; }

        public int? AutoGroupID { get; set; }

        public string? GroupID { get; set; }
        public string? GroupName { get; set; }
        public string? GroupDescription { get; set; }
        public string? FileName_PDF { get; set; }

        public string? Status { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }



[Table("tbl_Financial_Year")]
    [Keyless]
    public class Financial_Year
    {
        public string? CouncilId { get; set; }

        public string? StateId { get; set; }

        public string? CountryId { get; set; }

        public int? AutoFinYearId { get; set; }

        public int? YearID { get; set; }

        public string? GroupID { get; set; }

        public string? YearCode { get; set; }

        public string? FinancialYear { get; set; }

        public string? BankAccount { get; set; }

        public string? Opening_Balance { get; set; }

        public string? Closing_Balance { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }


    [Table("tbl_mds_subject_master")]
    public class MdsSubjectMaster
    {
        [Column(TypeName = "nvarchar(50)")]
        public string? CountryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? StateId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CouncilId { get; set; }

        [Key]
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Sub_code { get; set; } = string.Empty;

        [Column(TypeName = "varchar(50)")]
        public string? Sub_name { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? ShortCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CourseId { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? ActiveStatus { get; set; }
    }


    [Table("tbl_RayzorPay_Payment_Attempts")]
    public class RazorPayPaymentAttempt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? CountryId { get; set; }
        public string? StateId { get; set; }
        public string? CouncilId { get; set; }
        public string? PractitionerId { get; set; }
        public string? Order_id { get; set; }
        public string? Pay_id { get; set; }
        public string? ReceiptNo { get; set; }
        public string? Order_status { get; set; }
        public string? Currency { get; set; }
        public decimal? Amount { get; set; }
        public string? LedgerId { get; set; }
        public string? FinYear { get; set; }
        public string? CouncilName { get; set; }
        public string? CouncilAddress { get; set; }
        public string? Name { get; set; }
        public string? ContactNo { get; set; }
        public string? Email { get; set; }
        public string? DummyReceiptNo { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? Pids { get; set; }
        public string? ProgramId { get; set; }
        public string? ProgramName { get; set; }
    }
}
