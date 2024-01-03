namespace TestBuilder.Models;
public partial class TestExams
{
    public int TestId { get; set; }
    public int ExamId { get; set; }

    // Navigation properties
    public required Test Test { get; set; }
    public required Exam Exam { get; set; }
}