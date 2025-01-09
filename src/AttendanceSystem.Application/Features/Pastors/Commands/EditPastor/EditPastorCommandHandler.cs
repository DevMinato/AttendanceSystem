using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Features.Fellowships.Commands.EditFellowship;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Pastors.Commands.EditPastor
{
    public class EditPastorCommandHandler : IRequestHandler<EditPastorCommand, BaseResponse>
    {
        private readonly ILogger<EditPastorCommandHandler> _logger;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        private readonly IMapper _mapper;
        public EditPastorCommandHandler(ILogger<EditPastorCommandHandler> logger, IAsyncRepository<Pastor> pastorRepository, IMapper mapper, IAsyncRepository<Fellowship> fellowshipRepository)
        {
            _logger = logger;
            _pastorRepository = pastorRepository;
            _mapper = mapper;
            _fellowshipRepository = fellowshipRepository;
        }

        public async Task<BaseResponse> Handle(EditPastorCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new EditPastorCommandValidator(_pastorRepository, _fellowshipRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var pastor = await _pastorRepository.GetSingleAsync(x => x.Id == request.PastorId);
                if (pastor == null) throw new NotFoundException(nameof(pastor), Constants.ErrorCode_ReportNotFound + $" Pastor with Id {request.PastorId} not found.");

                _mapper.Map(request, pastor, typeof(EditPastorCommand), typeof(Pastor));
                await _pastorRepository.UpdateAsync(pastor);

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