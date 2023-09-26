using MFAE.Jobs.Location;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.Attachments;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MFAE.Jobs.Authorization.Delegation;
using MFAE.Jobs.Authorization.Roles;
using MFAE.Jobs.Authorization.Users;
using MFAE.Jobs.Chat;
using MFAE.Jobs.Editions;
using MFAE.Jobs.Friendships;
using MFAE.Jobs.MultiTenancy;
using MFAE.Jobs.MultiTenancy.Accounting;
using MFAE.Jobs.MultiTenancy.Payments;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.EntityFrameworkCore
{
    public class JobsDbContext : AbpZeroDbContext<Tenant, Role, User, JobsDbContext>
    {
        public virtual DbSet<Locality> Localities { get; set; }

        public virtual DbSet<Governorate> Governorates { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<ApplicantStatus> ApplicantStatuses { get; set; }

        public virtual DbSet<ConversationRate> ConversationRates { get; set; }

        public virtual DbSet<ApplicantLanguage> ApplicantLanguages { get; set; }

        public virtual DbSet<ApplicantTraining> ApplicantTrainings { get; set; }

        public virtual DbSet<ApplicantStudy> ApplicantStudies { get; set; }

        public virtual DbSet<MaritalStatus> MaritalStatuses { get; set; }

        public virtual DbSet<IdentificationType> IdentificationTypes { get; set; }

        public virtual DbSet<Applicant> Applicants { get; set; }

        public virtual DbSet<Conversation> Conversations { get; set; }

        public virtual DbSet<GraduationRate> GraduationRates { get; set; }

        public virtual DbSet<Specialties> Specialtieses { get; set; }

        public virtual DbSet<AcademicDegree> AcademicDegrees { get; set; }

        public virtual DbSet<AppLanguage> AppLanguages { get; set; }

        public virtual DbSet<AttachmentFile> AttachmentFiles { get; set; }

        public virtual DbSet<AttachmentType> AttachmentTypes { get; set; }

        public virtual DbSet<AttachmentTypeGroup> AttachmentTypeGroups { get; set; }

        public virtual DbSet<AttachmentEntityType> AttachmentEntityTypes { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public JobsDbContext(DbContextOptions<JobsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });
        }
    }
}