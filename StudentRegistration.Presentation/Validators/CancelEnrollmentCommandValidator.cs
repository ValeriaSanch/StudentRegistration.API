// <copyright file="CancelEnrollmentCommandValidator.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using FluentValidation;
using StudentRegistration.Application.Commands;

namespace StudentRegistration.Presentation.Validators;

/// <summary>
/// FluentValidation validator for <see cref="CancelEnrollmentCommand"/>.
/// </summary>
public sealed class CancelEnrollmentCommandValidator : AbstractValidator<CancelEnrollmentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelEnrollmentCommandValidator"/> class with all rules.
    /// </summary>
    public CancelEnrollmentCommandValidator()
    {
        RuleFor(x => x.EnrollmentId)
            .NotEmpty().WithMessage("El identificador de la matrícula es requerido.");
    }
}
