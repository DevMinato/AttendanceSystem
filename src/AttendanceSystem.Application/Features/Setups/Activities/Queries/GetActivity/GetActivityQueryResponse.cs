using AttendanceSystem.Application.Responses;

namespace AttendanceSystem.Application.Features.Setups.Activities.Queries.GetActivity
{
    public class GetActivityQueryResponse : BaseResponse
    {
        public GetActivityQueryResponse() : base() { }
        public ActivityDetailResultVM Result { get; set; } = default!;
    }
}