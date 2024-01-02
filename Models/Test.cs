namespace TestBuilder.Models;
public abstract class Test
{
    public int TestId { get; set; }
    public required string Title { get; set; }

    // Navigation properties
    public List<TestExams>? TestExams { get; set; }
    public List<TestQuestions>? TestQuestions { get; set; }
}