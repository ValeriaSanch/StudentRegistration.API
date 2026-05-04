// <copyright file="InvalidEmailException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Domain.Exceptions;

/// <summary>
/// Thrown when an email address does not conform to a valid format.
/// </summary>
public sealed class InvalidEmailException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidEmailException"/> class.
    /// </summary>
    public InvalidEmailException()
        : base("El formato del correo electrónico es inválido.")
    {
    }
}
