using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.StudyGroup.Commands.Add
{
    public class AddStudyGroupSubmissionCommand : IRequest<BaseResponse>
    {
        public Guid? StudyGroupId { get; set; }
        public Guid? MemberId { get; set; }
        public DocumentRequest? Upload { get; set; }
    }
}