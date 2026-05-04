// <copyright file="StudentId.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using StudentRegistration.Domain.Exceptions;

namespace StudentRegistration.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for a Student entity.
/// </summary>
public readonly record struct StudentId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StudentId"/> struct.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    /// <exception cref="InvalidIdException">Thrown when value is <see cref="Guid.Empty"/>.</exception>
    public StudentId(Guid value)
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
    /// Creates a new <see cref="StudentId"/> with a freshly generated GUID.
    /// </summary>
    /// <returns>A new unique <see cref="StudentId"/>.</returns>
    public static StudentId New() => new(Guid.NewGuid());

    /// <summary>
    /// Implicitly converts a <see cref="StudentId"/> to a <see cref="Guid"/>.
    /// </summary>
    /// <param name="id">The student identifier.</param>
    public static implicit operator Guid(StudentId id) => id.Value;
}
