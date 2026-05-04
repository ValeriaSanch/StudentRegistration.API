// <copyright file="EnrollmentId.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using StudentRegistration.Domain.Exceptions;

namespace StudentRegistration.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for an Enrollment entity.
/// </summary>
public readonly record struct EnrollmentId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnrollmentId"/> struct.
    /// </summary>
    /// <param name="value">The GUID value.</param>
    /// <exception cref="InvalidIdException">Thrown when value is <see cref="Guid.Empty"/>.</exception>
    public EnrollmentId(Guid value)
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
    /// Creates a new <see cref="EnrollmentId"/> with a freshly generated GUID.
    /// </summary>
    /// <returns>A new unique <see cref="EnrollmentId"/>.</returns>
    public static EnrollmentId New() => new(Guid.NewGuid());

    /// <summary>
    /// Implicitly converts an <see cref="EnrollmentId"/> to a <see cref="Guid"/>.
    /// </summary>
    /// <param name="id">The enrollment identifier.</param>
    public static implicit operator Guid(EnrollmentId id) => id.Value;
}
