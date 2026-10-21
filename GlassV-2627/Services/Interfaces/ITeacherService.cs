using GlassV_2627.DTOs.TeacherDTOs;
using GlassV_2627.Filters;
namespace GlassV_2627.Services.Interfaces;

public interface ITeacherService
{
    public List<TeacherResponseDTO> GetAll(TeacherFilterModel filters);
    public TeacherResponseDTO? GetOne(int id);
    public int? Create(TeacherEditDTO teacher);
    public bool Update(TeacherEditDTO teacher);
    public bool Delete(int id);
}