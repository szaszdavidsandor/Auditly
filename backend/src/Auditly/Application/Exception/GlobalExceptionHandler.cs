using System.Diagnostics.CodeAnalysis;
using System.Net;
using Application.DTO.Response;

namespace Application.Exceptions
{
    public static class GlobalExceptionHandler
    {
        [DoesNotReturn]
        public static ErrorResponse ThrowInvalidArgument()
        {
            throw new ErrorResponse(HttpStatusCode.BadRequest, "Invalid request argument provided.");
        }

        [DoesNotReturn]
        public static ErrorResponse ThrowUnauthorized()
        {
            throw new ErrorResponse(HttpStatusCode.Unauthorized, "You are not authorized to access this resource.");
        }

        [DoesNotReturn]
        public static ErrorResponse ThrowNotFound()
        {
            throw new ErrorResponse(HttpStatusCode.NotFound, "The requested resource was not found.");
        }

        [DoesNotReturn]
        public static ErrorResponse ThrowInvalidOperation()
        {
            throw new ErrorResponse(HttpStatusCode.Conflict, "The operation is not valid in the current state.");
        }

        [DoesNotReturn]
        public static ErrorResponse ThrowNotSupported()
        {
            throw new ErrorResponse(HttpStatusCode.UnsupportedMediaType, "The requested operation or media type is not supported.");
        }

        public static ErrorResponse Handle(Exception? exception)
        {
            return exception is ErrorResponse errorResponse
                ? errorResponse
                : HandleUnexpected();
        }

        public static ErrorResponse HandleUnexpected()
        {
            return new ErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }
}
