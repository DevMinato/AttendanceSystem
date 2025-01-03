using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Domain.Enums
{
    public enum FollowUpType
    {
        [Display(Name = "Visitation")]
        Visitation = 1,
        [Display(Name = "Teaching")]
        Teaching = 2
    }
}
