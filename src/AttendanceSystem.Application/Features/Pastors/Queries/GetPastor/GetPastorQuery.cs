using MediatR;

namespace AttendanceSystem.Application.Features.Pastors.Queries.GetPastor
{
    public class GetPastorQuery : IRequest<GetPastorQueryResponse>
    {
        public Guid Id { get; set; }
    }
}