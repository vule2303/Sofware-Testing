namespace TestBuilder.Models;
public abstract class Option
{
    public int OptionId { get; set; }
    public int QuestionId { get; set; }
    public required string Text { get; set; }
    public string? Image { get; set; }

    // Navigation property
    public required Question Question { get; set; }
}