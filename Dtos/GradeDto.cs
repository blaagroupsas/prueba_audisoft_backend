using System.ComponentModel.DataAnnotations;

namespace SchoolApi.Dtos;

public class GradeReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public int TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
}

public class GradeWriteDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 5, ErrorMessage = "Value must be between 0 and 5.")]
    public decimal Value { get; set; }

    [Required(ErrorMessage = "TeacherId is required.")]
    public int TeacherId { get; set; }

    [Required(ErrorMessage = "StudentId is required.")]
    public int StudentId { get; set; }
}
