using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AttendanceSystem.API.Utility.Attributes
{
    /*[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class UserPermissionAttribute : Attribute, IAuthorizationFilter
    {
        public UserPermissionAttribute(PermissionEnum permission)
        {
            PermissionRequired = permission;
        }

        public PermissionEnum PermissionRequired { get; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (ApplicationUser)context.HttpContext.Items["User"];
            if (user == null)
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            else
            {
                if (user.UserType == UserType.System)
                {
                    var permissionClaims = user.Roles.SelectMany(x => x.RolePermissions).ToList();
                    if (permissionClaims == null || permissionClaims.Count == 0)
                    {
                        context.Result = new JsonResult(new { message = "You are not allowed to access this resource." }) { StatusCode = StatusCodes.Status403Forbidden };
                    }
                    else if (!permissionClaims.Any(x => x == PermissionRequired.DisplayShortName()))
                    {
                        context.Result = new JsonResult(new { message = "You are not allowed to access this resource." }) { StatusCode = StatusCodes.Status403Forbidden };
                    }
                }
            }
        }
    }*/
}