using System.ComponentModel.DataAnnotations;

namespace TestBuilder.Models;

public class Test
{
    [Key] public int TestId { get; set; }

    public required string Title { get; set; }
    public int SubjectId { get; set; }

    // Navigation properties
    public List<TestExams>? TestExams { get; set; } = [];
    public List<TestQuestions>? TestQuestions { get; set; } = [];
    public Subject? Subject { get; set; }
}