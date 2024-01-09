using System.ComponentModel.DataAnnotations;

namespace TestBuilder.Models;

public partial class Option
{
    [Key] public int OptionId { get; set; }
    public Guid QuestionId { get; set; }
    public required string Text { get; set; }
    public string? Image { get; set; }

    // Navigation property
    public Question Question { get; set; }
}