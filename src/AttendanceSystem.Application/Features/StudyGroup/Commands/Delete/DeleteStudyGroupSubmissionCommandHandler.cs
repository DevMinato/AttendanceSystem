using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.StudyGroup.Commands.Delete
{
    public class DeleteStudyGroupSubmissionCommandHandler : IRequestHandler<DeleteStudyGroupSubmissionCommand, BaseResponse>
    {
        private readonly ILogger<DeleteStudyGroupSubmissionCommandHandler> _logger;
        private readonly IAsyncRepository<StudyGroupSubmission> _studyGroupRepository;
        private readonly IMapper _mapper;
        public DeleteStudyGroupSubmissionCommandHandler(ILogger<DeleteStudyGroupSubmissionCommandHandler> logger, IAsyncRepository<StudyGroupSubmission> studyGroupRepository, IMapper mapper)
        {
            _logger = logger;
            _studyGroupRepository = studyGroupRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(DeleteStudyGroupSubmissionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new DeleteStudyGroupSubmissionCommandValidator(_studyGroupRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var studyGroup = await _studyGroupRepository.GetSingleAsync(x => x.Id == request.StudyGroupSubmissionId);
                if (studyGroup == null) throw new NotFoundException(nameof(studyGroup), Constants.ErrorCode_ReportNotFound + $" Study group submission with Id {request.StudyGroupSubmissionId} not found.");

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