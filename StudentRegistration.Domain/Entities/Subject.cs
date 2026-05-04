// <copyright file="Subject.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using StudentRegistration.Domain.Exceptions;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Domain.Entities;

/// <summary>
/// Represents an academic subject that students can enroll in.
/// Credits are a domain constant fixed at 3 (BR-05).
/// </summary>
public sealed class Subject
{
    /// <summary>
    /// The domain constant for subject credits. Always 3 per BR-05.
    /// </summary>
    public const int FixedCredits = 3;

    private readonly List<Enrollment> _enrollments = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Subject"/> class.
    /// Required by EF Core.
    /// </summary>
    private Subject()
    {
    }

    /// <summary>
    /// Gets the unique identifier of the subject.
    /// </summary>
    public SubjectId SubjectId { get; private set; }

    /// <summary>
    /// Gets the name of the subject.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the credit value. Always 3 per domain invariant BR-05.
    /// </summary>
    public int Credits { get; private set; }

    /// <summary>
    /// Gets the identifier of the professor teaching this subject.
    /// </summary>
    public ProfessorId ProfessorId { get; private set; }

    /// <summary>
    /// Gets the professor who teaches this subject.
    /// </summary>
    public Professor? Professor { get; private set; }

    /// <summary>
    /// Gets the enrollments for this subject.
    /// </summary>
    public IReadOnlyList<Enrollment> Enrollments => _enrollments.AsReadOnly();

    /// <summary>
    /// Creates a new <see cref="Subject"/> instance.
    /// </summary>
    /// <param name="id">The subject identifier.</param>
    /// <param name="name">The subject name.</param>
    /// <param name="credits">The credit value (must be 3).</param>
    /// <param name="professorId">The professor identifier.</param>
    /// <returns>A new <see cref="Subject"/> instance.</returns>
    /// <exception cref="InvalidCreditsException">Thrown when credits is not 3.</exception>
    public static Subject Create(SubjectId id, string name, int credits, ProfessorId professorId)
    {
        if (credits != FixedCredits)
            throw new InvalidCreditsException();

        return new Subject
        {
            SubjectId = id,
            Name = name,
            Credits = credits,
            ProfessorId = professorId,
        };
    }
}
