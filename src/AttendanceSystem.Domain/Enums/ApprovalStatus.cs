using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Domain.Enums
{
    public enum ApprovalStatus
    {
        [Display(Name = "Pending")]
        [Description("Pending")]
        Pending = 1,
        [Display(Name = "Approved")]
        [Description("Approved")]
        Approved = 2,
        [Display(Name = "Rejected")]
        [Description("Rejected")]
        Rejected = 3
    }
}