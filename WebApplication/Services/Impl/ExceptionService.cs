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
            Log.Error(exception, "User already exists: {}", exception.Message);
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
            Log.Error(exception, "User not found: {}", exception.Message);
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
            Log.Error(exception, "Token not valid: {}", exception.Message);
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
            Log.Error(exception, "Token expired: {}", exception.Message);
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
            Log.Error(exception, "Dish not found: {}", exception.Message);
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
            Log.Error(exception, "Dish estimation error: {}", exception.Message);
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
            Log.Error(exception, "User not purchased dish: {}", exception.Message);
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
            Log.Error(exception, "Token not found: {}", exception.Message);
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
            Log.Error(exception, "Invalid phone number: {}", exception.Message);
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
            Log.Error(exception, "Basket empty: {}", exception.Message);
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
            Log.Error(exception, "Order already confirmed: {}", exception.Message);
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
            Log.Error(exception, "Internal server error {}", exception.Message);
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