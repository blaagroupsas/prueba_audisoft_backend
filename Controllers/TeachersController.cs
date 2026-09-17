using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Dtos;
using SchoolApi.Models;

namespace SchoolApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeachersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/teachers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeacherReadDto>>> GetAll()
    {
        var teachers = await _context.Teachers
            .AsNoTracking()
            .OrderBy(t => t.Id)
            .Select(t => new TeacherReadDto { Id = t.Id, Name = t.Name })
            .ToListAsync();

        return Ok(teachers);
    }

    // GET: api/teachers/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TeacherReadDto>> GetById(int id)
    {
        var teacher = await _context.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

        if (teacher is null)
        {
            return NotFound(new { message = $"No teacher found with id {id}." });
        }

        return Ok(new TeacherReadDto { Id = teacher.Id, Name = teacher.Name });
    }

    // POST: api/teachers
    [HttpPost]
    public async Task<ActionResult<TeacherReadDto>> Create(TeacherWriteDto dto)
    {
        var teacher = new Teacher { Name = dto.Name };

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        var result = new TeacherReadDto { Id = teacher.Id, Name = teacher.Name };
        return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, result);
    }

    // PUT: api/teachers/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TeacherWriteDto dto)
    {
        var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);

        if (teacher is null)
        {
            return NotFound(new { message = $"No teacher found with id {id}." });
        }

        teacher.Name = dto.Name;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/teachers/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);

        if (teacher is null)
        {
            return NotFound(new { message = $"No teacher found with id {id}." });
        }

        var hasGrades = await _context.Grades.AnyAsync(g => g.TeacherId == id);
        if (hasGrades)
        {
            return Conflict(new { message = "Cannot delete: this teacher has grades associated." });
        }

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
