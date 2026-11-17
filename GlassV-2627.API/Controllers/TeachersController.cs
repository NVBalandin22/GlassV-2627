using GlassV_2627.DTOs.TeacherDTOs;
using GlassV_2627.Filters;
using GlassV_2627.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GlassV_2627.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]

public class TeachersController:ControllerBase
{
    private readonly ITeacherService _service;
    public TeachersController(ITeacherService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> GetAll([FromBody] TeacherFilterModel filters)
    {
        var result = await _service.GetAll(filters);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetOne([FromQuery] int id)
    {
        var result = await _service.GetOne(id);
        if(result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TeacherEditDTO teacher)
    {
        var result = await _service.Create(teacher);
        if (result == null) return BadRequest();
        return Created();
    }

    [HttpPut]
    public  async Task<IActionResult> Update([FromBody] TeacherEditDTO teacher)
    {
        var result = await _service.Update(teacher);
        if (!result) return BadRequest();
        return Ok();
    }

    [HttpDelete]
    public  async Task<IActionResult> Delete([FromQuery] int id)
    {
        var result = await _service.Delete(id);
        if (!result) return NotFound();
        return Ok();
    }
}