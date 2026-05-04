// <copyright file="EmailAlreadyExistsException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Exceptions;

/// <summary>
/// Thrown when a student registration is attempted with an email already in use.
/// </summary>
public sealed class EmailAlreadyExistsException : AlreadyExistsException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailAlreadyExistsException"/> class.
    /// </summary>
    /// <param name="email">The duplicate email address.</param>
    public EmailAlreadyExistsException(string email)
        : base($"Ya existe un estudiante con el correo '{email}'.")
    {
    }
}
