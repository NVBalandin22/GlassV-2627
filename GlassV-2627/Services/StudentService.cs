using GlassV_2627.Data;
using GlassV_2627.DTOs.StudentDTOs;
using GlassV_2627.Models;
using GlassV_2627.Services.Interfaces;

namespace GlassV_2627.Services;

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;
    
    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public List<StudentResponseDTO> GetAll()
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
        
        return students;
    }

    public StudentResponseDTO? GetOne(int id)
    {
        StudentResponseDTO? student = _context.Students
            .Where(student => student.Id == id)
            .Select(student =>
                new StudentResponseDTO()
                {
                    Id = student.Id,
                    Age = student.Age,
                    Names = student.FirstName + " " + student.LastName
                }
            ).FirstOrDefault();

        return student;
    }

    public int? Create(StudentEditDTO createStudent)
    {
        Student entity = new Student();
        
        entity.FirstName = createStudent.FirstName;
        entity.LastName = createStudent.LastName;
        entity.Age = createStudent.Age;
        entity.UserName = createStudent.UserName;
        entity.Grade = 0;
        entity.Password = "TempPassword";

        try
        {
            _context.Students.Add(entity);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            return null;
        }
        
        return entity.Id;
    }

    public bool Update(StudentEditDTO updateStudent)
    {
        Student? entity = _context.Students
            .Where(student => student.Id == updateStudent.Id)
            .FirstOrDefault();

        if (entity == null)
        {
            return false;
        }
        
        entity.FirstName = updateStudent.FirstName;
        entity.LastName = updateStudent.LastName;
        entity.Age = updateStudent.Age;
        entity.UserName = updateStudent.UserName;
        
        bool result = _context.SaveChanges() == 1;
        
        return result;
    }

    public bool Delete(int id)
    {
        Student? entity = _context.Students
            .Where(student => student.Id == id)
            .FirstOrDefault();

        if (entity == null)
        {
            return false;
        }
        
        _context.Students.Remove(entity);
        bool result = _context.SaveChanges() == 1;
        
        return result;
    }

    public bool UpdateGrade(StudentUpdateGradeDTO updateStudent)
    {
        Student entity = _context.Students
            .Where(student => student.Id == updateStudent.StudentId)
            .First();

        entity.Grade = updateStudent.NewGrade;
        
        bool result = _context.SaveChanges() == 1;
        
        return result;
    }
}