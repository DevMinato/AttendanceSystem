using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Members.Commands.AddMember
{
    public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, AddMemberCommandResponse>
    {
        private readonly ILogger<AddMemberCommandHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public AddMemberCommandHandler(ILogger<AddMemberCommandHandler> logger, IAsyncRepository<Member> memberRepository, IMapper mapper, IUserService userService)
        {
            _logger = logger;
            _memberRepository = memberRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<AddMemberCommandResponse> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            var response = new AddMemberCommandResponse();
            try
            {
                var validator = new AddMemberCommandValidator();
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var member = _mapper.Map<Member>(request);
                member.DisciplerId = _userService.UserDetails().UserId;
                member.FellowshipId = _userService.UserDetails().GroupId.Value;
                member.IsActive = false;
                member.Status = ApprovalStatus.Pending;

                await _memberRepository.AddAsync(member);

                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (CustomException ex)
            {
                response.Message = ex.Message;
            }
            catch (ValidationException ex)
            {
                response.ValidationErrors = ex.ValidationErrors;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response.Success = false;
                response.Message = Constants.ErrorResponse;
            }

            return response;
        }
    }
}