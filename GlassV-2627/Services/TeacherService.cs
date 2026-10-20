using GlassV_2627.Data;

namespace GlassV_2627.Services;

public class TeacherService : ITeacherService
{
    private readonly ApplicationDbContext _context;
    
    public TeacherService(ApplicationDbContext context)
    {
        _context = context;
    }
}