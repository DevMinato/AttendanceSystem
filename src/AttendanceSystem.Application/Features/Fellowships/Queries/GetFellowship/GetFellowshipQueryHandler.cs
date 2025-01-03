using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Fellowships.Queries.GetFellowship
{
    public class GetFellowshipQueryHandler : IRequestHandler<GetFellowshipQuery, GetFellowshipQueryResponse>
    {
        private readonly ILogger<GetFellowshipQueryHandler> _logger;
        private readonly IAsyncRepository<Fellowship> _fellowshipRepository;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        private readonly IMapper _mapper;
        public GetFellowshipQueryHandler(ILogger<GetFellowshipQueryHandler> logger, IAsyncRepository<Fellowship> fellowshipRepository, IMapper mapper, IAsyncRepository<Pastor> pastorRepository)
        {
            _logger = logger;
            _fellowshipRepository = fellowshipRepository;
            _mapper = mapper;
            _pastorRepository = pastorRepository;
        }

        public async Task<GetFellowshipQueryResponse> Handle(GetFellowshipQuery request, CancellationToken cancellationToken)
        {
            var response = new GetFellowshipQueryResponse();
            try
            {
                var fellowship = await _fellowshipRepository.GetSingleAsync(x => x.Id == request.Id, false);
                if (fellowship == null)
                {
                    throw new CustomException(Constants.ErrorCode_RecordNotFound + $"Fellowship with request id {request.Id} not found.");
                }

                var result = _mapper.Map<FellowshipDetailResultVM>(fellowship);

                if(result.PastorId != null)
                {
                    var pastor = await _pastorRepository.GetSingleAsync(x => x.Id == result.PastorId, false);
                    if(pastor != null) result.PastorFullName = $"{pastor.FirstName} {pastor.LastName}";
                }

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