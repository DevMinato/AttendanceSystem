using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Auths.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseResponse>
    {
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Member> _memberPasswordHasher;
        private readonly IPasswordHasher<Pastor> _pastorPasswordHasher;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, IAsyncRepository<Member> memberRepository, IMapper mapper, IPasswordHasher<Member> memberPasswordHasher,
            IPasswordHasher<Pastor> pastorPasswordHasher, IAsyncRepository<Pastor> pastorRepository, IAsyncRepository<Fellowship> fellowshipRepository)
        {
            _logger = logger;
            _memberRepository = memberRepository;
            _mapper = mapper;
            _memberPasswordHasher = memberPasswordHasher;
            _pastorPasswordHasher = pastorPasswordHasher;
            _pastorRepository = pastorRepository;
            _fellowshipRepository = fellowshipRepository;
        }

        public async Task<BaseResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new CreateUserCommandValidator(_memberRepository, _pastorRepository, _fellowshipRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                if (request.MemberType == MemberType.WorkersInTraining)
                {
                    var member = new Member
                    {
                        Id = Guid.NewGuid(),
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                        FellowshipId = request.FellowshipId.Value,
                        IsActive = false,
                        Status = ApprovalStatus.Pending,
                    };
                    member.PasswordHash = _memberPasswordHasher.HashPassword(member, request.Password);

                    await _memberRepository.AddAsync(member);
                }
                else if(request.MemberType == MemberType.Pastor)
                {
                    var pastor = new Pastor
                    {
                        Id = Guid.NewGuid(),
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                        FellowshipId = request.FellowshipId.Value,
                        IsActive = false,
                        Status = ApprovalStatus.Pending,
                    };
                    pastor.PasswordHash = _pastorPasswordHasher.HashPassword(pastor, request.Password);

                    await _pastorRepository.AddAsync(pastor);
                }

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