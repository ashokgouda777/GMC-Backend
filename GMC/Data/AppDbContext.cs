using GMC.Model;
using GMC.Model.GMC.Models;
using Microsoft.EntityFrameworkCore;

namespace GMC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Practitioner> Practitioners { get; set; }

        public DbSet<RoleMaster> RoleMasters { get; set; }
        public DbSet<CaseWorker> CaseWorkers { get; set; }

        public DbSet<RegistrationForMaster> RegistrationFor { get; set; }
        public DbSet<TitleMaster> TitleMaster { get; set; }

        public DbSet<GenderMaster> GenderMaster { get; set; }

        public DbSet<BloodGroupTypeMaster> BloodGroupTypeMaster { get; set; }

        public DbSet<NationalityMaster> NationalityMaster { get; set; }
        public DbSet<PractitionerEligiblity> PractitionerEligiblity { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PractitionerEligiblity>().HasNoKey();
        }

        public DbSet<CountryMaster> CountryMaster { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }

        public DbSet<Address> Address { get; set; }
        public DbSet<StateMaster> StateMaster { get; set; }
        public DbSet<DistrictMaster> DistrictMaster { get; set; }
        public DbSet<EducationInfo> EducationInfo { get; set; }
        public DbSet<CourseMaster> CourseMaster { get; set; }
        public DbSet<University> University { get; set; }
        public DbSet<College> College { get; set; }

        public DbSet<JCouncilMaster> JCouncilMaster { get; set; }
        public DbSet<RenewalHistory> RenewalHistory { get; set; }
        public DbSet<RenewalItems> RenewalItems { get; set; }
        public DbSet<GroupLedgerMaster> GroupLedgerMaster { get; set; }
        public DbSet<FinancialYearMaster> FinancialYearMaster { get; set; }
        public DbSet<FeesItems> FeesItems { get; set; }
        public DbSet<FeesReceipt> FeesReceipt { get; set; }
        public DbSet<LedgerFeeItemLink> LedgerFeeItemLink { get; set; }

        public DbSet<CBAccountMaster> CBAccountMaster { get; set; }
        public DbSet<CBGroupMaster> CBGroupMaster { get; set; }
        public DbSet<Financial_Year> Financial_Year { get; set; }

        public DbSet<MdsSubjectMaster> MdsSubjectMaster { get; set; }

        public DbSet<RazorPayPaymentAttempt> RazorPayPaymentAttempt { get; set; }
        public DbSet<CertificateMaster> CertificateMasters { get; set; }

        public DbSet<PractitionerDocumentDetails> PractitionerDocumentDetails { get; set; }
        public DbSet<RegtypeDocListLink> RegtypeDocListLink { get; set; }
    }
}
