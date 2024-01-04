using System.ComponentModel.DataAnnotations;

namespace TestBuilder.Models;
public partial class Chapters
{
    [Key]
    public int ChapterId { get; set; }
    public int SubjectId { get; set; }
    public required string Name { get; set; }

    // Navigation properties
    public required Subject Subject { get; set; }
}