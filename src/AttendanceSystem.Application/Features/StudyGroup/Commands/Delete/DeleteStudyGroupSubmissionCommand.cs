using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.StudyGroup.Commands.Delete
{
    public class DeleteStudyGroupSubmissionCommand : IRequest<BaseResponse>
    {
        public Guid? StudyGroupSubmissionId { get; set; }
    }
}