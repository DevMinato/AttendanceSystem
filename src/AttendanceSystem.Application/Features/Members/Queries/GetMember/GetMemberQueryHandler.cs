using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Application.Features.Members.Queries.GetMember
{
    public class GetMemberQueryHandler : IRequestHandler<GetMemberQuery, GetMemberQueryResponse>
    {
        private readonly ILogger<GetMemberQueryHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IMapper _mapper;
        public GetMemberQueryHandler(ILogger<GetMemberQueryHandler> logger, IAsyncRepository<Member> memberRepository, IMapper mapper)
        {
            _logger = logger;
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<GetMemberQueryResponse> Handle(GetMemberQuery request, CancellationToken cancellationToken)
        {
            var response = new GetMemberQueryResponse();
            try
            {
                var member = await _memberRepository.GetSingleAsync(x => x.Id == request.Id, false, x => x.Fellowship);
                if (member == null)
                {
                    throw new CustomException(Constants.ErrorCode_MemberRecordNotFound + $"Member with request id {request.Id} not found.");
                }

                var result = _mapper.Map<MemberDetailResultVM>(member);

                if(result.DisciplerId != null)
                {
                    var discipler = await _memberRepository.GetSingleAsync(x => x.Id == result.DisciplerId, false, x=> x.Fellowship);
                    if(discipler != null) result.DisciplerFullName = $"{discipler.FirstName} {discipler.LastName}";
                }

                response.Result = result;
                response.Success = true;
                response.Message = Constants.SuccessResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString() + "{Microservice}", Constants.Microservice);
                response.Message = $"Error completing member querying request.";
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