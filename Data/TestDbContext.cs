using Microsoft.EntityFrameworkCore;
using TestBuilder.Models;

namespace TestBuilder.Data;

public class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<Option> Options { get; set; }
    public virtual DbSet<Exam> Exams { get; set; }
    public virtual DbSet<Test> Tests { get; set; }
    public virtual DbSet<TestExams> TestExams { get; set; }
    public virtual DbSet<TestQuestions> TestQuestions { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<Chapters> Chapters { get; set; }
    public virtual DbSet<ExamsSubjects> ExamsSubjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure your database connection here
        optionsBuilder.UseInMemoryDatabase("ExaminingTestDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>()
            .HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId);

        modelBuilder.Entity<TestExams>()
            .HasKey(te => new { te.TestId, te.ExamId });

        modelBuilder.Entity<TestExams>()
            .HasOne(te => te.Test)
            .WithMany(t => t.TestExams)
            .HasForeignKey(te => te.TestId);

        modelBuilder.Entity<TestExams>()
            .HasOne(te => te.Exam)
            .WithMany(e => e.TestExams)
            .HasForeignKey(te => te.ExamId);

        modelBuilder.Entity<TestQuestions>()
            .HasKey(tq => new { tq.TestId, tq.QuestionId });

        modelBuilder.Entity<TestQuestions>()
            .HasOne(tq => tq.Test)
            .WithMany(t => t.TestQuestions)
            .HasForeignKey(tq => tq.TestId);

        modelBuilder.Entity<TestQuestions>()
            .HasOne(tq => tq.Question)
            .WithMany(q => q.TestQuestions)
            .HasForeignKey(tq => tq.QuestionId);

        modelBuilder.Entity<Chapters>()
            .HasOne(c => c.Subject)
            .WithMany(s => s.Chapters)
            .HasForeignKey(c => c.SubjectId);

        modelBuilder.Entity<ExamsSubjects>()
            .HasKey(es => new { es.ExamId, es.SubjectId });

        modelBuilder.Entity<ExamsSubjects>()
            .HasOne(es => es.Exam)
            .WithMany(e => e.ExamsSubjects)
            .HasForeignKey(es => es.ExamId);

        modelBuilder.Entity<ExamsSubjects>()
            .HasOne(es => es.Subject)
            .WithMany(s => s.ExamsSubjects)
            .HasForeignKey(es => es.SubjectId);
    }
}