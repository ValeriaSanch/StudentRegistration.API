// <copyright file="DocumentAlreadyExistsException.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Exceptions;

/// <summary>
/// Thrown when a student registration is attempted with a document number already in use.
/// </summary>
public sealed class DocumentAlreadyExistsException : AlreadyExistsException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentAlreadyExistsException"/> class.
    /// </summary>
    /// <param name="documentNumber">The duplicate document number.</param>
    public DocumentAlreadyExistsException(string documentNumber)
        : base($"Ya existe un estudiante con el número de documento '{documentNumber}'.")
    {
    }
}
