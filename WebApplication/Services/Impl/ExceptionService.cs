using System.Text.Json;

namespace WebApplication.Services.Impl;

public class ExceptionService
{
    private readonly RequestDelegate _next;
    
    public ExceptionService(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        } catch (UserAlreadyExists exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = exception.Message
            });
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = exception.Message,
            });
        }
    }
    
    private class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = null!;
        public DateTime Time { get; set; } = DateTime.Now;
        
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}