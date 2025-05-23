using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Diagnostics;
using System.Security.Claims;

namespace EmocineSveikataServer.Filters
{
    public class LoggingActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var user = context.HttpContext.User;
                var username = user?.Identity?.IsAuthenticated == true ? user.Identity.Name : "Anonymous";

                var rolesList = user?.Claims
                    .Where(c => c.Type == ClaimTypes.Role && !string.IsNullOrWhiteSpace(c.Value))
                    .Select(c => c.Value)
                    .ToList() ?? new List<string>();
                var roles = rolesList.Count != 0 ? string.Join(", ", rolesList) : "Anonymous";

                var controller = context.Controller.GetType().Name;
                var action = context.ActionDescriptor.DisplayName;

                Log.Information($"User: {username}; Roles: {roles}; Executing: {controller}.{action}");
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error in LoggingActionFilter: {ex.StackTrace}");
            }
        }
    }
}
