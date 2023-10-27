namespace WebApplication.MiddleWares;

public static class MiddlewareExtension
{
    public static void UseExceptionHandlingMiddlewares(this Microsoft.AspNetCore.Builder.WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}