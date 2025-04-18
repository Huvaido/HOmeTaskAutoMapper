using Domain.DTOs.InstructorDto;
using Domain.Filters;
using Domain.Response;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IInstructorService
{
    Task<Response<GetInstructorDto>> AddInstructor(CreateInstructorDto instructorDto);
    Task<Response<string>> DeleteInstructor(int id);
    Task<Response<List<GetInstructorDto>>> GetAll(StudentFilter filter);
    Task<Response<GetInstructorDto>> Update(int id, UpdateInstructorDto instructorDto);
}
