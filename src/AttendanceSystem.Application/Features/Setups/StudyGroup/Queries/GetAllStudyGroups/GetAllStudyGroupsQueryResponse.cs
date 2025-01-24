using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Queries.GetAllStudyGroups
{
    public class GetAllStudyGroupsQueryResponse : BaseResponse
    {
        public GetAllStudyGroupsQueryResponse() : base() { }
        public PagedResult<StudyGroupsListResultVM> Result { get; set; } = default!;
    }
}