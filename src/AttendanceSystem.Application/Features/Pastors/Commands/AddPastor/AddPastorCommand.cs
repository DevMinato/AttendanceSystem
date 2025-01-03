using AttendanceSystem.Domain.Enums;
using MediatR;

namespace AttendanceSystem.Application.Features.Pastors.Commands.AddPastor
{
    public class AddPastorCommand : IRequest<AddPastorCommandResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public GenderEnum Gender { get; set; }
        public Guid FellowshipId { get; set; }
    }
}