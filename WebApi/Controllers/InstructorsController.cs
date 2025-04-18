using Microsoft.AspNetCore.Mvc;
using Domain.Responses;
using Infrastructure.Interfaces;
using Domain.DTOs.StudentDTOs;
using Domain.Filters;
using Domain.DTOs.InstructorDto;
using Domain.Response;



namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class InstructorsController(IInstructorService instructorService)
{

    [HttpGet]
    public Task<Response<List<GetInstructorDto>>> GetAll(StudentFilter filter)
    {
       return instructorService.GetAll(filter);
    }

    [HttpPost]
    public Task<Response<GetInstructorDto>> Add(CreateInstructorDto instructorDto)
    {
        return instructorService.AddInstructor(instructorDto);

    }
    
    [HttpPut]
    public Task<Response<GetInstructorDto>> Update(int id, UpdateInstructorDto instructorDto)
    {
        return instructorService.Update(id,instructorDto);

    }


    [HttpDelete("{id:int}")]
    public Task<Response<string>> Delete(int id)
    {
        return instructorService.DeleteInstructor(id);

    }

}
