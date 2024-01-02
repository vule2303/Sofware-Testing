namespace TestBuilder.Models;
public abstract class Chapters
{
    public int ChapterId { get; set; }
    public int SubjectId { get; set; }
    public required string Name { get; set; }

    // Navigation properties
    public required Subject Subject { get; set; }
}