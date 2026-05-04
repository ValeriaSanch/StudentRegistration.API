// <copyright file="StudentRepository.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Interfaces;
using StudentRegistration.Domain.ValueObjects;
using StudentRegistration.Infrastructure.Persistence;

namespace StudentRegistration.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IStudentRepository"/>.
/// </summary>
public sealed class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a student by identifier with enrollments and their subjects loaded.
    /// </summary>
    /// <param name="id">The student identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The student or null.</returns>
    public async Task<Student?> GetByIdAsync(StudentId id, CancellationToken ct)
    {
        return await _context.Students
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Subject)
                    .ThenInclude(sub => sub!.Professor)
            .FirstOrDefaultAsync(s => s.StudentId == id, ct);
    }

    /// <summary>
    /// Retrieves a student by email address.
    /// </summary>
    /// <param name="email">The email value object.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The student or null.</returns>
    public async Task<Student?> GetByEmailAsync(Email email, CancellationToken ct)
    {
        return await _context.Students
            .FirstOrDefaultAsync(s => s.Email == email, ct);
    }

    /// <summary>
    /// Retrieves a student by document number.
    /// </summary>
    /// <param name="documentNumber">The document number.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The student or null.</returns>
    public async Task<Student?> GetByDocumentAsync(string documentNumber, CancellationToken ct)
    {
        return await _context.Students
            .FirstOrDefaultAsync(s => s.DocumentNumber == documentNumber, ct);
    }

    /// <summary>
    /// Retrieves all students.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of all students.</returns>
    public async Task<IReadOnlyList<Student>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Students
            .Include(s => s.Enrollments)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Adds a new student to the context.
    /// </summary>
    /// <param name="student">The student to add.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddAsync(Student student, CancellationToken ct)
    {
        await _context.Students.AddAsync(student, ct);
    }

    /// <summary>
    /// Marks the student entity as modified in the context.
    /// </summary>
    /// <param name="student">The student to update.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A completed task.</returns>
    public Task UpdateAsync(Student student, CancellationToken ct)
    {
        _context.Students.Update(student);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Removes a student from the context by identifier.
    /// </summary>
    /// <param name="id">The student identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteAsync(StudentId id, CancellationToken ct)
    {
        Student? student = await GetByIdAsync(id, ct);
        if (student is not null)
            _context.Students.Remove(student);
    }
}
