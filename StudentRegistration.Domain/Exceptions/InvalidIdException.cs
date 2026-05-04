// <copyright file="InvalidIdException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Domain.Exceptions;

/// <summary>
/// Thrown when an identifier value is an empty GUID.
/// </summary>
public sealed class InvalidIdException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidIdException"/> class.
    /// </summary>
    public InvalidIdException()
        : base("El identificador no puede ser vacío.")
    {
    }
}
