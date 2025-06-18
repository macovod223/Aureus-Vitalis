using Microsoft.EntityFrameworkCore;
using AureusVitalis.Data.Entities;

namespace AureusVitalis.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        // ─── сущности ────────────────────────────────────────────────────────────
        public DbSet<AppUser>            Users               => Set<AppUser>();
        public DbSet<UserProgress>       UserProgress        => Set<UserProgress>();
        public DbSet<UserPracticeResult> UserPracticeResults => Set<UserPracticeResult>();
        public DbSet<UserExamAttempt>    UserExamAttempts    => Set<UserExamAttempt>();
        public DbSet<UserTestResult>     UserTestResults     => Set<UserTestResult>();
        public DbSet<Certificate>        Certificates        => Set<Certificate>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<UserProgress>()
                .HasKey(e => new { e.UserId, e.ModuleKey });

            mb.Entity<UserPracticeResult>()
                .HasKey(e => new { e.UserId, e.TaskKey });

            mb.Entity<UserTestResult>()
                .HasKey(e => new { e.AttemptId, e.QuestionKey });

            mb.Entity<UserTestResult>()
                .HasOne(r => r.Attempt)
                .WithMany(a => a.TestResults)
                .HasForeignKey(r => r.AttemptId);

            // один сертификат на курс для пользователя
            mb.Entity<Certificate>()
                .ToTable("Certificates")
                .HasIndex(c => new { c.UserId, c.CourseKey })
                .IsUnique();
        }
    }
}