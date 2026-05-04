// <copyright file="StudentNotFoundException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Exceptions;

/// <summary>
/// Thrown when a student is not found in the system.
/// </summary>
public sealed class StudentNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StudentNotFoundException"/> class.
    /// </summary>
    /// <param name="id">The identifier that was not found.</param>
    public StudentNotFoundException(Guid id)
        : base($"Estudiante con id '{id}' no encontrado.")
    {
    }
}
