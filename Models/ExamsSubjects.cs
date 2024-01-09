namespace TestBuilder.Models;

public class ExamsSubjects
{
    public int ExamId { get; set; }
    public int SubjectId { get; set; }

    // Navigation properties
    public Exam? Exam { get; set; }
    public Subject? Subject { get; set; }
}