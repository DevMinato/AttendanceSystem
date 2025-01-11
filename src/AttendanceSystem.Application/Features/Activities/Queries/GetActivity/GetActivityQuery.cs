using MediatR;

namespace AttendanceSystem.Application.Features.Activities.Queries.GetActivity
{
    public class GetActivityQuery : IRequest<GetActivityQueryResponse>
    {
        public Guid Id { get; set; }
    }
}