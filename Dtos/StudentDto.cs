using System.ComponentModel.DataAnnotations;

namespace SchoolApi.Dtos;

public class StudentReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class StudentWriteDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
}
