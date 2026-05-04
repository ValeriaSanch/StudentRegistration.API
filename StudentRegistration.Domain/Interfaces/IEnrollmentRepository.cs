// <copyright file="IEnrollmentRepository.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Domain.Interfaces;

/// <summary>
/// Output port for enrollment query operations.
/// </summary>
public interface IEnrollmentRepository
{
    /// <summary>
    /// Retrieves all enrollments for a specific student.
    /// </summary>
    /// <param name="studentId">The student identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of enrollments.</returns>
    Task<IReadOnlyList<Enrollment>> GetByStudentAsync(StudentId studentId, CancellationToken ct);

    /// <summary>
    /// Retrieves all students actively enrolled in a subject, excluding a given student.
    /// </summary>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="excludeStudentId">The student to exclude from the results.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of classmate students.</returns>
    Task<IReadOnlyList<Student>> GetClassmatesAsync(SubjectId subjectId, StudentId excludeStudentId, CancellationToken ct);
}
