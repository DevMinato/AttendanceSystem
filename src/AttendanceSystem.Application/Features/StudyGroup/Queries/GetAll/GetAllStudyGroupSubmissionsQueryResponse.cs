using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.StudyGroup.Queries.GetAll
{
    public class GetAllStudyGroupSubmissionsQueryResponse : BaseResponse
    {
        public GetAllStudyGroupSubmissionsQueryResponse() : base() { }
        public PagedResult<StudyGroupSubmissionsListResultVM> Result { get; set; } = default!;
    }
}