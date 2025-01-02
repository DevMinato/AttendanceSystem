using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Domain.Enums
{
    public enum MemberType
    {
        [Display(Name = "Pastor")]
        Pastor,
        [Display(Name = "Workers in Training")]
        WorkersInTraining,
        [Display(Name = "Disciple")]
        Disciple
    }
}