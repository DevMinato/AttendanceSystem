using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Domain.Enums
{
    public enum WeekEnum
    {
        [Display(Name = "Week1")]
        Week1,
        [Display(Name = "Week2")]
        Week2,
        [Display(Name = "Week3")]
        Week3,
        [Display(Name = "Week4")]
        Week4,
        [Display(Name = "Week5")]
        Week5
    }
}