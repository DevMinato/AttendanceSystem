using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Responses;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Edit
{
    public class EditStudyGroupCommandHandler : IRequestHandler<EditStudyGroupCommand, BaseResponse>
    {
        private readonly ILogger<EditStudyGroupCommandHandler> _logger;
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        private readonly IMapper _mapper;
        public EditStudyGroupCommandHandler(ILogger<EditStudyGroupCommandHandler> logger, IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository, IMapper mapper)
        {
            _logger = logger;
            _studyGroupRepository = studyGroupRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(EditStudyGroupCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            try
            {
                var validator = new EditStudyGroupCommandValidator(_studyGroupRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var studyGroup = await _studyGroupRepository.GetSingleAsync(x => x.Id == request.StudyGroupId);
                if (studyGroup == null) throw new NotFoundException(nameof(studyGroup), Constants.ErrorCode_RecordNotFound + $" StudyGroup with Id {request.StudyGroupId} not found.");

                studyGroup.Year = request.From.Value.Year.ToString();
                studyGroup.Month = request.From.Value.Month.ToString();
                studyGroup.Week = Helper.GetWeek(request.From.Value, request.To.Value);

                _mapper.Map(request, studyGroup, typeof(EditStudyGroupCommand), typeof(Domain.Entities.StudyGroup));
                await _studyGroupRepository.UpdateAsync(studyGroup);

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