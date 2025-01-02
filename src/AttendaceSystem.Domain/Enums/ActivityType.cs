using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Domain.Enums
{
    public enum ActivityType
    {
        [Display(Name = "Attendance")]
        Attendance,
        [Display(Name = "FollowUp")]
        FollowUp,
        MidWeekService,
        [Display(Name = "PrayerGroup")]
        PrayerGroup,
        [Display(Name = "Outreach")]
        Outreach
    }
}