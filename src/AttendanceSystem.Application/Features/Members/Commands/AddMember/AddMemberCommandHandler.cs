﻿using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Features.Members.Queries.GetAllMembers;
using AttendanceSystem.Application.Features.Members.Queries.GetMember;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AttendanceSystem.Domain.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Members.Commands.AddMember
{
    public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, AddMemberCommandResponse>
    {
        private readonly ILogger<AddMemberCommandHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public AddMemberCommandHandler(ILogger<AddMemberCommandHandler> logger, IAsyncRepository<Member> memberRepository, IMapper mapper, IUserService userService, IAsyncRepository<Fellowship> fellowshipRepository)
        {
            _logger = logger;
            _memberRepository = memberRepository;
            _mapper = mapper;
            _userService = userService;
            _fellowshipRepository = fellowshipRepository;
        }

        public async Task<AddMemberCommandResponse> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            var response = new AddMemberCommandResponse();
            try
            {
                var validator = new AddMemberCommandValidator(_memberRepository, _fellowshipRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var member = _mapper.Map<Member>(request);
                member.DisciplerId = _userService.UserDetails().UserId;
                member.FellowshipId = _userService.UserDetails().GroupId.Value;
                member.IsActive = false;
                member.Status = ApprovalStatus.Pending;

                await _memberRepository.AddAsync(member);

                var result = new MembersListResultVM
                {
                    Id = member.Id,
                    DisciplerFullName = _userService.UserDetails().FullName,
                    FellowshipId = member.FellowshipId,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    PhoneNumber = member.PhoneNumber,
                    Email = member.Email,
                    DisciplerId = member.DisciplerId,
                    FellowshipName = _userService.UserDetails().GroupName,
                    Gender = member.Gender.DisplayName(),
                    Status = member.Status.DisplayName(),
                };

                response.Result = result;
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