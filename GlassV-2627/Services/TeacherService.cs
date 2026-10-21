using GlassV_2627.Data;
using GlassV_2627.DTOs.TeacherDTOs;
using GlassV_2627.Filters;
using GlassV_2627.Models;
using GlassV_2627.Services.Interfaces;

namespace GlassV_2627.Services;

public class TeacherService : ITeacherService
{
    private readonly ApplicationDbContext _context;
    
    public TeacherService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<TeacherResponseDTO> GetAll(TeacherFilterModel filters)
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
        return query.ToList();
    }

    public TeacherResponseDTO? GetOne(int id)
    {
        throw new NotImplementedException();
    }

    public int? Create(TeacherEditDTO teacher)
    {
        throw new NotImplementedException();
    }

    public bool Update(TeacherEditDTO teacher)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }
}