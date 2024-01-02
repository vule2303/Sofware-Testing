using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestBuilder.Models;

public abstract class Option
{
    [Key]
    public int OptionId { get; set; }
    public int QuestionId { get; set; }
    public required string Text { get; set; }
    public string? Image { get; set; }

    // Navigation property
    [ForeignKey("QuestionId")]
    public required Question Question { get; set; }
}