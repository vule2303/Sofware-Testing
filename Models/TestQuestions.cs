using System.ComponentModel.DataAnnotations;

namespace TestBuilder.Models;

public class TestQuestions
{
    [Key] public int TestId { get; set; }
    public required Guid QuestionId { get; set; }

    // Navigation properties
    public Test Test { get; set; }
    public Question Question { get; set; }
}