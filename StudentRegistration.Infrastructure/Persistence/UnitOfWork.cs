// <copyright file="UnitOfWork.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using StudentRegistration.Domain.Interfaces;

namespace StudentRegistration.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of the unit of work pattern.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Commits all pending changes to the database.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The number of state entries written.</returns>
    public Task<int> CommitAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}
