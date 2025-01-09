using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Fellowships.Commands.AddFellowship
{
    public class AddFellowshipCommandHandler : IRequestHandler<AddFellowshipCommand, AddFellowshipCommandResponse>
    {
        private readonly ILogger<AddFellowshipCommandHandler> _logger;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public AddFellowshipCommandHandler(ILogger<AddFellowshipCommandHandler> logger, IAsyncRepository<Fellowship> fellowshipRepository, IMapper mapper, IUserService userService, IAsyncRepository<Pastor> pastorRepository)
        {
            _logger = logger;
            _fellowshipRepository = fellowshipRepository;
            _mapper = mapper;
            _userService = userService;
            _pastorRepository = pastorRepository;
        }

        public async Task<AddFellowshipCommandResponse> Handle(AddFellowshipCommand request, CancellationToken cancellationToken)
        {
            var response = new AddFellowshipCommandResponse();
            try
            {
                var validator = new AddFellowshipCommandValidator(_fellowshipRepository, _pastorRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var fellowship = _mapper.Map<Fellowship>(request);
                fellowship.IsActive = false;
                fellowship.Status = ApprovalStatus.Pending;

                await _fellowshipRepository.AddAsync(fellowship);

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