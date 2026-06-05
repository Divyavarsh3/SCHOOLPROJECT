using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.DTOs
{
    /// <summary>
    /// DTO used to create a new student.
    /// </summary>
    public class StudentCreateDto
    {
        [Required(ErrorMessage = "Class Id is required.")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        [StringLength(100, ErrorMessage = "Student Name cannot exceed 100 characters.")]
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }
    }
}