using EducationalSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EducationalSystem.Infrastructure.Models;

public partial class EducationalSystemApiContext : IdentityDbContext<ApplicationUser>
{
    public EducationalSystemApiContext()
    {
    }

    public EducationalSystemApiContext(DbContextOptions<EducationalSystemApiContext> options)
    : base(options)
    {
    }

    public virtual DbSet<Group> TbGroups { get; set; }
    public virtual DbSet<Attendance> TbAttendance { get; set; }
    public virtual DbSet<Payment> TbPayments { get; set; }
    public virtual DbSet<Expense> TbExpenses { get; set; }
    public DbSet<UserGroup> TbUserGroups { get; set; }
    public DbSet<Exam> TbExams { get; set; }
    public DbSet<Question> TbQuestions { get; set; }
    public DbSet<QuestionOption> TbQuestionOptions { get; set; }
    public DbSet<StudentExam> TbStudentExams { get; set; }
    public DbSet<StudentAnswer> TbStudentAnswers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-S1KCJF8\\SQLEXPRESS;Database=EducationalSystemAPI;Trusted_Connection=true;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.User)
            .WithMany(u => u.UserGroups)
            .HasForeignKey(ug => ug.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.Group)
            .WithMany(g => g.UserGroups)
            .HasForeignKey(ug => ug.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>()
       .HasOne(p => p.Student)
       .WithMany()
       .HasForeignKey(p => p.StudentId)
       .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.ReceivedByUser)
            .WithMany()
            .HasForeignKey(p => p.ReceivedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TotalMarks).HasPrecision(5, 2);
            entity.Property(e => e.PassingMarks).HasPrecision(5, 2);

            entity.HasOne(e => e.Group)
                  .WithMany()
                  .HasForeignKey(e => e.GroupId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Question Configuration
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(q => q.Id);
            entity.Property(q => q.QuestionText).IsRequired();
            entity.Property(q => q.Marks).HasPrecision(5, 2);

            entity.HasOne(q => q.Exam)
                  .WithMany(e => e.Questions)
                  .HasForeignKey(q => q.ExamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // QuestionOption Configuration
        modelBuilder.Entity<QuestionOption>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.OptionText).IsRequired();

            entity.HasOne(o => o.Question)
                  .WithMany(q => q.Options)
                  .HasForeignKey(o => o.QuestionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // StudentExam Configuration
        modelBuilder.Entity<StudentExam>(entity =>
        {
            entity.HasKey(se => se.Id);
            entity.Property(se => se.Score).HasPrecision(5, 2);

            entity.HasOne(se => se.Exam)
                  .WithMany(e => e.StudentExams)
                  .HasForeignKey(se => se.ExamId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // StudentAnswer Configuration
        modelBuilder.Entity<StudentAnswer>(entity =>
        {
            entity.HasKey(sa => sa.Id);
            entity.Property(sa => sa.MarksObtained).HasPrecision(5, 2);

            entity.HasOne(sa => sa.StudentExam)
                  .WithMany(se => se.Answers)
                  .HasForeignKey(sa => sa.StudentExamId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sa => sa.Question)
                  .WithMany()
                  .HasForeignKey(sa => sa.QuestionId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
