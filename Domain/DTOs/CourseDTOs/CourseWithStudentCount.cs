namespace Domain.DTOs.CourseDTOs;

public class CourseWithStudentCount
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
}