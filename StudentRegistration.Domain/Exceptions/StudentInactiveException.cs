// <copyright file="StudentInactiveException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Domain.Exceptions;

/// <summary>
/// Thrown when an inactive student attempts to enroll in a subject.
/// </summary>
public sealed class StudentInactiveException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StudentInactiveException"/> class.
    /// </summary>
    public StudentInactiveException()
        : base("El estudiante está inactivo y no puede inscribirse.")
    {
    }
}
