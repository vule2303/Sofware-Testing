using System.ComponentModel.DataAnnotations;

namespace TestBuilder.Models;

public abstract class TestExams
{
    [Key]
    public int TestId { get; set; }
    public int ExamId { get; set; }

    // Navigation properties
    public required Test Test { get; set; }
    public required Exam Exam { get; set; }
}