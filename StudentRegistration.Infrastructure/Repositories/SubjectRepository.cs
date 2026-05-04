// <copyright file="SubjectRepository.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Interfaces;
using StudentRegistration.Domain.ValueObjects;
using StudentRegistration.Infrastructure.Persistence;

namespace StudentRegistration.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="ISubjectRepository"/>.
/// </summary>
public sealed class SubjectRepository : ISubjectRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SubjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a subject by identifier, including its professor.
    /// </summary>
    /// <param name="id">The subject identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The subject or null.</returns>
    public async Task<Subject?> GetByIdAsync(SubjectId id, CancellationToken ct)
    {
        return await _context.Subjects
            .Include(s => s.Professor)
            .FirstOrDefaultAsync(s => s.SubjectId == id, ct);
    }

    /// <summary>
    /// Retrieves all subjects including their professors.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of all subjects.</returns>
    public async Task<IReadOnlyList<Subject>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Subjects
            .Include(s => s.Professor)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Retrieves subjects by professor identifier.
    /// </summary>
    /// <param name="professorId">The professor identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of subjects for the professor.</returns>
    public async Task<IReadOnlyList<Subject>> GetByProfessorAsync(ProfessorId professorId, CancellationToken ct)
    {
        return await _context.Subjects
            .Include(s => s.Professor)
            .Where(s => s.ProfessorId == professorId)
            .ToListAsync(ct);
    }
}
