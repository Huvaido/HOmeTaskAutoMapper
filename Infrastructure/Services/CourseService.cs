using System.Net;
using AutoMapper;
using Domain.DTOs.CourseDTOs;
using Domain.DTOs.StudentDTOs;
using Domain.Entities;
using Domain.Response;
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

    public async Task<Response<List<GetCourseDTO>>> GetAllCourses()
    {
        var courses = await context.Courses.ToListAsync();

        if (courses.Count == 0)
            return new Response<List<GetCourseDTO>>(HttpStatusCode.NotFound, "No courses found");

        var getCoursesDTO = mapper.Map<List<GetCourseDTO>>(courses);

        return new Response<List<GetCourseDTO>>(getCoursesDTO);
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

}