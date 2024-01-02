namespace TestBuilder.Models;
public abstract class Question
{
    public int QuestionId { get; set; }
    public required string Text { get; set; }
    public string? Image { get; set; }

    // Navigation property
    public List<Option>? Options { get; set; }
    public List<TestQuestions>? TestQuestions { get; set; }
}