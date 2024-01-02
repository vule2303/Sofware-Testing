namespace TestBuilder.Models;
public abstract class ExamsSubjects
{
    public int ExamId { get; set; }
    public int SubjectId { get; set; }

    // Navigation properties
    public required Exam Exam { get; set; }
    public required Subject Subject { get; set; }
}