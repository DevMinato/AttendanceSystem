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

namespace AttendanceSystem.Application.Features.StudyGroup.Commands.Add
{
    public class AddStudyGroupSubmissionCommandHandler : IRequestHandler<AddStudyGroupSubmissionCommand, BaseResponse>
    {
        private readonly ILogger<AddStudyGroupSubmissionCommandHandler> _logger;
        private readonly IAsyncRepository<StudyGroupSubmission> _studyGroupSubmissionRepository;
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IDocumentUpload _documentUpload;
        public AddStudyGroupSubmissionCommandHandler(ILogger<AddStudyGroupSubmissionCommandHandler> logger, IAsyncRepository<StudyGroupSubmission> studyGroupSubmissionRepository, 
            IMapper mapper, IUserService userService, IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository, IDocumentUpload documentUpload)
        {
            _logger = logger;
            _studyGroupSubmissionRepository = studyGroupSubmissionRepository;
            _mapper = mapper;
            _userService = userService;
            _studyGroupRepository = studyGroupRepository;
            _documentUpload = documentUpload;
        }

        public async Task<BaseResponse> Handle(AddStudyGroupSubmissionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new AddStudyGroupSubmissionCommandValidator(_studyGroupRepository, _studyGroupSubmissionRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var studyGroup = _mapper.Map<StudyGroupSubmission>(request);


                // Proceed with document upload if approval is not required
                var docUpload = await _documentUpload.UploadDocuments(_userService.UserDetails().UserId.ToString(), new List<DocumentRequest> { request.Upload },
                                                                      UserType.Member.DisplayName());

                if (!docUpload.Status && docUpload.ErrorResponse != null) throw new CustomException("Study Group assignment upload failed.");

                await _studyGroupSubmissionRepository.AddAsync(studyGroup);

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