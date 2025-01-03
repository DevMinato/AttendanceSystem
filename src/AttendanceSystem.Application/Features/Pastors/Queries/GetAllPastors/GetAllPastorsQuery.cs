using AttendanceSystem.Domain.Entities;
using MediatR;

namespace AttendanceSystem.Application.Features.Pastors.Queries.GetAllPastors
{
    public class GetAllPastorsQuery : PagingModel, IRequest<GetAllPastorsQueryResponse>
    {
        public string? Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}