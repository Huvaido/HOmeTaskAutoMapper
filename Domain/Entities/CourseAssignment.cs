namespace Domain.Entities;

public class CourseAssignment
{
    public int CourseAssignmentId { get; set; }
    public int CourseId { get; set; }
    public int InstructorId { get; set; }
    public Course Course { get; set; }
    public Instructor Instructor { get; set; }
}