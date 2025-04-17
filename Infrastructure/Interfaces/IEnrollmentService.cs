using Domain.DTOs.EnrollmentDTOs;
using Domain.Response;

namespace Infrastructure.Interfaces;

public interface IEnrollmentService
{
    Task<Response<GetEnrollmentDTO>> CreateEnrollment(CreateEnrollmentDTO createEnrollment);
    Task<Response<GetEnrollmentDTO>> UpdateEnrollment(int EnrollmentId, GetEnrollmentDTO updateEnrollment);
    Task<Response<string>> DeleteEnrollment(int Enrollmentid);
    Task<Response<List<GetEnrollmentDTO>>> GetAllEnrollments();
}