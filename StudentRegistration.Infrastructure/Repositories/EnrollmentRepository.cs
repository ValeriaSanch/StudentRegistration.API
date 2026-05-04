// <copyright file="EnrollmentRepository.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Interfaces;
using StudentRegistration.Domain.ValueObjects;
using StudentRegistration.Infrastructure.Persistence;

namespace StudentRegistration.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IEnrollmentRepository"/>.
/// </summary>
public sealed class EnrollmentRepository : IEnrollmentRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnrollmentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public EnrollmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves enrollments for a specific student.
    /// </summary>
    /// <param name="studentId">The student identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of enrollments.</returns>
    public async Task<IReadOnlyList<Enrollment>> GetByStudentAsync(StudentId studentId, CancellationToken ct)
    {
        return await _context.Enrollments
            .Include(e => e.Subject)
                .ThenInclude(s => s!.Professor)
            .Where(e => e.StudentId == studentId)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Retrieves all students with an active enrollment in a subject, excluding one student.
    /// </summary>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="excludeStudentId">The student to exclude.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of classmate students.</returns>
    public async Task<IReadOnlyList<Student>> GetClassmatesAsync(SubjectId subjectId, StudentId excludeStudentId, CancellationToken ct)
    {
        return await _context.Enrollments
            .Include(e => e.Subject)
            .Where(e => e.SubjectId == subjectId && e.IsActive && e.StudentId != excludeStudentId)
            .Select(e => e.StudentId)
            .Distinct()
            .Join(_context.Students, id => id, s => s.StudentId, (id, s) => s)
            .ToListAsync(ct);
    }
}
