// <copyright file="ProfessorId.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using StudentRegistration.Domain.Exceptions;

namespace StudentRegistration.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for a Professor entity.
/// </summary>
public readonly record struct ProfessorId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfessorId"/> struct.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    /// <exception cref="InvalidIdException">Thrown when value is <see cref="Guid.Empty"/>.</exception>
    public ProfessorId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidIdException();
        Value = value;
    }

    /// <summary>
    /// Gets the underlying GUID value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Creates a new <see cref="ProfessorId"/> with a freshly generated GUID.
    /// </summary>
    /// <returns>A new unique <see cref="ProfessorId"/>.</returns>
    public static ProfessorId New() => new(Guid.NewGuid());

    /// <summary>
    /// Implicitly converts a <see cref="ProfessorId"/> to a <see cref="Guid"/>.
    /// </summary>
    /// <param name="id">The professor identifier.</param>
    public static implicit operator Guid(ProfessorId id) => id.Value;
}
