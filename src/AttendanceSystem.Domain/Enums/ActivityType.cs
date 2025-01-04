﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AttendanceSystem.Domain.Enums
{
    public enum ActivityType
    {
        [Display(Name = "Attendance")]
        Attendance,
        [Display(Name = "FollowUp")]
        FollowUp,
        [Display(Name = "Mid-week Service")]
        MidWeekService,
        [Display(Name = "PrayerGroup")]
        PrayerGroup,
        [Display(Name = "Outreach")]
        Outreach
    }
}