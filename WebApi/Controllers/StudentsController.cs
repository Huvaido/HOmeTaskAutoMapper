using Microsoft.AspNetCore.Mvc;
using Domain.Responses;
using Infrastructure.Interfaces;
using Domain.DTOs.StudentDTOs;
using Domain.Filters;
using Domain.Response;



namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StudentsController(IStudentService studentService)
{

    [HttpGet]
    public Task<Response<List<GetStudentDTO>>> GetAllStudent(StudentFilter filter)
    {
       return studentService.GetAllStudents(filter);
    }

    [HttpPost]
    public Task<Response<GetStudentDTO>> AddStudent(CreateStudentDTO studentDto)
    {
        return studentService.CreateStudent(studentDto);

    }


    [HttpPut]
    public Task<Response<GetStudentDTO>> UpdateStudent(int id, GetStudentDTO studentDto)
    {
        return studentService.UpdateStudent(id, studentDto);

    }


    [HttpDelete("{id:int}")]
    public Task<Response<string>> DeleteStudent(int id)
    {
        return studentService.DeleteStudent(id);

    }

}
