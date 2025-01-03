using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Domain.Enums
{
    public enum UserType
    {
        [Display(Name = "System")]
        System,
        [Display(Name = "Member")]
        Member
    }
}