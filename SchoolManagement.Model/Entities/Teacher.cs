namespace SchoolManagement.Model.Entities
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        public Guid TeacherGuid { get; set; }

        public int SubjectId { get; set; }

        public string TeacherName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
}