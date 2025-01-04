using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Members.Commands.DeleteMember
{
    public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, BaseResponse>
    {
        private readonly ILogger<DeleteMemberCommandHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IMapper _mapper;
        public DeleteMemberCommandHandler(ILogger<DeleteMemberCommandHandler> logger, IAsyncRepository<Member> memberRepository, IMapper mapper)
        {
            _logger = logger;
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new DeleteMemberCommandValidator(_memberRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var member = await _memberRepository.GetSingleAsync(x => x.Id == request.MemberId);
                if(member == null) throw new NotFoundException(nameof(member), Constants.ErrorCode_ReportNotFound + $" Member with Id {request.MemberId} not found.");

                member.IsDeleted = true;
                await _memberRepository.UpdateAsync(member);

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