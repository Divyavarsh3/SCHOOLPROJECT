namespace SchoolManagement.Model.Entities
{
    public class Student
    {
        public Guid StudentGuid { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
}