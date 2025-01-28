using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Contracts.Utilities;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AttendanceSystem.Domain.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.StudyGroup.Commands.Edit
{
    public class EditStudyGroupSubmissionCommandHandler : IRequestHandler<EditStudyGroupSubmissionCommand, BaseResponse>
    {
        private readonly ILogger<EditStudyGroupSubmissionCommandHandler> _logger;
        private readonly IAsyncRepository<StudyGroupSubmission> _studyGroupSubmissionRepository;
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IDocumentUpload _documentUpload;
        private readonly IAsyncRepository<Document> _documentRepository;
        public EditStudyGroupSubmissionCommandHandler(ILogger<EditStudyGroupSubmissionCommandHandler> logger, IAsyncRepository<StudyGroupSubmission> studyGroupSubmissionRepository,
            IMapper mapper, IUserService userService, IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository, IDocumentUpload documentUpload,
            IAsyncRepository<Document> documentRepository)
        {
            _logger = logger;
            _studyGroupSubmissionRepository = studyGroupSubmissionRepository;
            _mapper = mapper;
            _userService = userService;
            _studyGroupRepository = studyGroupRepository;
            _documentUpload = documentUpload;
            _documentRepository = documentRepository;
        }

        public async Task<BaseResponse> Handle(EditStudyGroupSubmissionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new EditStudyGroupSubmissionCommandValidator(_studyGroupRepository, _studyGroupSubmissionRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var submission = await _studyGroupSubmissionRepository.GetSingleAsync(x => x.Id == request.SubmissionId);
                if (submission == null) throw new NotFoundException(nameof(StudyGroupSubmission), Constants.ErrorCode_RecordNotFound + $" Study group assignment with Id {request.SubmissionId} not found.");

                _mapper.Map(request, submission, typeof(EditStudyGroupSubmissionCommand), typeof(StudyGroupSubmission));

                // Proceed with document upload if approval is not required
                var docUpload = await _documentUpload.UploadDocuments(request.SubmissionId.ToString(), new List<DocumentRequest> { request.Upload },
                                                                      UserType.Member.DisplayName());

                if (!docUpload.Status && docUpload.ErrorResponse != null) throw new CustomException("Study Group assignment upload failed.");

                await _studyGroupSubmissionRepository.UpdateAsync(submission);

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