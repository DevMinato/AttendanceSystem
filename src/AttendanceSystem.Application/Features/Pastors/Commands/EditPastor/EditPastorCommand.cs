using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Pastors.Commands.EditPastor
{
    public class EditPastorCommand : IRequest<BaseResponse>
    {
        public Guid? PastorId { get; set; }
        public Guid? FellowshipId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public GenderEnum Gender { get; set; }
    }
}