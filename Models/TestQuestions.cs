namespace TestBuilder.Models;
public abstract class TestQuestions
{
    public int TestId { get; set; }
    public Guid QuestionId { get; set; }

    // Navigation properties
    public required Test Test { get; set; }
    public required Question Question { get; set; }
}