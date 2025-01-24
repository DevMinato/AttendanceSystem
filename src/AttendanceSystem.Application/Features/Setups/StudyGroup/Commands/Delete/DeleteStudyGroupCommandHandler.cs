using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Delete
{
    public class DeleteStudyGroupCommandHandler : IRequestHandler<DeleteStudyGroupCommand, BaseResponse>
    {
        private readonly ILogger<DeleteStudyGroupCommandHandler> _logger;
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        private readonly IMapper _mapper;
        public DeleteStudyGroupCommandHandler(ILogger<DeleteStudyGroupCommandHandler> logger, IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository, IMapper mapper)
        {
            _logger = logger;
            _studyGroupRepository = studyGroupRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(DeleteStudyGroupCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new DeleteStudyGroupCommandValidator(_studyGroupRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var studyGroup = await _studyGroupRepository.GetSingleAsync(x => x.Id == request.StudyGroupId);
                if(studyGroup == null) throw new NotFoundException(nameof(studyGroup), Constants.ErrorCode_ReportNotFound + $" Study group with Id {request.StudyGroupId} not found.");

                studyGroup.IsDeleted = true;
                await _studyGroupRepository.UpdateAsync(studyGroup);

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