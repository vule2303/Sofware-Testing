namespace TestBuilder.Models;
public class Option
{
    public int OptionId { get; set; }
    public int QuestionId { get; set; }
    public required string Text { get; set; }
    public string? Image { get; set; }

    // Navigation property
    public Question Question { get; set; }
}