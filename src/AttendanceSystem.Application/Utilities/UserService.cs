using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AttendanceSystem.Application.Utilities
{
    public class TokenUserData
    {
        public Guid UserId { get; set; }
        public string UserType { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string GroupName { get; set; }
        public Guid? GroupId { get; set; }
        public List<string>? Permissions { get; set; }
    }

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public UserService(IHttpContextAccessor contextAccessor)
        {
            _HttpContextAccessor = contextAccessor;
        }

        public TokenUserData UserDetails()
        {
            TokenUserData detail = null;
            if (_HttpContextAccessor.HttpContext != null)
            {
                detail = JsonConvert.DeserializeObject<TokenUserData>(_HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            }
            return detail;
        }
    }
    public interface IUserService
    {
        TokenUserData UserDetails();
    }
}

