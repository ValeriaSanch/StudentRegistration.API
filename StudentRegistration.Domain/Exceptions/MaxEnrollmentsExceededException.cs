// <copyright file="MaxEnrollmentsExceededException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Domain.Exceptions;

/// <summary>
/// Thrown when a student attempts to enroll in more than 3 active subjects simultaneously.
/// </summary>
public sealed class MaxEnrollmentsExceededException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MaxEnrollmentsExceededException"/> class.
    /// </summary>
    public MaxEnrollmentsExceededException()
        : base("El estudiante ya tiene 3 materias inscritas.")
    {
    }
}
