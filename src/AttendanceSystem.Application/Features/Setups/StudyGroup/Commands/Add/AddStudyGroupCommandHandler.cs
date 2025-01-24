using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Commands.Add
{
    public class AddStudyGroupCommandHandler : IRequestHandler<AddStudyGroupCommand, AddStudyGroupCommandResponse>
    {
        private readonly ILogger<AddStudyGroupCommandHandler> _logger;
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public AddStudyGroupCommandHandler(ILogger<AddStudyGroupCommandHandler> logger, IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository, IMapper mapper, IUserService userService)
        {
            _logger = logger;
            _studyGroupRepository = studyGroupRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<AddStudyGroupCommandResponse> Handle(AddStudyGroupCommand request, CancellationToken cancellationToken)
        {
            var response = new AddStudyGroupCommandResponse();
            try
            {
                var validator = new AddStudyGroupCommandValidator(_studyGroupRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var studyGroup = _mapper.Map<Domain.Entities.StudyGroup>(request);
                studyGroup.Year = request.From.Value.Year.ToString();
                studyGroup.Month = request.From.Value.Month.ToString();
                studyGroup.Week = Helper.GetWeek(request.From.Value, request.To.Value);

                await _studyGroupRepository.AddAsync(studyGroup);

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