using Microsoft.AspNetCore.Mvc;
using Domain.Responses;
using Infrastructure.Interfaces;
using Domain.DTOs.StudentDTOs;
using Domain.Filters;
using Domain.DTOs.CourseDTOs;
using Domain.Response;



namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CourseController(ICourseService courseService)
{

    [HttpGet]
    public Task<Response<List<GetCourseDTO>>> GetAll(StudentFilter filter)
    {
       return courseService.GetAllCourses(filter);
    }

    [HttpPost]
    public Task<Response<GetCourseDTO>> Add(CreateCourseDTO courseDto)
    {
        return courseService.CreateCourse(courseDto);

    }

    [HttpPut]
    public Task<Response<GetCourseDTO>> Update(int id, GetCourseDTO courseDto)
    {
        return courseService.UpdateCourse(id,courseDto);

    }


    [HttpDelete("{id:int}")]
    public Task<Response<string>> Delete(int id)
    {
        return courseService.DeleteCourse(id);

    }

}
