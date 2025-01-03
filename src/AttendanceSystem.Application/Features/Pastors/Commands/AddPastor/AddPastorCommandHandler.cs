using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Pastors.Commands.AddPastor
{
    public class AddPastorCommandHandler : IRequestHandler<AddPastorCommand, AddPastorCommandResponse>
    {
        private readonly ILogger<AddPastorCommandHandler> _logger;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public AddPastorCommandHandler(ILogger<AddPastorCommandHandler> logger, IAsyncRepository<Pastor> pastorRepository, IMapper mapper, IUserService userService, IAsyncRepository<Fellowship> fellowshipRepository)
        {
            _logger = logger;
            _pastorRepository = pastorRepository;
            _mapper = mapper;
            _userService = userService;
            _fellowshipRepository = fellowshipRepository;
        }

        public async Task<AddPastorCommandResponse> Handle(AddPastorCommand request, CancellationToken cancellationToken)
        {
            var response = new AddPastorCommandResponse();
            try
            {
                var validator = new AddPastorCommandValidator(_pastorRepository, _fellowshipRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                var pastor = _mapper.Map<Pastor>(request);
                pastor.IsActive = false;
                pastor.Status = ApprovalStatus.Pending;

                await _pastorRepository.AddAsync(pastor);

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