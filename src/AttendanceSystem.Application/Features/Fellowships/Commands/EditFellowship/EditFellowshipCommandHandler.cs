using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Fellowships.Commands.EditFellowship
{
    public class EditFellowshipCommandHandler : IRequestHandler<EditFellowshipCommand, BaseResponse>
    {
        private readonly ILogger<EditFellowshipCommandHandler> _logger;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        private readonly IMapper _mapper;
        public EditFellowshipCommandHandler(ILogger<EditFellowshipCommandHandler> logger, IAsyncRepository<Fellowship> fellowshipRepository, IMapper mapper, IAsyncRepository<Pastor> pastorRepository)
        {
            _logger = logger;
            _fellowshipRepository = fellowshipRepository;
            _mapper = mapper;
            _pastorRepository = pastorRepository;
        }

        public async Task<BaseResponse> Handle(EditFellowshipCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new EditFellowshipCommandValidator(_fellowshipRepository, _pastorRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var fellowship = await _fellowshipRepository.GetSingleAsync(x => x.Id == request.FellowshipId);
                if (fellowship == null) throw new NotFoundException(nameof(fellowship), Constants.ErrorCode_ReportNotFound + $" Fellowship with Id {request.FellowshipId} not found.");

                _mapper.Map(request, fellowship, typeof(EditFellowshipCommand), typeof(Fellowship));
                await _fellowshipRepository.UpdateAsync(fellowship);

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