using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Fellowships.Commands.DeleteFellowship
{
    public class DeleteFellowshipCommandHandler : IRequestHandler<DeleteFellowshipCommand, BaseResponse>
    {
        private readonly ILogger<DeleteFellowshipCommandHandler> _logger;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        private readonly IMapper _mapper;
        public DeleteFellowshipCommandHandler(ILogger<DeleteFellowshipCommandHandler> logger, IAsyncRepository<Fellowship> fellowshipRepository, IMapper mapper)
        {
            _logger = logger;
            _fellowshipRepository = fellowshipRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(DeleteFellowshipCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new DeleteFellowshipCommandValidator(_fellowshipRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var member = await _fellowshipRepository.GetSingleAsync(x => x.Id == request.FellowshipId);
                if(member == null) throw new NotFoundException(nameof(member), Constants.ErrorCode_ReportNotFound + $" Fellowship with Id {request.FellowshipId} not found.");

                member.IsDeleted = true;
                await _fellowshipRepository.UpdateAsync(member);

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