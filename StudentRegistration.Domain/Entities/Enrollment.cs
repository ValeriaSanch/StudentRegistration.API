// <copyright file="Enrollment.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Domain.Entities;

/// <summary>
/// Represents a student's enrollment in a subject. Belongs to the Student aggregate.
/// </summary>
public sealed class Enrollment
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Enrollment"/> class.
    /// Required by EF Core.
    /// </summary>
    private Enrollment()
    {
    }

    /// <summary>
    /// Gets the unique identifier of the enrollment.
    /// </summary>
    public EnrollmentId EnrollmentId { get; private set; }

    /// <summary>
    /// Gets the identifier of the enrolled student.
    /// </summary>
    public StudentId StudentId { get; private set; }

    /// <summary>
    /// Gets the identifier of the enrolled subject.
    /// </summary>
    public SubjectId SubjectId { get; private set; }

    /// <summary>
    /// Gets the professor identifier, stored for BR-02 validation within the aggregate.
    /// </summary>
    public ProfessorId ProfessorId { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp when the enrollment was created.
    /// </summary>
    public DateTime EnrolledAt { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the enrollment is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the subject navigation property loaded by EF Core.
    /// </summary>
    public Subject? Subject { get; private set; }

    /// <summary>
    /// Creates a new active <see cref="Enrollment"/>.
    /// </summary>
    /// <param name="studentId">The student identifier.</param>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="professorId">The professor identifier (denormalized for BR-02).</param>
    /// <returns>A new <see cref="Enrollment"/> instance.</returns>
    public static Enrollment Create(StudentId studentId, SubjectId subjectId, ProfessorId professorId)
    {
        return new Enrollment
        {
            EnrollmentId = EnrollmentId.New(),
            StudentId = studentId,
            SubjectId = subjectId,
            ProfessorId = professorId,
            EnrolledAt = DateTime.UtcNow,
            IsActive = true,
        };
    }

    /// <summary>
    /// Cancels this enrollment by setting it inactive (BR-10).
    /// </summary>
    public void Cancel()
    {
        IsActive = false;
    }
}
