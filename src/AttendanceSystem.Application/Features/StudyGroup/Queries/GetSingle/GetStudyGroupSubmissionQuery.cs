using MediatR;

namespace AttendanceSystem.Application.Features.StudyGroup.Queries.GetSingle
{
    public class GetStudyGroupSubmissionQuery : IRequest<GetStudyGroupSubmissionQueryResponse>
    {
        public Guid Id { get; set; }
    }
}