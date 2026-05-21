using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GMC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_address",
                columns: table => new
                {
                    AddressID = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "varchar(50)", nullable: true),
                    StateId = table.Column<string>(type: "varchar(50)", nullable: true),
                    CouncilId = table.Column<string>(type: "varchar(50)", nullable: true),
                    ClientID = table.Column<string>(type: "varchar(50)", nullable: false),
                    AddressType = table.Column<string>(type: "varchar(10)", nullable: false),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "varchar(100)", nullable: true),
                    State = table.Column<string>(type: "varchar(50)", nullable: true),
                    Zip = table.Column<string>(type: "varchar(50)", nullable: true),
                    Country = table.Column<string>(type: "varchar(100)", nullable: true),
                    Phone1 = table.Column<string>(type: "varchar(25)", nullable: true),
                    Phone2 = table.Column<string>(type: "varchar(25)", nullable: true),
                    PlaceType = table.Column<string>(type: "varchar(10)", nullable: true),
                    District = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_address", x => x.AddressID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_blood_group_type_master",
                columns: table => new
                {
                    BloodGroupId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    BloodGroupCode = table.Column<string>(type: "text", nullable: true),
                    BloodGroupDescription = table.Column<string>(type: "text", nullable: true),
                    BloodGroupDetails = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Active = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_blood_group_type_master", x => x.BloodGroupId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_case_workers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false),
                    CouncilId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CWUserId = table.Column<string>(type: "text", nullable: false),
                    MobileNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    EmailId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RoleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_case_workers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_CB_Account_Master",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    AccountId = table.Column<int>(type: "integer", nullable: true),
                    AccountCode = table.Column<string>(type: "text", nullable: true),
                    AccountNo = table.Column<string>(type: "text", nullable: true),
                    AccountName = table.Column<string>(type: "text", nullable: true),
                    AccountDescription = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_CB_Account_Master", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_CB_Group_Master",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    AutoGroupID = table.Column<int>(type: "integer", nullable: true),
                    GroupID = table.Column<string>(type: "text", nullable: true),
                    GroupName = table.Column<string>(type: "text", nullable: true),
                    GroupDescription = table.Column<string>(type: "text", nullable: true),
                    FileName_PDF = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_CB_Group_Master", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Certificate_Master",
                columns: table => new
                {
                    Certificate_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    Certificate_Name = table.Column<string>(type: "text", nullable: true),
                    Certificate_Type = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Certificate_Master", x => x.Certificate_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_colleges",
                columns: table => new
                {
                    col_id = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    col_name = table.Column<string>(type: "varchar(250)", nullable: true),
                    col_address = table.Column<string>(type: "varchar(500)", nullable: true),
                    District = table.Column<string>(type: "varchar(50)", nullable: true),
                    type = table.Column<string>(type: "varchar(25)", nullable: true),
                    Principal_name = table.Column<string>(type: "varchar(250)", nullable: true),
                    tel_number = table.Column<string>(type: "varchar(50)", nullable: true),
                    university_name = table.Column<string>(type: "varchar(250)", nullable: true),
                    Email = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(50)", nullable: true),
                    Password = table.Column<string>(type: "varchar(50)", nullable: true),
                    FirsttimeLogin = table.Column<string>(type: "varchar(50)", nullable: true),
                    Photo = table.Column<string>(type: "varchar(50)", nullable: true),
                    Usercount = table.Column<int>(type: "integer", nullable: true),
                    Country = table.Column<string>(type: "varchar(50)", nullable: true),
                    State = table.Column<string>(type: "varchar(50)", nullable: true),
                    college_code = table.Column<string>(type: "varchar(5)", nullable: true),
                    Myid = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_colleges", x => x.col_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_CourseMaster",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CourseDescription = table.Column<string>(type: "varchar(50)", nullable: true),
                    CourseShortCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<string>(type: "varchar(50)", nullable: true),
                    AdditionalDegree = table.Column<string>(type: "varchar(5)", nullable: true),
                    CourseNomeclature = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_CourseMaster", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_district_master",
                columns: table => new
                {
                    CountryId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StateId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CouncilId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DistrictId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DistrictName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Districtcount = table.Column<int>(type: "integer", nullable: true),
                    AppYN = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    AppCenter = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_district_master", x => x.DistrictId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_education_info",
                columns: table => new
                {
                    EducationID = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    PractitionerID = table.Column<string>(type: "varchar(50)", nullable: false),
                    EducationName = table.Column<string>(type: "varchar(100)", nullable: true),
                    YearOfPassing = table.Column<string>(type: "varchar(4)", nullable: true),
                    CollegeID = table.Column<string>(type: "varchar(50)", nullable: true),
                    UniversityId = table.Column<string>(type: "varchar(50)", nullable: true),
                    Subject = table.Column<string>(type: "varchar(50)", nullable: true),
                    TempYOP = table.Column<string>(type: "varchar(50)", nullable: true),
                    TempCollege = table.Column<string>(type: "varchar(250)", nullable: true),
                    MonthOfPassing = table.Column<string>(type: "varchar(50)", nullable: true),
                    TempUniversity = table.Column<string>(type: "varchar(250)", nullable: true),
                    TempEducationName = table.Column<string>(type: "varchar(250)", nullable: true),
                    SubCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<string>(type: "varchar(50)", nullable: true),
                    CouncilCode = table.Column<string>(type: "varchar(30)", nullable: true),
                    CertificateNo = table.Column<string>(type: "varchar(20)", nullable: true),
                    CertificateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ADSerialno = table.Column<string>(type: "varchar(20)", nullable: true),
                    IsIssued = table.Column<string>(type: "varchar(25)", nullable: true),
                    LedgerVerified = table.Column<string>(type: "varchar(50)", nullable: true),
                    Active = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_education_info", x => x.EducationID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Fees_Items",
                columns: table => new
                {
                    FeeItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CouncilId = table.Column<string>(type: "varchar(50)", nullable: false),
                    StateId = table.Column<string>(type: "varchar(50)", nullable: true),
                    CountryId = table.Column<string>(type: "varchar(50)", nullable: true),
                    FeeItemName = table.Column<string>(type: "varchar(250)", nullable: true),
                    FeeAmount = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    Status = table.Column<string>(type: "varchar(10)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Fees_Items", x => x.FeeItemId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Fees_Receipt",
                columns: table => new
                {
                    AutoReceiptNo = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<string>(type: "varchar(50)", nullable: true),
                    StateId = table.Column<string>(type: "varchar(50)", nullable: true),
                    CouncilId = table.Column<string>(type: "varchar(50)", nullable: true),
                    GroupID = table.Column<string>(type: "text", nullable: true),
                    LedgerID = table.Column<string>(type: "text", nullable: true),
                    FeeTmplID = table.Column<string>(type: "text", nullable: true),
                    AccountNo = table.Column<string>(type: "text", nullable: true),
                    ReceiptNumber = table.Column<string>(type: "text", nullable: true),
                    ReceiptDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Type = table.Column<string>(type: "varchar(10)", nullable: true),
                    CourseCode = table.Column<string>(type: "text", nullable: true),
                    FinancialYear = table.Column<string>(type: "text", nullable: true),
                    Opening_Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Closing_Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    Remited_NotRemited = table.Column<string>(type: "varchar(2)", nullable: true),
                    Contra_Remarks = table.Column<string>(type: "text", nullable: true),
                    Remitted_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Cancel_status = table.Column<string>(type: "text", nullable: true),
                    Cancel_Reason = table.Column<string>(type: "text", nullable: true),
                    Cancel_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Course_Category = table.Column<string>(type: "text", nullable: true),
                    NextPymntDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SubReceiptNo = table.Column<decimal>(type: "numeric", nullable: true),
                    receipt = table.Column<int>(type: "integer", nullable: true),
                    ipaddress = table.Column<string>(type: "varchar(50)", nullable: true),
                    ProgramId = table.Column<string>(type: "varchar(50)", nullable: true),
                    TAX_Amnt = table.Column<decimal>(type: "numeric(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Fees_Receipt", x => x.AutoReceiptNo);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Financial_Year",
                columns: table => new
                {
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    AutoFinYearId = table.Column<int>(type: "integer", nullable: true),
                    YearID = table.Column<int>(type: "integer", nullable: true),
                    GroupID = table.Column<string>(type: "text", nullable: true),
                    YearCode = table.Column<string>(type: "text", nullable: true),
                    FinancialYear = table.Column<string>(type: "text", nullable: true),
                    BankAccount = table.Column<string>(type: "text", nullable: true),
                    Opening_Balance = table.Column<string>(type: "text", nullable: true),
                    Closing_Balance = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tbl_Financial_Year_Master",
                columns: table => new
                {
                    FinYearrId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    YearCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    FinancialYear = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Financial_Year_Master", x => x.FinYearrId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_GenderMaster",
                columns: table => new
                {
                    GenderId = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    GenderName = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_GenderMaster", x => x.GenderId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Group_LedgerMaster",
                columns: table => new
                {
                    LedgerID = table.Column<string>(type: "varchar(50)", nullable: false),
                    CouncilId = table.Column<string>(type: "varchar(50)", nullable: false),
                    StateId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "varchar(50)", nullable: true),
                    GroupID = table.Column<string>(type: "varchar(50)", nullable: true),
                    LedgerCount = table.Column<int>(type: "integer", nullable: false),
                    LedgerName = table.Column<string>(type: "varchar(50)", nullable: true),
                    LedgerDescription = table.Column<string>(type: "varchar(100)", nullable: true),
                    Type = table.Column<string>(type: "char(10)", nullable: true),
                    Certificate_Type = table.Column<string>(type: "varchar(10)", nullable: true),
                    LedgerStatus = table.Column<string>(type: "varchar(10)", nullable: true),
                    Status = table.Column<string>(type: "varchar(5)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Group_LedgerMaster", x => x.LedgerID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_J_Council_Master",
                columns: table => new
                {
                    CouncilId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "varchar(50)", nullable: false),
                    StateId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CouncilName = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    EmailId = table.Column<string>(type: "varchar(50)", nullable: true),
                    Phoneno = table.Column<string>(type: "varchar(50)", nullable: true),
                    Website = table.Column<string>(type: "varchar(50)", nullable: true),
                    Logo = table.Column<string>(type: "text", nullable: true),
                    BannerName = table.Column<string>(type: "text", nullable: true),
                    Banner = table.Column<string>(type: "text", nullable: true),
                    ShortCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    city = table.Column<string>(type: "varchar(50)", nullable: true),
                    Address2 = table.Column<string>(type: "varchar(200)", nullable: true),
                    ZipCode = table.Column<string>(type: "varchar(8)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_J_Council_Master", x => x.CouncilId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_J_Country_Master",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<string>(type: "text", nullable: false),
                    CountryName = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Active = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_J_Country_Master", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_J_State_Master",
                columns: table => new
                {
                    StateId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CountryId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    StateName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Active = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_J_State_Master", x => x.StateId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Ledger_FeeItem_Link",
                columns: table => new
                {
                    FeeItemID = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "varchar(50)", nullable: false),
                    StateId = table.Column<string>(type: "varchar(50)", nullable: false),
                    LedgerID = table.Column<string>(type: "varchar(50)", nullable: true),
                    Certificate_Type = table.Column<string>(type: "varchar(10)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Ledger_FeeItem_Link", x => x.FeeItemID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_masterNationality",
                columns: table => new
                {
                    NationalityId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    Nationality = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_masterNationality", x => x.NationalityId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_mds_subject_master",
                columns: table => new
                {
                    Sub_code = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "varchar(50)", nullable: true),
                    StateId = table.Column<string>(type: "varchar(50)", nullable: true),
                    CouncilId = table.Column<string>(type: "varchar(50)", nullable: true),
                    Sub_name = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShortCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    CourseId = table.Column<string>(type: "varchar(50)", nullable: true),
                    ActiveStatus = table.Column<string>(type: "varchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_mds_subject_master", x => x.Sub_code);
                });

            migrationBuilder.CreateTable(
                name: "tbl_practitioner",
                columns: table => new
                {
                    PractitionerID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    RegistrationNo = table.Column<string>(type: "text", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RegistrationType = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ChangeOfName = table.Column<string>(type: "text", nullable: true),
                    SpouseName = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BirthPlace = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    Nationality = table.Column<string>(type: "text", nullable: true),
                    Vote = table.Column<string>(type: "text", nullable: true),
                    EmailID = table.Column<string>(type: "text", nullable: true),
                    MobileNumber = table.Column<string>(type: "text", nullable: true),
                    LoginID = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    PhotoPath = table.Column<string>(type: "text", nullable: true),
                    FirstLogin = table.Column<string>(type: "text", nullable: true),
                    photo = table.Column<string>(type: "text", nullable: true),
                    practsign = table.Column<string>(type: "text", nullable: true),
                    thumb = table.Column<string>(type: "text", nullable: true),
                    barcode = table.Column<string>(type: "text", nullable: true),
                    declaration = table.Column<string>(type: "text", nullable: true),
                    Revwr_Editr_YN = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    BloodGroup = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Supplementary = table.Column<string>(type: "text", nullable: true),
                    RegistrationFor = table.Column<string>(type: "text", nullable: true),
                    PrintStatus = table.Column<string>(type: "text", nullable: true),
                    Class = table.Column<string>(type: "text", nullable: true),
                    Registerstatus = table.Column<string>(type: "text", nullable: true),
                    RefreshRegistration = table.Column<string>(type: "text", nullable: true),
                    PaymentStatus = table.Column<string>(type: "text", nullable: true),
                    Synch = table.Column<string>(type: "text", nullable: true),
                    PRC_payment_ReceiptNO = table.Column<string>(type: "text", nullable: true),
                    IsOnline = table.Column<string>(type: "text", nullable: true),
                    tem_regNo = table.Column<string>(type: "text", nullable: true),
                    tem_practitionerId = table.Column<string>(type: "text", nullable: true),
                    cme_photo = table.Column<string>(type: "text", nullable: true),
                    FromStatecode = table.Column<string>(type: "text", nullable: true),
                    ToStateCode = table.Column<string>(type: "text", nullable: true),
                    IsInliue = table.Column<string>(type: "text", nullable: true),
                    YearofRegistration = table.Column<string>(type: "text", nullable: true),
                    HaveUserNamePassword = table.Column<string>(type: "text", nullable: true),
                    TR_MBBS = table.Column<string>(type: "text", nullable: true),
                    TR_Non_MBBS = table.Column<string>(type: "text", nullable: true),
                    TR_SuperSpeciality = table.Column<string>(type: "text", nullable: true),
                    PrcNoArchive = table.Column<string>(type: "text", nullable: true),
                    deleteyn = table.Column<string>(type: "text", nullable: true),
                    deletedby = table.Column<string>(type: "text", nullable: true),
                    Approveby = table.Column<string>(type: "text", nullable: true),
                    ApproveOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Approve1 = table.Column<string>(type: "text", nullable: true),
                    Approve1date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    photo1 = table.Column<string>(type: "text", nullable: true),
                    photo2 = table.Column<string>(type: "text", nullable: true),
                    Proffession = table.Column<string>(type: "text", nullable: true),
                    ipaddress = table.Column<string>(type: "text", nullable: true),
                    Obj_Ren = table.Column<string>(type: "text", nullable: true),
                    CME_Renw = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    RegCanceldate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_practitioner", x => x.PractitionerID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Practitioner_Document_Details",
                columns: table => new
                {
                    DocumentId = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    PractitionerID = table.Column<string>(type: "text", nullable: true),
                    DocumentType = table.Column<string>(type: "text", nullable: true),
                    DocumentSize = table.Column<string>(type: "text", nullable: true),
                    DocumentName = table.Column<string>(type: "text", nullable: true),
                    DocumentPath = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<string>(type: "text", nullable: true),
                    FileExtn = table.Column<string>(type: "text", nullable: true),
                    PhotoYN = table.Column<string>(type: "text", nullable: true),
                    BCodeYN = table.Column<string>(type: "text", nullable: true),
                    Sync_Yn = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Practitioner_Document_Details", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_PractitionerEligiblity",
                columns: table => new
                {
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    EligibiltyId = table.Column<int>(type: "integer", nullable: true),
                    Eligibilty = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tbl_RayzorPay_Payment_Attempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    PractitionerId = table.Column<string>(type: "text", nullable: true),
                    Order_id = table.Column<string>(type: "text", nullable: true),
                    Pay_id = table.Column<string>(type: "text", nullable: true),
                    ReceiptNo = table.Column<string>(type: "text", nullable: true),
                    Order_status = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    LedgerId = table.Column<string>(type: "text", nullable: true),
                    FinYear = table.Column<string>(type: "text", nullable: true),
                    CouncilName = table.Column<string>(type: "text", nullable: true),
                    CouncilAddress = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ContactNo = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    DummyReceiptNo = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Pids = table.Column<string>(type: "text", nullable: true),
                    ProgramId = table.Column<string>(type: "text", nullable: true),
                    ProgramName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RayzorPay_Payment_Attempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_RegistrationForMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    RegId = table.Column<string>(type: "text", nullable: true),
                    RegForName = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_RegistrationForMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Regtype_doc_list_link",
                columns: table => new
                {
                    Doc_types = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    Ledgerid = table.Column<string>(type: "text", nullable: true),
                    Type_Id = table.Column<string>(type: "text", nullable: true),
                    Doc_Details = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Regtype_doc_list_link", x => x.Doc_types);
                });

            migrationBuilder.CreateTable(
                name: "tbl_renewal_history",
                columns: table => new
                {
                    RenewalID = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "varchar(50)", nullable: true),
                    StateId = table.Column<string>(type: "varchar(50)", nullable: true),
                    CouncilId = table.Column<string>(type: "varchar(50)", nullable: true),
                    PractitionerID = table.Column<string>(type: "varchar(50)", nullable: false),
                    Type = table.Column<string>(type: "varchar(10)", nullable: true),
                    ReceiptNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    ReceiptDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RenewalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidUpto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Bank = table.Column<string>(type: "varchar(50)", nullable: true),
                    DD_ChequeNO = table.Column<string>(type: "varchar(50)", nullable: true),
                    DD_ChequeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TransactionNo = table.Column<string>(type: "varchar(50)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Amount = table.Column<string>(type: "varchar(50)", nullable: true),
                    Status = table.Column<string>(type: "char(1)", nullable: true),
                    PaymentFor = table.Column<string>(type: "varchar(50)", nullable: false),
                    internship_institution = table.Column<string>(type: "varchar(250)", nullable: true),
                    internship_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    internship_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    address = table.Column<string>(type: "varchar(500)", nullable: true),
                    RefNo = table.Column<string>(type: "varchar(100)", nullable: true),
                    DCIRefNo = table.Column<string>(type: "varchar(50)", nullable: true),
                    DCIDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SincID = table.Column<string>(type: "varchar(50)", nullable: true),
                    Fin_yr_code = table.Column<string>(type: "varchar(50)", nullable: true),
                    slNo = table.Column<string>(type: "varchar(10)", nullable: true),
                    GroupID = table.Column<string>(type: "varchar(50)", nullable: true),
                    AutoReceiptNo = table.Column<decimal>(type: "numeric", nullable: true),
                    AccountNo = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    councilRegdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GoodStationdingRefNo = table.Column<string>(type: "varchar(10)", nullable: true),
                    Comments = table.Column<string>(type: "varchar(300)", nullable: true),
                    councilname = table.Column<string>(type: "varchar(50)", nullable: true),
                    counciladdre = table.Column<string>(type: "varchar(50)", nullable: true),
                    OnlineGoodStandingApprove = table.Column<string>(type: "varchar(100)", nullable: true),
                    TAX_Amnt = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_renewal_history", x => x.RenewalID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_renewal_items",
                columns: table => new
                {
                    RenewalID = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "varchar(50)", nullable: true),
                    StateId = table.Column<string>(type: "varchar(50)", nullable: true),
                    CouncilId = table.Column<string>(type: "varchar(50)", nullable: true),
                    FeeItemId = table.Column<int>(type: "integer", nullable: false),
                    PractitionerID = table.Column<string>(type: "varchar(50)", nullable: false),
                    FeeAmount = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_renewal_items", x => x.RenewalID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Role_Master",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<string>(type: "text", nullable: true),
                    RoleDesc = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdateBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Role_Master", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_title_master",
                columns: table => new
                {
                    TitleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TitleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_title_master", x => x.TitleId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_university",
                columns: table => new
                {
                    university_id = table.Column<string>(type: "varchar(50)", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    university_name = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", nullable: true),
                    university_code = table.Column<string>(type: "varchar(5)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_university", x => x.university_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    StateId = table.Column<string>(type: "text", nullable: true),
                    CouncilId = table.Column<string>(type: "text", nullable: true),
                    Site_Id = table.Column<int>(type: "integer", nullable: true),
                    UserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Role_Id = table.Column<string>(type: "text", nullable: true),
                    Email_Id = table.Column<string>(type: "text", nullable: true),
                    MobileNo = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    District = table.Column<string>(type: "text", nullable: true),
                    Photo = table.Column<string>(type: "text", nullable: true),
                    usercount = table.Column<int>(type: "integer", nullable: true),
                    Firsttime_login = table.Column<string>(type: "text", nullable: true),
                    userId = table.Column<string>(type: "text", nullable: true),
                    RegType = table.Column<string>(type: "text", nullable: true),
                    typeUser = table.Column<string>(type: "text", nullable: true),
                    ipaddress = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_wesiteconfig",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PrimaryColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SecondaryColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ButtonColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SidebarColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SidebarText = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SidebarActiveColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TopbarBg = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NavbarBg = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NavbarText = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CardBg = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TableHeaderBg = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TableHeaderText = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TableBodyBg = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TableBodyText = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TableBtnColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_wesiteconfig", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_address");

            migrationBuilder.DropTable(
                name: "tbl_blood_group_type_master");

            migrationBuilder.DropTable(
                name: "tbl_case_workers");

            migrationBuilder.DropTable(
                name: "tbl_CB_Account_Master");

            migrationBuilder.DropTable(
                name: "tbl_CB_Group_Master");

            migrationBuilder.DropTable(
                name: "tbl_Certificate_Master");

            migrationBuilder.DropTable(
                name: "tbl_colleges");

            migrationBuilder.DropTable(
                name: "tbl_CourseMaster");

            migrationBuilder.DropTable(
                name: "tbl_district_master");

            migrationBuilder.DropTable(
                name: "tbl_education_info");

            migrationBuilder.DropTable(
                name: "tbl_Fees_Items");

            migrationBuilder.DropTable(
                name: "tbl_Fees_Receipt");

            migrationBuilder.DropTable(
                name: "tbl_Financial_Year");

            migrationBuilder.DropTable(
                name: "tbl_Financial_Year_Master");

            migrationBuilder.DropTable(
                name: "tbl_GenderMaster");

            migrationBuilder.DropTable(
                name: "tbl_Group_LedgerMaster");

            migrationBuilder.DropTable(
                name: "tbl_J_Council_Master");

            migrationBuilder.DropTable(
                name: "tbl_J_Country_Master");

            migrationBuilder.DropTable(
                name: "tbl_J_State_Master");

            migrationBuilder.DropTable(
                name: "tbl_Ledger_FeeItem_Link");

            migrationBuilder.DropTable(
                name: "tbl_masterNationality");

            migrationBuilder.DropTable(
                name: "tbl_mds_subject_master");

            migrationBuilder.DropTable(
                name: "tbl_practitioner");

            migrationBuilder.DropTable(
                name: "tbl_Practitioner_Document_Details");

            migrationBuilder.DropTable(
                name: "tbl_PractitionerEligiblity");

            migrationBuilder.DropTable(
                name: "tbl_RayzorPay_Payment_Attempts");

            migrationBuilder.DropTable(
                name: "tbl_RegistrationForMaster");

            migrationBuilder.DropTable(
                name: "tbl_Regtype_doc_list_link");

            migrationBuilder.DropTable(
                name: "tbl_renewal_history");

            migrationBuilder.DropTable(
                name: "tbl_renewal_items");

            migrationBuilder.DropTable(
                name: "tbl_Role_Master");

            migrationBuilder.DropTable(
                name: "tbl_title_master");

            migrationBuilder.DropTable(
                name: "tbl_university");

            migrationBuilder.DropTable(
                name: "tbl_Users");

            migrationBuilder.DropTable(
                name: "tbl_wesiteconfig");
        }
    }
}
