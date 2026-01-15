using Microsoft.AspNetCore.Mvc.Filters;
using Rahhal.Common.Views;
using System.Security.Claims;


public class UserInfoFilter : IActionFilter
{
    private readonly UserInfoProvider _userInfoProvider;

    public UserInfoFilter(UserInfoProvider userInfoProvider) 
    {
        _userInfoProvider = userInfoProvider;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity is ClaimsIdentity identity && identity.IsAuthenticated)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null) userId = "-1";   
            _userInfoProvider.userState= new UserState { UserID  =  userId };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}   