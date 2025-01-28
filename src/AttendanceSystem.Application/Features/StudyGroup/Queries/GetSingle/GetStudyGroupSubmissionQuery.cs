using MediatR;

namespace AttendanceSystem.Application.Features.StudyGroup.Queries.Get
{
    public class GetStudyGroupSubmissionQuery : IRequest<GetStudyGroupSubmissionQueryResponse>
    {
        public Guid Id { get; set; }
    }
}