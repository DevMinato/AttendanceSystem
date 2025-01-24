using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Setups.StudyGroup.Queries.GetStudyGroup
{
    public class GetStudyGroupQueryHandler : IRequestHandler<GetStudyGroupQuery, GetStudyGroupQueryResponse>
    {
        private readonly ILogger<GetStudyGroupQueryHandler> _logger;
        private readonly IAsyncRepository<Domain.Entities.StudyGroup> _studyGroupRepository;
        private readonly IMapper _mapper;
        public GetStudyGroupQueryHandler(ILogger<GetStudyGroupQueryHandler> logger, IAsyncRepository<Domain.Entities.StudyGroup> studyGroupRepository, IMapper mapper)
        {
            _logger = logger;
            _studyGroupRepository = studyGroupRepository;
            _mapper = mapper;
        }

        public async Task<GetStudyGroupQueryResponse> Handle(GetStudyGroupQuery request, CancellationToken cancellationToken)
        {
            var response = new GetStudyGroupQueryResponse();
            try
            {
                var fellowship = await _studyGroupRepository.GetSingleAsync(x => x.Id == request.Id, false);
                if (fellowship == null)
                {
                    throw new CustomException(Constants.ErrorCode_RecordNotFound + $" Study group with request id {request.Id} not found.");
                }

                var result = _mapper.Map<StudyGroupDetailResultVM>(fellowship);

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing fellowship querying request.";
            }
            catch (CustomException ex)
            {
                _logger.LogError(ex.ToString(), ex.Message);
                response.Message = ex.Message;
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