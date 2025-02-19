using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Models;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AttendanceSystem.Application.Features.Auths.Commands.LoginUser
{
    internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
    {
        private readonly ILogger<LoginUserCommandHandler> _logger;
        private readonly IAsyncRepository<Member> _memberRepository;
        private readonly IAsyncRepository<Pastor> _pastorRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Member> _memberPasswordHasher;
        private readonly IPasswordHasher<Pastor> _pastorPasswordHasher;
        private readonly IConfiguration _config;
        public LoginUserCommandHandler(ILogger<LoginUserCommandHandler> logger, IAsyncRepository<Member> memberRepository, IMapper mapper, IPasswordHasher<Member> memberPasswordHasher,
            IPasswordHasher<Pastor> pastorPasswordHasher, IAsyncRepository<Pastor> pastorRepository, IConfiguration config)
        {
            _logger = logger;
            _memberRepository = memberRepository;
            _mapper = mapper;
            _memberPasswordHasher = memberPasswordHasher;
            _pastorPasswordHasher = pastorPasswordHasher;
            _pastorRepository = pastorRepository;
            _config = config;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var response = new LoginUserCommandResponse();
            try
            {
                var validator = new LoginUserCommandValidator(_memberRepository, _pastorRepository);
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new ValidationException(validationResult);

                if (request.MemberType == MemberType.WorkersInTraining)
                {
                    var member = await _memberRepository.GetSingleAsync(m => m.Email == request.Email, false, x => x.Fellowship);
                    if (member == null ||
                        _memberPasswordHasher.VerifyHashedPassword(member, member.PasswordHash, request.Password) != PasswordVerificationResult.Success)
                    {
                        if (member != null)
                        {
                            if (member.LoginAttempt > 3)
                            {
                                member.IsPasswordLocked = true;
                                member.IsActive = false;
                                await _memberRepository.UpdateAsync(member);
                            }

                            member.LoginAttempt += 1;
                            await _memberRepository.UpdateAsync(member);                            
                        }
                        throw new CustomException($"Invalid email or password, {Constants.ErrorCode_InvalidDetails}");
                    }

                    //Check if user is active
                    if (!member.IsActive) throw new CustomException($"Account is inactive, {Constants.ErrorCode_InactiveAccount}");

                    //Generate Token
                    var userResponse = new
                    {
                        UserId = member.Id,
                        GroupId = member.FellowshipId,
                        FullName = string.Join(' ', member.FirstName, member.LastName),
                        PhoneNumber = member.PhoneNumber,
                        EmailAddress = member.Email,
                        UserType = member.MemberType.ToString(),
                        GroupName = member.Fellowship.Name,
                        LastLoginDate = member.LastLoginDate,
                    };
                    var token = GenerateToken(userResponse, DateTime.Now, _config);
                    token.UserData = new UserData
                    {
                        UserId = userResponse.UserId,
                        GroupId = userResponse.GroupId,
                        FullName = userResponse.GroupName,
                        PhoneNumber = userResponse.PhoneNumber,
                        EmailAddress = userResponse.EmailAddress,
                        UserType = userResponse.UserType,
                        GroupName = userResponse.GroupName,
                        LastLoginDate = userResponse.LastLoginDate,
                    };
                    response.Result = token;

                    member.LoginAttempt = 0;
                    member.LoginAccessDate = DateTime.Now;
                    member.LastLoginDate = DateTime.Now;
                }
                else if (request.MemberType == MemberType.Pastor)
                {
                    var pastor = await _pastorRepository.GetSingleAsync(m => m.Email == request.Email, false, x => x.Fellowship);
                    if (pastor == null ||
                        _pastorPasswordHasher.VerifyHashedPassword(pastor, pastor.PasswordHash, request.Password) != PasswordVerificationResult.Success)
                    {
                        if (pastor != null)
                        {
                            if (pastor.LoginAttempt > 3)
                            {
                                pastor.IsPasswordLocked = true;
                                pastor.IsActive = false;
                                await _pastorRepository.UpdateAsync(pastor);
                            }

                            pastor.LoginAttempt += 1;
                            await _pastorRepository.UpdateAsync(pastor);
                        }
                        throw new CustomException($"Invalid email or password, {Constants.ErrorCode_InvalidDetails}");
                    }

                    //Check if user is active
                    if (!pastor.IsActive) throw new CustomException($"Account is inactive, {Constants.ErrorCode_InactiveAccount}");

                    //Generate Token
                    var userResponse = new
                    {
                        UserId = pastor.Id,
                        GroupId = pastor.FellowshipId,
                        FullName = string.Join(' ', pastor.FirstName, pastor.LastName),
                        PhoneNumber = pastor.PhoneNumber,
                        EmailAddress = pastor.Email,
                        UserType = MemberType.Pastor.ToString(),
                        GroupName = pastor.Fellowship.Name,
                        LastLoginDate = pastor.LastLoginDate,
                    };
                    var token = GenerateToken(userResponse, DateTime.Now, _config);
                    token.UserData = new UserData
                    {
                        UserId = userResponse.UserId,
                        GroupId = userResponse.GroupId,
                        FullName = userResponse.GroupName,
                        PhoneNumber = userResponse.PhoneNumber,
                        EmailAddress = userResponse.EmailAddress,
                        UserType = userResponse.UserType,
                        GroupName = userResponse.GroupName,
                        LastLoginDate = userResponse.LastLoginDate,
                    };
                    response.Result = token;

                    pastor.LoginAttempt = 0;
                    pastor.LoginAccessDate = DateTime.Now;
                    pastor.LastLoginDate = DateTime.Now;
                    await _pastorRepository.UpdateAsync(pastor);
                }

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

        private JwtAuthResult GenerateToken(dynamic obj, DateTime now, IConfiguration _configuration)
        {
            // Get Jwt Settiongs
            var _jwtTokenConfig = JwtConfiguration.GetJwtSettings();

            // Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_jwtTokenConfig.Key.PadRight(32, ' '));
            //var key = Encoding.UTF8.GetBytes(_jwtTokenConfig.Key);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfig.Key));


            Guid userId = obj.UserId;
            Guid groupId = obj.GroupId;
            string usertype = obj.UserType;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, obj.UserId.ToString()),
                new Claim(ClaimTypes.GroupSid, obj.GroupId.ToString()),
                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject( new UserResponse(){
                    UserId=obj.UserId,
                    FullName=obj.FullName,
                    EmailAddress=obj.EmailAddress,
                    MobileNumber=obj.PhoneNumber,
                    GroupId = obj.GroupId,
                    UserType = obj.UserType,
                    Permissions = null,
                    GroupName=obj.GroupName,
                    LastLoginDate =obj.LastLoginDate,
                }))
            };

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtTokenConfig.Issuer,
                audience: _jwtTokenConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtTokenConfig.DurationInMinutes),
                signingCredentials: signingCredentials);
            //return jwtSecurityToken;


            /*var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                (
                    new[]{
                        new Claim(ClaimTypes.NameIdentifier, obj.UserId.ToString()),
                        new Claim(ClaimTypes.GroupSid, obj.GroupId.ToString()),
                        new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject( new UserResponse(){
                            UserId=obj.UserId,
                            FullName=obj.FullName,
                            EmailAddress=obj.EmailAddress,
                            MobileNumber=obj.PhoneNumber,
                            GroupId = obj.GroupId,
                            UserType = obj.UserType,
                            Permissions = null,
                            GroupName=obj.GroupName,
                            LastLoginDate =obj.LastLoginDate,
                        }))
                    }
                ),
                Expires = DateTime.Now.AddMinutes(_jwtTokenConfig.DurationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Audience = _jwtTokenConfig.Audience, // Set the correct audience
                Issuer = _jwtTokenConfig.Issuer,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);*/

            return new JwtAuthResult
            {
                AccessToken = tokenHandler.WriteToken(jwtSecurityToken),
                RefreshToken = null,
                ExpirationTime = now.AddMinutes(_jwtTokenConfig.DurationInMinutes)
            };
        }
    }
}