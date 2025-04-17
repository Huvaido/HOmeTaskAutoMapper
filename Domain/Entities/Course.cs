using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Course
{
    public int CourseId { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<Enrollment> Enrollments { get; set; } = new();
    public List<CourseAssignment> CourseAssignments { get; set; } = new();
}