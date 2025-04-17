using System.Net;
using AutoMapper;
using Domain.DTOs.StudentDTOs;
using Domain.Entities;
using Domain.Response;
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

    public async Task<Response<List<GetStudentDTO>>> GetAllStudents()
    {
        var AllStudents = await context.Students
            .ToListAsync();

        if (AllStudents.Count == 0)
            return new Response<List<GetStudentDTO>>(HttpStatusCode.NotFound, "No students found");

        var getStudentsDto = mapper.Map<List<GetStudentDTO>>(AllStudents);
        
        return new Response<List<GetStudentDTO>>(getStudentsDto);
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