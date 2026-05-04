// <copyright file="EnrollmentNotFoundException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Exceptions;

/// <summary>
/// Thrown when an enrollment is not found for a student.
/// </summary>
public sealed class EnrollmentNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnrollmentNotFoundException"/> class.
    /// </summary>
    /// <param name="enrollmentId">The enrollment identifier that was not found.</param>
    public EnrollmentNotFoundException(Guid enrollmentId)
        : base($"Matrícula con id '{enrollmentId}' no encontrada.")
    {
    }
}
