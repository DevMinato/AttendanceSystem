using AttendanceSystem.Application.Features.Auths.Commands.LoginUser;
using AttendanceSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AttendanceSystem.API.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class UserTypeAttribute : Attribute, IAuthorizationFilter
    {
        public UserTypeAttribute(params UserType[] userTypes)
        {
            UserTypes = userTypes;
        }

        public UserType[] UserTypes { get; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (UserResponse)context.HttpContext?.Items["User"];
            if (user == null)
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

            string userType = user.UserType.ToString();
            if (string.IsNullOrWhiteSpace(userType))
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else if (!UserTypes.Any(x => x.ToString() == userType))
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}

