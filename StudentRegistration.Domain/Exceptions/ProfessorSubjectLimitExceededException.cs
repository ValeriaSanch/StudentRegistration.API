// <copyright file="ProfessorSubjectLimitExceededException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Domain.Exceptions;

/// <summary>
/// Thrown when a professor already has the maximum of 2 subjects assigned.
/// </summary>
public sealed class ProfessorSubjectLimitExceededException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfessorSubjectLimitExceededException"/> class.
    /// </summary>
    public ProfessorSubjectLimitExceededException()
        : base("El profesor ya tiene 2 materias asignadas.")
    {
    }
}
