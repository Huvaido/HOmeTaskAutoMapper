namespace Domain.Entities;

public class Instructor
{
    public int InstructorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public List<CourseAssignment> courseAssignments { get; set; }
}