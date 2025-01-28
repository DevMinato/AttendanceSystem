using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.StudyGroup.Commands.Edit
{
    public class EditStudyGroupSubmissionCommand : IRequest<BaseResponse>
    {
        public Guid? SubmissionId { get; set; }
        public Guid? StudyGroupId { get; set; }
        public Guid? MemberId { get; set; }
        public DocumentRequest? Upload { get; set; }
    }
}