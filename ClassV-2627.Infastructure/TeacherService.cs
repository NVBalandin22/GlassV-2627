using GlassV_2627.Data;
using GlassV_2627.DTOs.TeacherDTOs;
using GlassV_2627.Filters;
using GlassV_2627.Models;
using GlassV_2627.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace GlassV_2627.Services;

public class TeacherService : ITeacherService
{
    private readonly ApplicationDbContext _context;
    
    public TeacherService(ApplicationDbContext context)
    {
        _context = context;
    }

    public  async Task<List<TeacherResponseDTO>> GetAll(TeacherFilterModel filters)
    {
        IQueryable<Teacher> baseQuery = _context.Teachers;

        if (!string.IsNullOrEmpty(filters.Subject))
        {
            baseQuery = baseQuery.Where(t => t.Subject.Contains(filters.Subject));
        }
        if (!string.IsNullOrEmpty(filters.Name))
        {
            baseQuery = baseQuery.Where(t => t.Name.Contains(filters.Name));
        }
        if (filters.Age.HasValue)
        {
            baseQuery = baseQuery.Where(teacher => teacher.Age == filters.Age.Value);
        }
        
        baseQuery = baseQuery.Skip((filters.Page-1) * filters.PageSize).Take(filters.PageSize);
        
        IQueryable<TeacherResponseDTO> query = baseQuery.Select(x=> 
            new TeacherResponseDTO()
            {
                Age = x.Age,
                Id = x.Id,
                Name = x.Name,
                Subject = x.Subject,
                
            });
        return await query.ToListAsync();
    }

    public async Task<TeacherResponseDTO?> GetOne(int id)
    {
        TeacherResponseDTO? teacher = await _context.Teachers
            .Where(teacher => teacher.Id == id)
            .Select(teacher =>
                new TeacherResponseDTO()
                {
                    Id = teacher.Id,
                    Age = teacher.Age,
                    Name = teacher.Name,
                    Subject = teacher.Subject
                }
            ).FirstOrDefaultAsync();
        return teacher;
    }

    public async Task<int?> Create(TeacherEditDTO teacher)
    {
        Teacher entity = new Teacher();
        entity.Age = teacher.Age;
        entity.Name = teacher.Name;
        entity.Subject = teacher.Subject;
        entity.UserName = teacher.UserName;
        entity.Password = "TempPassword";

        try
        {
            _context.Teachers.Add(entity);
            await _context.SaveChangesAsync();
        }
        catch
        {
            return null;
        }
        
        return entity.Id;
    }

    public async Task<bool> Update(TeacherEditDTO teacher)
    {
        Teacher? entity = await _context.Teachers.Where(t => t.Id==teacher.Id).FirstOrDefaultAsync();
        if (entity == null) return false;
        entity.Age = teacher.Age;
        entity.Name = teacher.Name;
        entity.Subject = teacher.Subject;
        entity.UserName = teacher.UserName;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        Teacher? entity = await _context.Teachers.Where(t=>t.Id == id).FirstOrDefaultAsync();
        if (entity == null) return false;
        _context.Teachers.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}