using System.ComponentModel.DataAnnotations;

namespace SchoolApi.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
