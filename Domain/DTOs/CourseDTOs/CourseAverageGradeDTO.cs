namespace Domain.DTOs.CourseDTOs;

public class CourseAverageGradeDTO
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public double AverageGrade { get; set; }
}