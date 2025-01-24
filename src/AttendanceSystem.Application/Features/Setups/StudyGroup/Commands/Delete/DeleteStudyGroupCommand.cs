using AttendanceSystem.Application.Responses;
using MediatR;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Delete
{
    public class DeleteStudyGroupCommand : IRequest<BaseResponse>
    {
        public Guid? StudyGroupId { get; set; }
    }
}