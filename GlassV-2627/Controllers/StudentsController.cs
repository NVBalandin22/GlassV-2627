using GlassV_2627.Data;
using GlassV_2627.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlassV_2627.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class StudentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public StudentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        List<Student> students = _context.Students.ToList();
        return Ok(students);
    }

    [HttpGet]
    public IActionResult GetById([FromQuery] int id)
    {
        Student student = _context.Students.Where(x => x.Id == id).First();
        return Ok(student);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Student student)
    {
        _context.Students.Add(student);
        _context.SaveChanges();
        
        return Ok();
    }

    [HttpPut]
    public IActionResult Update([FromBody] Student student)
    {
        Student dbStudent = _context.Students.Where(x => x.Id == student.Id).First();

        dbStudent.FirstName = student.FirstName;
        dbStudent.LastName = student.LastName;
        dbStudent.Age = student.Age;
        
        _context.SaveChanges();
        
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete([FromQuery] int id)
    {
        Student studentToDelete = _context.Students.Where(x => x.Id == id).First();
        _context.Students.Remove(studentToDelete);
        
        _context.SaveChanges();
        
        return Ok();
    }
}