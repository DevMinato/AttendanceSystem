using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AttendanceSystem.Application.Features.StudyGroup.Queries.Get
{
    public class GetStudyGroupSubmissionQueryHandler : IRequestHandler<GetStudyGroupSubmissionQuery, GetStudyGroupSubmissionQueryResponse>
    {
        private readonly ILogger<GetStudyGroupSubmissionQueryHandler> _logger;
        private readonly IAsyncRepository<StudyGroupSubmission> _studyGroupSubmissionRepository;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IMapper _mapper;
        public GetStudyGroupSubmissionQueryHandler(ILogger<GetStudyGroupSubmissionQueryHandler> logger, IAsyncRepository<StudyGroupSubmission> studyGroupSubmissionRepository, 
            IMapper mapper, IAsyncRepository<Member> memberRepository)
        {
            _logger = logger;
            _studyGroupSubmissionRepository = studyGroupSubmissionRepository;
            _mapper = mapper;
            _memberRepository = memberRepository;
        }

        public async Task<GetStudyGroupSubmissionQueryResponse> Handle(GetStudyGroupSubmissionQuery request, CancellationToken cancellationToken)
        {
            var response = new GetStudyGroupSubmissionQueryResponse();
            try
            {
                var includeExpressions = new Expression<Func<StudyGroupSubmission, object>>[]
                {
                    report => report.StudyGroup
                };

                var submission = await _studyGroupSubmissionRepository.GetSingleAsync(x => x.Id == request.Id, false, includeExpressions);
                if (submission == null)
                {
                    throw new CustomException(Constants.ErrorCode_RecordNotFound + $" Study group assignment with request id {request.Id} not found.");
                }

                var result = _mapper.Map<StudyGroupSubmissionDetailResultVM>(submission);

                if (result.MemberId != null)
                {
                    var member = await _memberRepository.GetSingleAsync(x => x.Id == result.MemberId, false);
                    if (member != null) result.MemberFullName = $"{member.FirstName} {member.LastName}";
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