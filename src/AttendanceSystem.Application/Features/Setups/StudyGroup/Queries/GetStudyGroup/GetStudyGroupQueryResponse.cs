using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Queries.GetStudyGroup
{
    public class GetStudyGroupQueryResponse : BaseResponse
    {
        public GetStudyGroupQueryResponse() : base() { }
        public StudyGroupDetailResultVM Result { get; set; } = default!;
    }
}