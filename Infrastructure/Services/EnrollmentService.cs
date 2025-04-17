using System.Net;
using AutoMapper;
using Domain.DTOs.EnrollmentDTOs;
using Domain.Entities;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class EnrollmentService(DataContext context, IMapper mapper) : IEnrollmentService
{
    public async Task<Response<GetEnrollmentDTO>> CreateEnrollment(CreateEnrollmentDTO createEnrollment)
    {
        var enrollment = mapper.Map<Enrollment>(createEnrollment);

        await context.Enrollments.AddAsync(enrollment);
        var result = await context.SaveChangesAsync();

        var getEnrollmentDto = mapper.Map<GetEnrollmentDTO>(enrollment);

        return result == 0
            ? new Response<GetEnrollmentDTO>(HttpStatusCode.BadRequest, "Enrollment wasn't created")
            : new Response<GetEnrollmentDTO>(getEnrollmentDto);
    }

    public async Task<Response<GetEnrollmentDTO>> UpdateEnrollment(int EnrollmentId, GetEnrollmentDTO updateEnrollment)
    {
        var enrollment = await context.Enrollments.FindAsync(EnrollmentId);

        if (enrollment == null)
            return new Response<GetEnrollmentDTO>(HttpStatusCode.NotFound, "Enrollment not found");

        enrollment.StudentId = updateEnrollment.StudentId;
        enrollment.CourseId = updateEnrollment.CourseId;
        enrollment.EnrollDate = updateEnrollment.EnrollDate;
        enrollment.Grade = updateEnrollment.Grade;

        var result = await context.SaveChangesAsync();
        var getEnrollmentDto = mapper.Map<GetEnrollmentDTO>(enrollment);

        return result == 0
            ? new Response<GetEnrollmentDTO>(HttpStatusCode.BadRequest, "Enrollment wasn't updated")
            : new Response<GetEnrollmentDTO>(getEnrollmentDto);
    }

    public async Task<Response<string>> DeleteEnrollment(int Enrollmentid)
    {
        var enrollment = await context.Enrollments.FindAsync(Enrollmentid);

        if (enrollment == null)
            return new Response<string>(HttpStatusCode.NotFound, "Enrollment not found");

        context.Enrollments.Remove(enrollment);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Enrollment wasn't deleted")
            : new Response<string>("Enrollment deleted successfully");
    }

    public async Task<Response<List<GetEnrollmentDTO>>> GetAllEnrollments()
    {
        var enrollments = await context.Enrollments.ToListAsync();

        if (enrollments.Count == 0)
            return new Response<List<GetEnrollmentDTO>>(HttpStatusCode.NotFound, "No enrollments found");

        var getEnrollmentsDto = mapper.Map<List<GetEnrollmentDTO>>(enrollments);

        return new Response<List<GetEnrollmentDTO>>(getEnrollmentsDto);
    }

}