// <copyright file="SubjectId.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using StudentRegistration.Domain.Exceptions;

namespace StudentRegistration.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for a Subject entity.
/// </summary>
public readonly record struct SubjectId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectId"/> struct.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    /// <exception cref="InvalidIdException">Thrown when value is <see cref="Guid.Empty"/>.</exception>
    public SubjectId(Guid value)
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
    /// Creates a new <see cref="SubjectId"/> with a freshly generated GUID.
    /// </summary>
    /// <returns>A new unique <see cref="SubjectId"/>.</returns>
    public static SubjectId New() => new(Guid.NewGuid());

    /// <summary>
    /// Implicitly converts a <see cref="SubjectId"/> to a <see cref="Guid"/>.
    /// </summary>
    /// <param name="id">The subject identifier.</param>
    public static implicit operator Guid(SubjectId id) => id.Value;
}
