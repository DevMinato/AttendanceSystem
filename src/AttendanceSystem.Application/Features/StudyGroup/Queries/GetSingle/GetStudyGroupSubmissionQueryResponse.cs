using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.StudyGroup.Queries.Get
{
    public class GetStudyGroupSubmissionQueryResponse : BaseResponse
    {
        public GetStudyGroupSubmissionQueryResponse() : base() { }
        public StudyGroupSubmissionDetailResultVM Result { get; set; } = default!;
    }
}