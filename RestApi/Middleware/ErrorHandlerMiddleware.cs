public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        
        var ip = context.Connection.RemoteIpAddress.ToString();
        //var sessionId = context.Session.Id;
        var headers = context.Request.Headers;
        //Console.WriteLine($"Session {sessionId}");
        Console.WriteLine($"Request from {ip}");
        foreach (var header in headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            await response.WriteAsync(error.Message);
        }
    }
}