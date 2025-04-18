using System.Net;
using AutoMapper;
using Domain.DTOs.StudentDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StudentService(DataContext context, IMapper mapper) : IStudentService
{
    public async Task<Response<GetStudentDTO>> CreateStudent(CreateStudentDTO createStudent)
    {
        var student = mapper.Map<Student>(createStudent);

        await context.Students.AddAsync(student);
        var result = await context.SaveChangesAsync();

        var getStudentDto = mapper.Map<GetStudentDTO>(student);

        return result == 0
            ? new Response<GetStudentDTO>(HttpStatusCode.BadRequest, "Student created")
            : new Response<GetStudentDTO>(getStudentDto);
    }

    public async Task<Response<GetStudentDTO>> UpdateStudent(int studentId, GetStudentDTO updateStudent)
    {
        var student = await context.Students.FindAsync(studentId);
        if (student == null)
            return new Response<GetStudentDTO>(HttpStatusCode.NotFound, "Student is not found");
        
        student.FirstName = updateStudent.FirstName;
        student.LastName = updateStudent.LastName;
        student.BirthDate = updateStudent.BirthDate;

        var result = await context.SaveChangesAsync();

        var getStudentDto = mapper.Map<GetStudentDTO>(student);

        return result == 0
            ? new Response<GetStudentDTO>(HttpStatusCode.BadRequest, "Student not updated")
            : new Response<GetStudentDTO>(getStudentDto);
    }

    public async Task<Response<string>> DeleteStudent(int StudentId)
    {
        var student = await context.Students.FindAsync(StudentId);
        if (student == null)
            return new Response<string>(HttpStatusCode.NotFound, "Student is not found");

        context.Students.Remove(student);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Student wansn't deleted")
            : new Response<string>("Student deleted successfully");
    }

    public async Task<Response<List<GetStudentDTO>>> GetAllStudents(StudentFilter filter)
    {
      var validfilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var students = context.Students.AsQueryable();

        if (filter.Name != null)
        {
            students = students.Where(s => string.Concat(s.FirstName, " ", s.LastName).ToLower().Contains(filter.Name.ToLower()));
        }

        if (filter.From != null)
        {
            var year = DateTime.UtcNow.Year;
            students = students.Where(s => year - s.BirthDate.Year >= filter.From);
        }

        var mapped = mapper.Map<List<GetStudentDTO>>(students);
        var totalRecords = mapped.Count;
        var data = mapped
                .Skip((validfilter.PageNumber - 1) * validfilter.PageSize)
                .Take(validfilter.PageSize)
                .ToList();
        return new PagedResponse<List<GetStudentDTO>>(data, validfilter.PageNumber, validfilter.PageSize,
                totalRecords);
    }

    //Task4
    public async Task<Response<List<GetStudentDTO>>> StudentsWithNoCourses()
    {
        var students = await context.Students
            .Include(s => s.Enrollments)
            .Where(e => e.Enrollments.Count == 0)
            .ToListAsync();
        
        if (students.Count == 0)
        {
            return new Response<List<GetStudentDTO>>(HttpStatusCode.NotFound, "No students found");
        }

        var getStudentsDto = mapper.Map<List<GetStudentDTO>>(students);

        return new Response<List<GetStudentDTO>>(getStudentsDto);
    }
}