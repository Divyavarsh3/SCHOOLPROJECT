using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.DTOs
{
    /// <summary>
    /// DTO used to create a new teacher.
    /// </summary>
    public class TeacherCreateDto
    {
        [Required(ErrorMessage = "Subject Id is required.")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Teacher Name is required.")]
        [StringLength(100, ErrorMessage = "Teacher Name cannot exceed 100 characters.")]
        public string TeacherName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}