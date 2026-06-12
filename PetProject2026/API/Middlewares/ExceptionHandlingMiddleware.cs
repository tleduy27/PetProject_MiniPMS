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
        }
    }
}
