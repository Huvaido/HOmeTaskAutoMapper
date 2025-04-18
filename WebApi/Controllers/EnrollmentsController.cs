using Microsoft.AspNetCore.Mvc;
using Domain.Responses;
using Infrastructure.Interfaces;
using Domain.DTOs.StudentDTOs;
using Domain.Filters;
using Domain.DTOs.CourseDTOs;
using Domain.DTOs.EnrollmentDTOs;
using Domain.Response;



namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController(IEnrollmentService enrollmentService)
{

    [HttpGet]
    public Task<Response<List<GetEnrollmentDTO>>> GetAll(StudentFilter filter)
    {
        return enrollmentService.GetAllEnrollments(filter);
    }

    [HttpPost]
    public Task<Response<GetEnrollmentDTO>> Add(CreateEnrollmentDTO enrrollmentDto)
    {
        return enrollmentService.CreateEnrollment(enrrollmentDto);

    }


    [HttpPut]
    public Task<Response<GetEnrollmentDTO>> Update(int id, GetEnrollmentDTO enrollmentDto)
    {
        return enrollmentService.UpdateEnrollment(id, enrollmentDto);

    }


    [HttpDelete("{id:int}")]
    public Task<Response<string>> Delete(int id)
    {
        return enrollmentService.DeleteEnrollment(id);

    }

}
