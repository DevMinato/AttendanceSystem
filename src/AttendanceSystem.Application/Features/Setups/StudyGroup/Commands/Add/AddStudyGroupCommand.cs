using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Add
{
    public class AddStudyGroupCommand : IRequest<AddStudyGroupCommandResponse>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? StudyGroupMaterial { get; set; }
        public string? StudyGroupQuestion { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public bool AllowLateSubmission { get; set; } = false;
    }
}