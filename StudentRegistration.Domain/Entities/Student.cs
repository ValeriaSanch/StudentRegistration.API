// <copyright file="Student.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using StudentRegistration.Domain.Exceptions;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Domain.Entities;

/// <summary>
/// Represents a student in the registration system.
/// Acts as the aggregate root for the student enrollment context.
/// </summary>
public sealed class Student
{
    private readonly List<Enrollment> _enrollments = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Student"/> class.
    /// Required by EF Core.
    /// </summary>
    private Student()
    {
    }

    /// <summary>
    /// Gets the unique identifier of the student.
    /// </summary>
    public StudentId StudentId { get; private set; }

    /// <summary>
    /// Gets the full name of the student.
    /// </summary>
    public string FullName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the email address of the student.
    /// </summary>
    public Email Email { get; private set; } = null!;

    /// <summary>
    /// Gets the document (ID) number of the student.
    /// </summary>
    public string DocumentNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the UTC timestamp when the student was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp of the last update, or null if never updated.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the student account is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the collection of enrollments belonging to this student.
    /// </summary>
    public IReadOnlyList<Enrollment> Enrollments => _enrollments.AsReadOnly();

    /// <summary>
    /// Creates a new active <see cref="Student"/>.
    /// </summary>
    /// <param name="fullName">The student's full name.</param>
    /// <param name="email">The validated email value object.</param>
    /// <param name="documentNumber">The student's document number.</param>
    /// <returns>A newly created <see cref="Student"/> instance.</returns>
    public static Student Create(string fullName, Email email, string documentNumber)
    {
        return new Student
        {
            StudentId = StudentId.New(),
            FullName = fullName,
            Email = email,
            DocumentNumber = documentNumber,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
        };
    }

    /// <summary>
    /// Adds a new enrollment for the given subject, enforcing domain invariants.
    /// </summary>
    /// <param name="subject">The subject to enroll the student in.</param>
    /// <exception cref="StudentInactiveException">Thrown when the student is inactive (BR-04).</exception>
    /// <exception cref="MaxEnrollmentsExceededException">Thrown when the student already has 3 active enrollments (BR-01).</exception>
    /// <exception cref="SameProfessorConflictException">Thrown when the student already has a subject with the same professor (BR-02).</exception>
    /// <exception cref="DuplicateEnrollmentException">Thrown when the student is already enrolled in this subject (BR-09).</exception>
    public void AddEnrollment(Subject subject)
    {
        if (!IsActive)
            throw new StudentInactiveException();

        List<Enrollment> active = _enrollments.Where(e => e.IsActive).ToList();

        if (active.Count >= 3)
            throw new MaxEnrollmentsExceededException();

        if (active.Any(e => e.ProfessorId == subject.ProfessorId))
            throw new SameProfessorConflictException();

        if (active.Any(e => e.SubjectId == subject.SubjectId))
            throw new DuplicateEnrollmentException();

        _enrollments.Add(Enrollment.Create(StudentId, subject.SubjectId, subject.ProfessorId));
    }

    /// <summary>
    /// Updates the student's full name and document number.
    /// </summary>
    /// <param name="fullName">The new full name.</param>
    /// <param name="documentNumber">The new document number.</param>
    /// <exception cref="StudentInactiveException">Thrown when the student is inactive.</exception>
    public void Update(string fullName, string documentNumber)
    {
        if (!IsActive)
            throw new StudentInactiveException();

        FullName = fullName;
        DocumentNumber = documentNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Logically deletes the student and cancels all active enrollments (CU-05).
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        foreach (Enrollment enrollment in _enrollments.Where(e => e.IsActive))
            enrollment.Cancel();
    }

    /// <summary>
    /// Loads a persisted enrollment collection into this student. Used by the repository after hydration.
    /// </summary>
    /// <param name="enrollments">The enrollments to load.</param>
    public void LoadEnrollments(IEnumerable<Enrollment> enrollments)
    {
        _enrollments.Clear();
        _enrollments.AddRange(enrollments);
    }
}
