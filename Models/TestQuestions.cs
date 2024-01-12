using System.ComponentModel.DataAnnotations;

namespace TestBuilder.Models;

public class TestQuestions
{
    [Key] public int TestId { get; set; }
    public Guid QuestionId { get; set; }

    // Navigation properties
    public required Test Test { get; set; }
    public required Question Question { get; set; }
}