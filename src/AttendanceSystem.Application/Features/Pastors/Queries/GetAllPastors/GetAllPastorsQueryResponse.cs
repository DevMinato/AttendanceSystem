using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.Pastors.Queries.GetAllPastors
{
    public class GetAllPastorsQueryResponse : BaseResponse
    {
        public GetAllPastorsQueryResponse() : base() { }
        public PagedResult<PastorsListResultVM> Result { get; set; } = default!;
    }
}