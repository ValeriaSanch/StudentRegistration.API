// <copyright file="SameProfessorConflictException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Domain.Exceptions;

/// <summary>
/// Thrown when a student attempts to enroll in two subjects taught by the same professor.
/// </summary>
public sealed class SameProfessorConflictException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SameProfessorConflictException"/> class.
    /// </summary>
    public SameProfessorConflictException()
        : base("El estudiante ya tiene una materia con este profesor.")
    {
    }
}
