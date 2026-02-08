using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HomeChefss.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				await HandleExceptionAsync(context, ex);
			}
		}

		private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var statusCode = exception switch
			{
				ArgumentException => StatusCodes.Status400BadRequest,
				KeyNotFoundException => StatusCodes.Status404NotFound,
				UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
				_ => StatusCodes.Status500InternalServerError
			};

			var problemDetails = new ProblemDetails
			{
				Status = statusCode,
				Title = "An error occurred while processing your request.",
				Detail = exception.Message,
				Instance = context.Request.Path
			};

			context.Response.ContentType = "application/problem+json";
			context.Response.StatusCode = statusCode;

			await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
		}
	}

}
