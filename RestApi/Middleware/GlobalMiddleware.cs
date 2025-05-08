public class GlobalMiddleware
{

    private readonly RequestDelegate _next;
    public GlobalMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string path = context.Request.Path;
        long time = DateTime.Now.Ticks;
        string ip = context.Connection.RemoteIpAddress.ToString();
        Console.WriteLine($"Request {path} from {ip} at {time}");
        // aa.com -> mobile rest
        // a1.com
        /*
        var userAgent = context.Request.Headers["User-Agent"].ToString().ToLower();
        if (userAgent.Contains("android") || userAgent.Contains("iphone") || userAgent.Contains("ipad"))
        {
            await _next(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access forbidden: Only Android and iOS users are allowed.");
        }
        */
        await _next(context);
    }

}