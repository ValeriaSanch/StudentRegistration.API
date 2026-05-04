// <copyright file="ExceptionHandlingMiddleware.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Exceptions;
using StudentRegistration.Domain.Exceptions;

namespace StudentRegistration.Presentation.Middleware;

/// <summary>
/// Global exception handling middleware that maps exceptions to RFC 7807 problem detail responses.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware delegate.</param>
    /// <param name="logger">The logger.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware, catching and formatting exceptions into problem detail responses.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        string traceId = context.TraceIdentifier;

        (int status, string title, string detail) = exception switch
        {
            DomainException => (422, "Error de dominio", exception.Message),
            NotFoundException => (404, "Recurso no encontrado", exception.Message),
            AlreadyExistsException => (409, "Conflicto de datos", exception.Message),
            FluentValidation.ValidationException vex => (400, "Error de validación", string.Join("; ", vex.Errors.Select(e => e.ErrorMessage))),
            _ => (500, "Error interno del servidor", "Ocurrió un error inesperado."),
        };

        if (status == 500)
            _logger.LogError(exception, "Unhandled exception. TraceId: {TraceId}", traceId);
        else if (exception is DomainException)
            _logger.LogWarning("Domain exception: {Message}. TraceId: {TraceId}", exception.Message, traceId);
        else
            _logger.LogWarning("Application exception {Type}: {Message}. TraceId: {TraceId}", exception.GetType().Name, exception.Message, traceId);

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";

        Dictionary<string, object> body = new Dictionary<string, object>
        {
            ["type"] = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ["title"] = title,
            ["status"] = status,
            ["detail"] = detail,
            ["traceId"] = traceId,
        };

        string json = JsonSerializer.Serialize(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
}
