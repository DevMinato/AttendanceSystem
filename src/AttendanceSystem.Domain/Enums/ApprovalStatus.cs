using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Domain.Enums
{
    public enum ApprovalStatus
    {
        [Display(Name = "Pending")]
        Pending = 1,
        [Display(Name = "Approved")]
        Approved = 2,
        [Display(Name = "Rejected")]
        Rejected = 3
    }
}