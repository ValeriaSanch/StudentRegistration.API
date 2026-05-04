// <copyright file="DomainException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Domain.Exceptions;

/// <summary>
/// Base class for all domain exceptions in the student registration system.
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    protected DomainException(string message)
        : base(message)
    {
    }
}
