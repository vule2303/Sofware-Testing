using System.ComponentModel.DataAnnotations;

namespace TestBuilder.Models;

public abstract class TestQuestions
{
    [Key]
    public int TestId { get; set; }
    public int QuestionId { get; set; }

    // Navigation properties
    public required Test Test { get; set; }
    public required Question Question { get; set; }
}