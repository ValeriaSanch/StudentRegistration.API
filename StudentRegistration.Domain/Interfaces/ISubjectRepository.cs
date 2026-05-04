// <copyright file="ISubjectRepository.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Domain.Interfaces;

/// <summary>
/// Output port for subject query operations.
/// </summary>
public interface ISubjectRepository
{
    /// <summary>
    /// Retrieves a subject by its unique identifier, including its professor.
    /// </summary>
    /// <param name="id">The subject identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The subject, or <c>null</c> if not found.</returns>
    Task<Subject?> GetByIdAsync(SubjectId id, CancellationToken ct);

    /// <summary>
    /// Retrieves all subjects, including their professors.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of all subjects.</returns>
    Task<IReadOnlyList<Subject>> GetAllAsync(CancellationToken ct);

    /// <summary>
    /// Retrieves all subjects taught by a specific professor.
    /// </summary>
    /// <param name="professorId">The professor identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of subjects for the given professor.</returns>
    Task<IReadOnlyList<Subject>> GetByProfessorAsync(ProfessorId professorId, CancellationToken ct);
}
