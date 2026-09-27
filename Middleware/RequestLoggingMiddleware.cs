namespace NotesApi.Middleware;
using System.Diagnostics;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context){
        var stopwatch = Stopwatch.StartNew();
       await _next(context);

    stopwatch.Stop();
        Console.WriteLine($"{context.Request.Method} {context.Request.Path} took {stopwatch.ElapsedMilliseconds} ms");
    }
}
