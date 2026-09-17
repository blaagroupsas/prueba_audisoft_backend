using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;

namespace SchoolApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/students
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentReadDto>>> GetAll()
    {
        var students = await _context.Students
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .Select(s => new StudentReadDto { Id = s.Id, Name = s.Name })
            .ToListAsync();

        return Ok(students);
    }

    // GET: api/students/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentReadDto>> GetById(int id)
    {
        var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

        if (student is null)
        {
            return NotFound(new { message = $"No student found with id {id}." });
        }

        return Ok(new StudentReadDto { Id = student.Id, Name = student.Name });
    }

    // POST: api/students
    [HttpPost]
    public async Task<ActionResult<StudentReadDto>> Create(StudentWriteDto dto)
    {
        var student = new Student { Name = dto.Name };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        var result = new StudentReadDto { Id = student.Id, Name = student.Name };
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, result);
    }

    // PUT: api/students/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, StudentWriteDto dto)
    {
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);

        if (student is null)
        {
            return NotFound(new { message = $"No student found with id {id}." });
        }

        student.Name = dto.Name;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/students/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);

        if (student is null)
        {
            return NotFound(new { message = $"No student found with id {id}." });
        }

        var hasGrades = await _context.Grades.AnyAsync(g => g.StudentId == id);
        if (hasGrades)
        {
            return Conflict(new { message = "Cannot delete: this student has grades associated." });
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
