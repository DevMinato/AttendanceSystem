using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Edit
{
    public class EditStudyGroupCommand : IRequest<BaseResponse>
    {
        public Guid? StudyGroupId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? StudyGroupMaterial { get; set; }
        public string? StudyGroupQuestion { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public bool AllowLateSubmission { get; set; } = false;
    }
}