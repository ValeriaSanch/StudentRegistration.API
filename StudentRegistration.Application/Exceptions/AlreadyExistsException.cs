// <copyright file="AlreadyExistsException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Exceptions;

/// <summary>
/// Base application exception for resources that already exist (conflict).
/// </summary>
public abstract class AlreadyExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyExistsException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    protected AlreadyExistsException(string message)
        : base(message)
    {
    }
}
