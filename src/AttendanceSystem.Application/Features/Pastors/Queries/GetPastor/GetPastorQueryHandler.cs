using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Pastors.Queries.GetPastor
{
    public class GetPastorQueryHandler : IRequestHandler<GetPastorQuery, GetPastorQueryResponse>
    {
        private readonly ILogger<GetPastorQueryHandler> _logger;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        private readonly IMapper _mapper;
        public GetPastorQueryHandler(ILogger<GetPastorQueryHandler> logger, IMapper mapper, IAsyncRepository<Pastor> pastorRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _pastorRepository = pastorRepository;
        }

        public async Task<GetPastorQueryResponse> Handle(GetPastorQuery request, CancellationToken cancellationToken)
        {
            var response = new GetPastorQueryResponse();
            try
            {
                var pastor = await _pastorRepository.GetSingleAsync(x => x.Id == request.Id, false);
                if (pastor == null)
                {
                    throw new CustomException(Constants.ErrorCode_RecordNotFound + $"Pastor with request id {request.Id} not found.");
                }

                var result = _mapper.Map<PastorDetailResultVM>(pastor);

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing pastor querying request.";
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