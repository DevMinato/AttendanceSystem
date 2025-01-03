using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.Fellowships.Queries.GetAllFellowships
{
    public class GetAllFellowshipsQueryResponse : BaseResponse
    {
        public GetAllFellowshipsQueryResponse() : base() { }
        public PagedResult<FellowshipsListResultVM> Result { get; set; } = default!;
    }
}