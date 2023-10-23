using System.Text.Json;
using Serilog;
using WebApplication.Exceptions;

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
        }
        catch (UserAlreadyExistsException exception)
        {
            Log.Error(exception, "User already exists");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = exception.Message,
                Time = DateTime.Now
            });
        }
        catch (UserNotFoundException exception)
        {
            Log.Error(exception, "User not found");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = exception.Message,
                Time = DateTime.Now
            });
        }
        catch (TokenNotValidException exception)
        {
            Log.Error(exception, "Token not valid");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = exception.Message,
                Time = DateTime.Now
            });
        }
        catch (TokenExpiredException exception)
        {
            Log.Error(exception, "Token expired");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = exception.Message,
                Time = DateTime.Now
            });
        }
        catch (DishNotFoundException exception)
        {
            Log.Error(exception, "Dish not found");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = exception.Message,
                Time = DateTime.Now
            });
        }
        catch (DishEstimationException exception)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status409Conflict,
                Message = exception.Message,
                Time = DateTime.Now
            });
        }
        catch (UserNotPurchasedDishException exception)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Message = exception.Message,
                Time = DateTime.Now
            });
        }
        catch (TokenNotFoundException exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = exception.Message,
                Time = DateTime.Now
            });
        }
        catch (InvalidPhoneNumberException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = exception.Message,
                Time = DateTime.Now
            });
        }
        catch (BasketEmptyException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = exception.Message,
                Time = DateTime.Now
            });
        }
        catch (OrderAlreadyConfirmedException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = exception.Message,
                Time = DateTime.Now
            });
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Internal server error");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = exception.Message,
                Time = DateTime.Now
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