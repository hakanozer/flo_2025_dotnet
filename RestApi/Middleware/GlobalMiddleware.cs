using System.Text;
using System.Text.Json;

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
        /*
        foreach (var key in context.Request.Form.Keys)
        {
            var value = context.Request.Form[key];
            Console.WriteLine($"Key: {key}, Value: {value}");
        }
        */
        if (context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
        {
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (!string.IsNullOrWhiteSpace(body))
                {
                    try
                    {
                        var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(body);
                        foreach (var kvp in jsonObject)
                        {
                            Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Invalid JSON: {ex.Message}");
                    }
                }
            }
        }
        await _next(context);
    }

}