using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;

namespace SchoolApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly AppDbContext _context;

    public GradesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/grades
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GradeReadDto>>> GetAll()
    {
        var grades = await _context.Grades
            .AsNoTracking()
            .Include(g => g.Teacher)
            .Include(g => g.Student)
            .OrderBy(g => g.Id)
            .ToListAsync();

        // Mapped in memory because ToReadDto is not translatable to SQL.
        return Ok(grades.Select(ToReadDto).ToList());
    }

    // GET: api/grades/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GradeReadDto>> GetById(int id)
    {
        var grade = await _context.Grades
            .AsNoTracking()
            .Include(g => g.Teacher)
            .Include(g => g.Student)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (grade is null)
        {
            return NotFound(new { message = $"No grade found with id {id}." });
        }

        return Ok(ToReadDto(grade));
    }

    // POST: api/grades
    [HttpPost]
    public async Task<ActionResult<GradeReadDto>> Create(GradeWriteDto dto)
    {
        var validation = await ValidateReferences(dto.TeacherId, dto.StudentId);
        if (validation is not null)
        {
            return validation;
        }

        var grade = new Grade
        {
            Name = dto.Name,
            Value = dto.Value,
            TeacherId = dto.TeacherId,
            StudentId = dto.StudentId
        };

        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();

        await _context.Entry(grade).Reference(g => g.Teacher).LoadAsync();
        await _context.Entry(grade).Reference(g => g.Student).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = grade.Id }, ToReadDto(grade));
    }

    // PUT: api/grades/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, GradeWriteDto dto)
    {
        var grade = await _context.Grades.FirstOrDefaultAsync(g => g.Id == id);

        if (grade is null)
        {
            return NotFound(new { message = $"No grade found with id {id}." });
        }

        var validation = await ValidateReferences(dto.TeacherId, dto.StudentId);
        if (validation is not null)
        {
            return validation;
        }

        grade.Name = dto.Name;
        grade.Value = dto.Value;
        grade.TeacherId = dto.TeacherId;
        grade.StudentId = dto.StudentId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/grades/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var grade = await _context.Grades.FirstOrDefaultAsync(g => g.Id == id);

        if (grade is null)
        {
            return NotFound(new { message = $"No grade found with id {id}." });
        }

        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<ActionResult?> ValidateReferences(int teacherId, int studentId)
    {
        var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == teacherId);
        if (!teacherExists)
        {
            return BadRequest(new { message = $"No teacher found with id {teacherId}." });
        }

        var studentExists = await _context.Students.AnyAsync(s => s.Id == studentId);
        if (!studentExists)
        {
            return BadRequest(new { message = $"No student found with id {studentId}." });
        }

        return null;
    }

    private static GradeReadDto ToReadDto(Grade g) => new()
    {
        Id = g.Id,
        Name = g.Name,
        Value = g.Value,
        TeacherId = g.TeacherId,
        TeacherName = g.Teacher != null ? g.Teacher.Name : null,
        StudentId = g.StudentId,
        StudentName = g.Student != null ? g.Student.Name : null
    };
}
