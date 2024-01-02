namespace TestBuilder.Models;
public abstract class Exam
{
    public int ExamId { get; set; }
    public required string Title { get; set; }

    // Navigation properties
    public List<TestExams>? TestExams { get; set; }
    public List<ExamsSubjects>? ExamsSubjects { get; set; }
}