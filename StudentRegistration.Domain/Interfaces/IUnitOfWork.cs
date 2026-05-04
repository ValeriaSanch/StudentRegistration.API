// <copyright file="IUnitOfWork.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Domain.Interfaces;

/// <summary>
/// Abstraction for committing a unit of work to the persistence store.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Commits all pending changes in the current unit of work.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> CommitAsync(CancellationToken ct);
}
