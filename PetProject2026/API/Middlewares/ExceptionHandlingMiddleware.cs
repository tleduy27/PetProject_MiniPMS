using PetProject2026.Domain.Exception;

namespace PetProject2026.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest; // =400
                var errors = ex.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                });

                var response = new
                {
                    status = 400,
                    title = "Validation failed",
                    errors = errors
                };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);

            }
            catch (PetProject2026.Domain.Exception.NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound; // =404

                var response = new
                {
                    status = 404,
                    title = "Resource not found",
                    message = ex.Message
                };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
            //catch (InvalidOperationException ex)
            //{
            //    context.Response.StatusCode = StatusCodes.Status409Conflict; // =409

            //    var response = new
            //    {
            //        status = 409,
            //        title = "Conflict",
            //        message = ex.Message
            //    };
            //    context.Response.ContentType = "application/json";
            //    await context.Response.WriteAsJsonAsync(response);
            //}
            catch (ConflictException ex)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict; // =409
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    status = 409,
                    title = "Conflict",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError; // = 500
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    status = 500,
                    title = "Internal Server Error",
                    message = "Đã có lỗi xảy ra." // KHÔNG lộ ex.Message ra client ở môi trường thật
                });
                // TODO: log ex ra file/console để debug (Phase sau dùng ILogger)
            }
        }
    }
}
