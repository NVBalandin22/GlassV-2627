using GlassV_2627.Data;
using GlassV_2627.DTOs.StudentDTOs;
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
        List<StudentResponseDTO> students = _context.Students
            .Select(student => 
                new StudentResponseDTO()
                {
                    Id = student.Id,
                    Age = student.Age,
                    Names = student.FirstName + " " + student.LastName,
                }
            ).ToList();
        
        return Ok(students);
    }

    [HttpGet]
    public IActionResult GetOne([FromQuery] int id)
    {
        StudentResponseDTO student = _context.Students
            .Where(student => student.Id == id)
            .Select(student =>
                new StudentResponseDTO()
                {
                    Id = student.Id,
                    Age = student.Age,
                    Names = student.FirstName + " " + student.LastName
                }
            ).First();
        
        return Ok(student);
    }

    [HttpPost]
    public IActionResult Create([FromBody] StudentEditDTO createStudent)
    {
        Student entity = new Student();
        
        entity.FirstName = createStudent.FirstName;
        entity.LastName = createStudent.LastName;
        entity.Age = createStudent.Age;
        entity.UserName = createStudent.UserName;
        entity.Grade = 0;
        entity.Password = "TempPassword";

        _context.Students.Add(entity);
        _context.SaveChanges();
        
        return Ok();
    }

    [HttpPut]
    public IActionResult Update([FromBody] StudentEditDTO updateStudent)
    {
        Student entity = _context.Students
            .Where(student => student.Id == updateStudent.Id)
            .First();
        
        entity.FirstName = updateStudent.FirstName;
        entity.LastName = updateStudent.LastName;
        entity.Age = updateStudent.Age;
        entity.UserName = updateStudent.UserName;
        
        _context.SaveChanges();
        
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete([FromQuery] int id)
    {
        Student entity = _context.Students
            .Where(student => student.Id == id)
            .First();
        
        _context.Students.Remove(entity);
        _context.SaveChanges();
        
        return Ok();
    }

    [HttpPatch]
    public IActionResult UpdateGrade([FromBody] StudentUpdateGradeDTO updateStudent)
    {
        Student entity = _context.Students
            .Where(student => student.Id == updateStudent.StudentId)
            .First();

        entity.Grade = updateStudent.NewGrade;
        _context.SaveChanges();
        
        return Ok();
    }
}