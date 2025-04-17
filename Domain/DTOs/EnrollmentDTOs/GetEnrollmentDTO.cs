namespace Domain.DTOs.EnrollmentDTOs;

public class GetEnrollmentDTO
{
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollDate { get; set; }
    public int Grade { get; set; }
}