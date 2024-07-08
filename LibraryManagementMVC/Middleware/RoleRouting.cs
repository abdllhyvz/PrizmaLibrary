using System.Security.Claims;

public class RoleBasedRoutingMiddleware
{
    private readonly RequestDelegate _next;

    public RoleBasedRoutingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var roleClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (roleClaim != null)
            {
                var role = roleClaim.Value;

                switch (role)
                {
                    case "Admin":
                        context.Response.Redirect("/Admin/Index");
                        return;
                    case "Officer":
                        context.Response.Redirect("/Officer/Index");
                        return;
                    case "User":
                        context.Response.Redirect("/Home/Index");
                        return;
                }
            }
        }

        await _next(context);
    }
}
