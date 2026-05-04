// <copyright file="DuplicateEnrollmentException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Domain.Exceptions;

/// <summary>
/// Thrown when a student attempts to enroll in a subject they are already enrolled in.
/// </summary>
public sealed class DuplicateEnrollmentException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEnrollmentException"/> class.
    /// </summary>
    public DuplicateEnrollmentException()
        : base("El estudiante ya está inscrito en esta materia.")
    {
    }
}
