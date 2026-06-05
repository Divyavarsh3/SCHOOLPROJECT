using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.DTOs
{
    /// <summary>
    /// DTO used to update existing teacher details.
    /// </summary>
    public class TeacherUpdateDto
    {
        [Required(ErrorMessage = "Teacher Guid is required.")]
        public Guid TeacherGuid { get; set; }

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