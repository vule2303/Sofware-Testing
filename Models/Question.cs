using System.ComponentModel.DataAnnotations;

namespace TestBuilder.Models;

public class Question
{
    [Key] public Guid QuestionId { get; set; }

    public required string Content { get; set; }

    public string? Image { get; set; } = "";

    public string? Formula { get; set; } = "";

    public int? TestId { get; set; }

    // Navigation property
    public List<Option>? Options { get; set; }
    public List<TestQuestions>? TestQuestions { get; set; }
}