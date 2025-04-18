using System.Net;
using AutoMapper;
using Domain.DTOs.InstructorDto;
using Domain.DTOs.StudentDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class InstructorService(DataContext context, IMapper mapper) : IInstructorService
{
    public async Task<Response<GetInstructorDto>> AddInstructor(CreateInstructorDto instructorDto)
    {
        var instructor = mapper.Map<Instructor>(instructorDto);


        await context.Instructors.AddAsync(instructor);
        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetInstructorDto>(instructor);

        return result == 0
            ? new Response<GetInstructorDto>(HttpStatusCode.BadRequest, "Instructor not added!")
            : new Response<GetInstructorDto>(data);
    }

    public async Task<Response<string>> DeleteInstructor(int id)
    {
        var exist = await context.Instructors.FindAsync(id);
        if (exist == null)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Instructor not found");
        }

        context.Instructors.Remove(exist);
        var result = await context.SaveChangesAsync();
        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Instructor not deleted!")
            : new Response<string>("Instructor deleted!");
    }

    public async Task<Response<List<GetInstructorDto>>> GetAll(StudentFilter filter)
    {
        var validfilter = new ValidFilter(filter.PageNumber, filter.PageSize);

        var instructors = context.Instructors.AsQueryable();

        if (filter.Name != null)
        {
            instructors = instructors.Where(s => string.Concat(s.FirstName, " ", s.LastName).ToLower().Contains(filter.Name.ToLower()));
        }

        var mapped = mapper.Map<List<GetInstructorDto>>(instructors);
        var totalRecords = mapped.Count;

        var data = mapped
            .Skip((validfilter.PageNumber - 1) * validfilter.PageSize)
            .Take(validfilter.PageSize)
            .ToList();

        return new PagedResponse<List<GetInstructorDto>>(data, validfilter.PageNumber, validfilter.PageSize,
            totalRecords);
    }

    public async Task<Response<GetInstructorDto>> Update(int id, UpdateInstructorDto instructorDto)
    {
        var exist = await context.Instructors.FindAsync(id);
        if (exist == null)
        {
            return new Response<GetInstructorDto>(HttpStatusCode.NotFound, "Instructor not found");
        }

        exist.FirstName = instructorDto.FirstName;
        exist.LastName = instructorDto.LastName;
        exist.Phone = instructorDto.Phone;
        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetInstructorDto>(exist);

        return result == 0
            ? new Response<GetInstructorDto>(HttpStatusCode.BadRequest, "Instructor not updated")
            : new Response<GetInstructorDto>(data);
    }
}
