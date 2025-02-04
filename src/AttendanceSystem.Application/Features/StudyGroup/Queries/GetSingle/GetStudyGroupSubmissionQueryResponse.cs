using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.StudyGroup.Queries.GetSingle
{
    public class GetStudyGroupSubmissionQueryResponse : BaseResponse
    {
        public GetStudyGroupSubmissionQueryResponse() : base() { }
        public StudyGroupSubmissionDetailResultVM Result { get; set; } = default!;
    }
}