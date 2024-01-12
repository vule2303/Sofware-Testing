using System.ComponentModel.DataAnnotations;

namespace TestBuilder.Models;

public class Chapters
{
    [Key] public int ChapterId { get; set; }
    public int SubjectId { get; set; }
    public required string Name { get; set; }

    // Navigation properties
    public Subject? Subject { get; set; }
}