using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Members.Commands.EditMember
{
    public class EditMemberCommandHandler : IRequestHandler<EditMemberCommand, BaseResponse>
    {
        private readonly ILogger<EditMemberCommandHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        private readonly IMapper _mapper;
        public EditMemberCommandHandler(ILogger<EditMemberCommandHandler> logger, IAsyncRepository<Member> memberRepository, IMapper mapper, IAsyncRepository<Fellowship> fellowshipRepository)
        {
            _logger = logger;
            _memberRepository = memberRepository;
            _mapper = mapper;
            _fellowshipRepository = fellowshipRepository;
        }

        public async Task<BaseResponse> Handle(EditMemberCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new EditMemberCommandValidator(_memberRepository, _fellowshipRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var member = await _memberRepository.GetSingleAsync(x => x.Id == request.MemberId);
                if (member == null) throw new NotFoundException(nameof(member), Constants.ErrorCode_ReportNotFound + $"Member with Id {request.MemberId} not found.");

                var updateObj = _mapper.Map<Member>(request);
                await _memberRepository.UpdateAsync(updateObj);

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