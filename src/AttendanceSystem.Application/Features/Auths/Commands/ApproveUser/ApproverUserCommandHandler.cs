using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Auths.Commands.ApproveUser
{
    public class ApproverUserCommandHandler : IRequestHandler<ApproverUserCommand, BaseResponse>
    {
        private readonly ILogger<ApproverUserCommandHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        private readonly IMapper _mapper;
        public ApproverUserCommandHandler(ILogger<ApproverUserCommandHandler> logger, IAsyncRepository<Member> memberRepository, IMapper mapper, IAsyncRepository<Pastor> pastorRepository)
        {
            _logger = logger;
            _memberRepository = memberRepository;
            _mapper = mapper;
            _pastorRepository = pastorRepository;
        }

        public async Task<BaseResponse> Handle(ApproverUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new ApproverUserCommandValidator();
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                if (request.MemberType == MemberType.WorkersInTraining || request.MemberType == MemberType.Disciple)
                {
                    var member = await _memberRepository.GetSingleAsync(x => x.Id == request.UserId);
                    if (member == null) throw new NotFoundException(nameof(member), $"Member with Id {request.UserId} not found, ({Constants.ErrorCode_MemberRecordNotFound})");
                    else
                    {
                        if (member.Status == ApprovalStatus.Pending)
                        {
                            if (request.Status == ApprovalStatus.Approved)
                            {
                                member.Status = ApprovalStatus.Approved;
                                member.IsActive = true;
                            }
                            else if (request.Status == ApprovalStatus.Rejected)
                            {
                                member.Status = ApprovalStatus.Rejected;
                                member.IsActive = false;
                            }
                            await _memberRepository.UpdateAsync(member);
                        }
                    }
                }

                if (request.MemberType == MemberType.Pastor)
                {
                    var pastor = await _pastorRepository.GetSingleAsync(x => x.Id == request.UserId);
                    if (pastor == null) throw new NotFoundException(nameof(pastor), $"Member with Id {request.UserId} not found, ({Constants.ErrorCode_MemberRecordNotFound})");
                    else
                    {
                        if (pastor.Status == ApprovalStatus.Pending)
                        {
                            if (request.Status == ApprovalStatus.Approved)
                            {
                                pastor.Status = ApprovalStatus.Approved;
                                pastor.IsActive = true;
                            }
                            else if (request.Status == ApprovalStatus.Rejected)
                            {
                                pastor.Status = ApprovalStatus.Rejected;
                                pastor.IsActive = false;
                            }
                            await _pastorRepository.UpdateAsync(pastor);
                        }
                    }
                }

                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (CustomException ex)
            {
                response.Message = ex.Message;
            }
            catch (NotFoundException e)
            {
                response.Message = e.Message;
                response.Success = false;
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
