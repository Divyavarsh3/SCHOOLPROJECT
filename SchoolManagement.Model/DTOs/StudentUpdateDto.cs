using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.DTOs
{
    /// <summary>
    /// DTO used to update existing student details.
    /// </summary>
    public class StudentUpdateDto
    {
        [Required(ErrorMessage = "Student Guid is required.")]
        public Guid StudentGuid { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        [StringLength(100, ErrorMessage = "Student Name cannot exceed 100 characters.")]
        public string StudentName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }
    }
}