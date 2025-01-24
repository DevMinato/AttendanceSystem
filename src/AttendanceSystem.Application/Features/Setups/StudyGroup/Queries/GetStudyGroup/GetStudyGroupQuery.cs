using MediatR;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Queries.GetStudyGroup
{
    public class GetStudyGroupQuery : IRequest<GetStudyGroupQueryResponse>
    {
        public Guid Id { get; set; }
    }
}