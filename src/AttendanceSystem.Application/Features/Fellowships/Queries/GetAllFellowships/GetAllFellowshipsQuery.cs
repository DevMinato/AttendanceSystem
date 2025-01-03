using AttendanceSystem.Domain.Entities;
using MediatR;

namespace AttendanceSystem.Application.Features.Fellowships.Queries.GetAllFellowships
{
    public class GetAllFellowshipsQuery : PagingModel, IRequest<GetAllFellowshipsQueryResponse>
    {
        public string? Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}