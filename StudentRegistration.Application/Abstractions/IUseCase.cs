// <copyright file="IUseCase.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Abstractions;

/// <summary>
/// Defines a use case that accepts a request and returns a response.
/// </summary>
/// <typeparam name="TRequest">The input type.</typeparam>
/// <typeparam name="TResponse">The output type.</typeparam>
public interface IUseCase<TRequest, TResponse>
{
    /// <summary>
    /// Executes the use case with the given request.
    /// </summary>
    /// <param name="request">The input data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The use case response.</returns>
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken ct);
}

/// <summary>
/// Defines a use case that accepts a request and returns no value.
/// </summary>
/// <typeparam name="TRequest">The input type.</typeparam>
public interface IUseCase<TRequest>
{
    /// <summary>
    /// Executes the use case with the given request.
    /// </summary>
    /// <param name="request">The input data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync(TRequest request, CancellationToken ct);
}
