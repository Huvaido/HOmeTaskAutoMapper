using Domain.DTOs.CourseDTOs;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface ICourseService
{
    Task<Response<GetCourseDTO>> CreateCourse(CreateCourseDTO createCourse);
    Task<Response<GetCourseDTO>> UpdateCourse(int courseId, GetCourseDTO updateCourse);
    Task<Response<string>> DeleteCourse(int courseId);
    Task<Response<List<GetCourseDTO>>> GetAllCourses();
}