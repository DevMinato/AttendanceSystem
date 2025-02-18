﻿namespace AttendanceSystem.Application.Features.Reports.Followup.Queries.GetAll
{
    public class FollowupReportListResultVM
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public string MemberFullName { get; set; }
        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int TotalFollowUps { get; set; }
        //public List<FollowUpDetailResultVM>? FollowUpDetails { get; set; }
    }
}