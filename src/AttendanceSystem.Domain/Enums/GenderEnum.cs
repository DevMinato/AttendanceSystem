using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Domain.Enums
{
    public enum GenderEnum
    {
        [Display(Name = "Male")]
        Male,
        [Display(Name = "Female")]
        Female
    }
}