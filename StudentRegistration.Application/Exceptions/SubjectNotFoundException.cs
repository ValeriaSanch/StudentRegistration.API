// <copyright file="SubjectNotFoundException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Exceptions;

/// <summary>
/// Thrown when a subject is not found in the system.
/// </summary>
public sealed class SubjectNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectNotFoundException"/> class.
    /// </summary>
    /// <param name="id">The identifier that was not found.</param>
    public SubjectNotFoundException(Guid id)
        : base($"Materia con id '{id}' no encontrada.")
    {
    }
}
