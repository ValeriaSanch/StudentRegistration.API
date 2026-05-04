// <copyright file="IStudentRepository.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Domain.Interfaces;

/// <summary>
/// Output port for student persistence operations.
/// </summary>
public interface IStudentRepository
{
    /// <summary>
    /// Retrieves a student by their unique identifier, including their enrollments.
    /// </summary>
    /// <param name="id">The student identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The student, or <c>null</c> if not found.</returns>
    Task<Student?> GetByIdAsync(StudentId id, CancellationToken ct);

    /// <summary>
    /// Retrieves a student by their email address.
    /// </summary>
    /// <param name="email">The email value object.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The student, or <c>null</c> if not found.</returns>
    Task<Student?> GetByEmailAsync(Email email, CancellationToken ct);

    /// <summary>
    /// Retrieves a student by their document number.
    /// </summary>
    /// <param name="documentNumber">The document number.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The student, or <c>null</c> if not found.</returns>
    Task<Student?> GetByDocumentAsync(string documentNumber, CancellationToken ct);

    /// <summary>
    /// Retrieves all students in the system.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of all students.</returns>
    Task<IReadOnlyList<Student>> GetAllAsync(CancellationToken ct);

    /// <summary>
    /// Persists a new student.
    /// </summary>
    /// <param name="student">The student to add.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(Student student, CancellationToken ct);

    /// <summary>
    /// Updates an existing student.
    /// </summary>
    /// <param name="student">The student to update.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(Student student, CancellationToken ct);

    /// <summary>
    /// Deletes a student by their identifier (logical delete is handled at the entity level).
    /// </summary>
    /// <param name="id">The student identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(StudentId id, CancellationToken ct);
}
