using Application.DTO.Response;
using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Presentation.Middleware;

public static class ExceptionHandlerMiddlewareConfig
{
    public static WebApplication UseCustomExceptionHandler(this WebApplication app)
    {

        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            ExceptionHandler = async context =>
            {
                Exception? exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                ErrorResponse errorResponse = GlobalExceptionHandler.Handle(exception);

                context.Response.StatusCode = (int)errorResponse.StatusCode;

                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode = context.Response.StatusCode,
                    message = errorResponse.Message,
                    timestamp = errorResponse.Timestamp
                });
            }
        });

        return app;
    }
}
