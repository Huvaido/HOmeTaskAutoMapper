namespace Domain.DTOs.CourseDTOs;

public class CourseNotStudents
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
}