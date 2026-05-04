// <copyright file="Professor.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using StudentRegistration.Domain.Exceptions;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Domain.Entities;

/// <summary>
/// Represents a professor who teaches subjects in the registration system.
/// </summary>
public sealed class Professor
{
    private readonly List<Subject> _subjects = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Professor"/> class.
    /// Required by EF Core.
    /// </summary>
    private Professor()
    {
    }

    /// <summary>
    /// Gets the unique identifier of the professor.
    /// </summary>
    public ProfessorId ProfessorId { get; private set; }

    /// <summary>
    /// Gets the full name of the professor.
    /// </summary>
    public string FullName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the subjects assigned to this professor.
    /// </summary>
    public IReadOnlyList<Subject> Subjects => _subjects.AsReadOnly();

    /// <summary>
    /// Creates a new <see cref="Professor"/> instance.
    /// </summary>
    /// <param name="id">The professor identifier.</param>
    /// <param name="fullName">The full name of the professor.</param>
    /// <returns>A new <see cref="Professor"/> instance.</returns>
    public static Professor Create(ProfessorId id, string fullName)
    {
        return new Professor
        {
            ProfessorId = id,
            FullName = fullName,
        };
    }

    /// <summary>
    /// Assigns a subject to this professor, enforcing the 2-subject limit (BR-08).
    /// </summary>
    /// <param name="subject">The subject to assign.</param>
    /// <exception cref="ProfessorSubjectLimitExceededException">Thrown when professor already has 2 subjects.</exception>
    public void AddSubject(Subject subject)
    {
        if (_subjects.Count >= 2)
            throw new ProfessorSubjectLimitExceededException();

        _subjects.Add(subject);
    }
}
