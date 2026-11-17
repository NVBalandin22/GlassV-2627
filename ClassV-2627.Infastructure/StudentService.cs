using GlassV_2627.Data;
using GlassV_2627.DTOs.StudentDTOs;
using GlassV_2627.Filters;
using GlassV_2627.Models;
using GlassV_2627.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace GlassV_2627.Services;

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;
    
    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<StudentResponseDTO>> GetAll(StudentFilterModel filters)
    {
        IQueryable<Student> baseQuery = _context.Students;

        if (filters.Grade.HasValue)
        {
            baseQuery = baseQuery.Where(student => student.Grade == filters.Grade.Value);
        }
        if (filters.Age.HasValue)
        {
            baseQuery = baseQuery.Where(student => student.Age == filters.Age.Value);
        }
        if (!string.IsNullOrEmpty(filters.FirstName))
        {
            // WHERE FIRSTNAME LIKE '%asdadasd%'
            baseQuery = baseQuery.Where(student => student.FirstName.Contains(filters.FirstName));
        }
        if (!string.IsNullOrEmpty(filters.LastName))
        {
            baseQuery = baseQuery.Where(student => student.LastName.Contains(filters.LastName));
        }
        
        baseQuery = baseQuery.Skip((filters.Page - 1) * filters.PageSize).Take(filters.PageSize);

        IQueryable<StudentResponseDTO> query = baseQuery.Select(x =>
            new StudentResponseDTO()
            {
                Id = x.Id,
                Names = x.FirstName + " " + x.LastName,
                Age = x.Age
            });
        
        return await query.ToListAsync();
    }

    public async Task<StudentResponseDTO?> GetOne(int id)
    {
        StudentResponseDTO? student = await _context.Students
            .Where(student => student.Id == id)
            .Select(student =>
                new StudentResponseDTO()
                {
                    Id = student.Id,
                    Age = student.Age,
                    Names = student.FirstName + " " + student.LastName
                }
            ).FirstOrDefaultAsync();

        return student;
    }

    public async Task<int?> Create(StudentEditDTO createStudent)
    {
        Student entity = new Student();
        
        entity.FirstName = createStudent.FirstName;
        entity.LastName = createStudent.LastName;
        entity.Age = createStudent.Age;
        entity.UserName = createStudent.UserName;
        entity.Grade = 0;
        entity.TeacherId = createStudent.TeacherId;
        entity.Password = "TempPassword";

        try
        {
            _context.Students.Add(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return null;
        }
        
        return entity.Id;
    }

    public async Task<bool> Update(StudentEditDTO updateStudent)
    {
        Student? entity = await _context.Students
            .Where(student => student.Id == updateStudent.Id)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            return false;
        }
        
        entity.FirstName = updateStudent.FirstName;
        entity.LastName = updateStudent.LastName;
        entity.TeacherId = updateStudent.TeacherId;
        entity.Age = updateStudent.Age;
        entity.UserName = updateStudent.UserName;
        
        
        try
        {
            _context.Students.Update(entity);
            await _context.SaveChangesAsync();
            return true;
            
        }
        catch (Exception e)
        {
            return false;
        }
        
    }

    public async Task<bool> Delete(int id)
    {
        Student? entity = await _context.Students
            .Where(student => student.Id == id)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            return false;
        }
        
        _context.Students.Remove(entity);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> UpdateGrade(StudentUpdateGradeDTO updateStudent)
    {
        Student entity = await _context.Students
            .Where(student => student.Id == updateStudent.StudentId)
            .FirstAsync();

        entity.Grade = updateStudent.NewGrade;
        
        bool result = await _context.SaveChangesAsync() == 1;
        
        return result;
    }
}