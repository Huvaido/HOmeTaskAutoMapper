using System.Net;
using AutoMapper;
using Domain.DTOs.CourseDTOs;
using Domain.DTOs.StudentDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Response;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CourseService(DataContext context, IMapper mapper) : ICourseService
{
    public async Task<Response<GetCourseDTO>> CreateCourse(CreateCourseDTO createCourse)
    {
        var course = mapper.Map<Course>(createCourse);

        await context.Courses.AddAsync(course);
        var result = await context.SaveChangesAsync();

        var getCourseDto = mapper.Map<GetCourseDTO>(course);

        return result == 0
            ? new Response<GetCourseDTO>(HttpStatusCode.BadRequest, "Course wasn't created")
            : new Response<GetCourseDTO>(getCourseDto);
    }

    public async Task<Response<GetCourseDTO>> UpdateCourse(int courseId, GetCourseDTO updateCourse)
    {
        var course = await context.Courses.FindAsync(courseId);

        if (course == null)
            return new Response<GetCourseDTO>(HttpStatusCode.NotFound, "Course not found");

        course.Title = updateCourse.Title;
        course.Description = updateCourse.Description;
        course.Price = updateCourse.Price;

        var result = await context.SaveChangesAsync();
        var getCourseDto = mapper.Map<GetCourseDTO>(course);

        return result == 0
            ? new Response<GetCourseDTO>(HttpStatusCode.BadRequest, "Course wasn't updated")
            : new Response<GetCourseDTO>(getCourseDto);
    }

    public async Task<Response<string>> DeleteCourse(int courseId)
    {
        var course = await context.Courses.FindAsync(courseId);

        if (course == null)
            return new Response<string>(HttpStatusCode.NotFound, "Course not found");

        context.Courses.Remove(course);
        var result = await context.SaveChangesAsync();

        return result == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Course wasn't deleted")
            : new Response<string>("Course deleted successfully");
    }

    public async Task<Response<List<GetCourseDTO>>> GetAllCourses(StudentFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var courses = context.Courses.AsQueryable();

        var mapped = mapper.Map<List<GetCourseDTO>>(courses);
        var totalRecords = mapped.Count;

        var data = mapped
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        return new PagedResponse<List<GetCourseDTO>>(data, validFilter.PageNumber, validFilter.PageSize,
            totalRecords);
    }


    //Task1
    public async Task<Response<List<CourseWithStudentCount>>> GetStudentsCount()
    {
        var courses = await context.Courses
            .Include(c => c.Enrollments)
            .ToListAsync();

        if (courses.Count == 0)
        {
            return new Response<List<CourseWithStudentCount>>(HttpStatusCode.NotFound, "No courses found");
        }

        var courseDtos = mapper.Map<List<CourseWithStudentCount>>(courses);

        return new Response<List<CourseWithStudentCount>>(courseDtos);
    }

    //Task2
    public async Task<Response<List<CourseAverageGradeDTO>>> GetCoursesAverageGrades()
    {
        var courses = await context.Courses
            .Include(c => c.Enrollments)
            .ToListAsync();

        if (courses.Count == 0)
        {
            return new Response<List<CourseAverageGradeDTO>>(HttpStatusCode.NotFound, "No courses found");
        }

        var result = mapper.Map<List<CourseAverageGradeDTO>>(courses);

        return new Response<List<CourseAverageGradeDTO>>(result);
    }

    //Task3
    public async Task<Response<List<CourseCountStudents>>>GetCountCourseStudents(){
            var coursesStudent = context.Students
            .Select(s => new
            {
             StudentName = $"{s.FirstName} {s.LastName}",
             CourseCount = s.Enrollments.Count()
            })
            .ToList();

        if (coursesStudent.Count == 0)
        {
            return new Response<List<CourseCountStudents>>(HttpStatusCode.NotFound, "No courses found");
        }

        var result = mapper.Map<List<CourseCountStudents>>(coursesStudent);
        
        return new Response<List<CourseCountStudents>>(result);

    }

   //Task4
   public async Task<Response<List<CourseNotStudents>>>GetNotStudentCourse(){
        var studentsWitNotCourses = context.Students
        .Where(s => !s.Enrollments.Any())
        .Select(s => new
         {
            Studentid = s.StudentId,
            FullName = $"{s.FirstName} {s.LastName}"
        })
        .ToList();
        if (studentsWitNotCourses.Count == 0)
        {
            return new Response<List<CourseNotStudents>>(HttpStatusCode.NotFound, "No courses found");
        }
        var result = mapper.Map<List<CourseNotStudents>>(studentsWitNotCourses);
        
        return new Response<List<CourseNotStudents>>(result);
   }
   // task5

  
}