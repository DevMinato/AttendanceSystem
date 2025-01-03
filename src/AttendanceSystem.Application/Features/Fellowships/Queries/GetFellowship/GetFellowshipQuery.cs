using MediatR;

namespace AttendanceSystem.Application.Features.Fellowships.Queries.GetFellowship
{
    public class GetFellowshipQuery : IRequest<GetFellowshipQueryResponse>
    {
        public Guid Id { get; set; }
    }
}