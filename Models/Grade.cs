using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApi.Models;

public class Grade
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(5,2)")]
    public decimal Value { get; set; }

    public int TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    public int StudentId { get; set; }
    public Student? Student { get; set; }
}
