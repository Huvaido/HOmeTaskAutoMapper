namespace Domain.DTOs.EnrollmentDTOs;

public class CreateEnrollmentDTO
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollDate { get; set; }
    public int Grade { get; set; }
}