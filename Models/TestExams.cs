namespace TestBuilder.Models;

public class TestExams
{
    public int TestId { get; set; }
    public int ExamId { get; set; }

    // Navigation properties
    public Test? Test { get; set; }
    public Exam? Exam { get; set; }
}