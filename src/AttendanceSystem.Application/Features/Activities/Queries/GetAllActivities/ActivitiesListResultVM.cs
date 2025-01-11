using AttendanceSystem.Domain.Enums;

namespace AttendanceSystem.Application.Features.Activities.Queries.GetAllActivities
{
    public class ActivitiesListResultVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ActivityType Type { get; set; }
    }
}