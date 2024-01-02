namespace TestBuilder.Models;
public abstract class Subject
{
    public int SubjectId { get; set; }
    public required string Name { get; set; }

    // Navigation properties
    public List<Chapters>? Chapters { get; set; }
    public List<ExamsSubjects>? ExamsSubjects { get; set; }
}