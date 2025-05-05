using System.Security.Claims;

namespace quotes_backend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Mvc.ControllerBase;

public static class Rights
{
    public static bool hasRights(ClaimsPrincipal User)
    {
        if (User.HasClaim(c => c.Type == "Rights" && c.Value == "True"))
            return true;
        return false;
    }
}