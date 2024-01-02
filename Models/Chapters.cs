using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestBuilder.Models;

public abstract class Chapters
{
    [Key]
    public int ChapterId { get; set; }
    public int SubjectId { get; set; }
    public required string Name { get; set; }

    // Navigation properties
    [ForeignKey("SubjectId")]
    public required Subject Subject { get; set; }
}