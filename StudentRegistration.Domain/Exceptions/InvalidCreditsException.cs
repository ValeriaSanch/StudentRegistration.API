// <copyright file="InvalidCreditsException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Domain.Exceptions;

/// <summary>
/// Thrown when a subject is created with credits other than 3.
/// </summary>
public sealed class InvalidCreditsException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCreditsException"/> class.
    /// </summary>
    public InvalidCreditsException()
        : base("Los créditos de una materia deben ser exactamente 3.")
    {
    }
}
