using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Pastors.Commands.DeletePastor
{
    public class DeletePastorCommandHandler : IRequestHandler<DeletePastorCommand, BaseResponse>
    {
        private readonly ILogger<DeletePastorCommandHandler> _logger;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        private readonly IMapper _mapper;
        public DeletePastorCommandHandler(ILogger<DeletePastorCommandHandler> logger, IAsyncRepository<Pastor> pastorRepository, IMapper mapper)
        {
            _logger = logger;
            _pastorRepository = pastorRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(DeletePastorCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new DeletePastorCommandValidator(_pastorRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var pastor = await _pastorRepository.GetSingleAsync(x => x.Id == request.PastorId);
                if(pastor == null) throw new NotFoundException(nameof(pastor), Constants.ErrorCode_ReportNotFound + $" Pastor with Id {request.PastorId} not found.");

                pastor.IsDeleted = true;
                await _pastorRepository.UpdateAsync(pastor);

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