using System.Net;
using AutoMapper;
using Domain.DTOs.EnrollmentDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Domain.Responses;
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

    public async Task<Response<List<GetEnrollmentDTO>>> GetAllEnrollments(StudentFilter filter)
    {
        var validfilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var enrollments = context.Enrollments.AsQueryable();

        var mapped = mapper.Map<List<GetEnrollmentDTO>>(enrollments);
        var totalRecords = mapped.Count;

        var data = mapped
            .Skip((validfilter.PageNumber - 1) * validfilter.PageSize)
            .Take(validfilter.PageSize)
            .ToList();

        return new PagedResponse<List<GetEnrollmentDTO>>(data, validfilter.PageNumber, validfilter.PageSize,
            totalRecords);
    }

}