using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Domain.Enums
{
    public enum DocumentType
    {
        [Display(Name = "Study Group Assignment")]
        StudyGroupAssignment,
        [Display(Name = "Others")]
        Others
    }
}