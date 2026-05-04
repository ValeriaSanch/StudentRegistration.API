// <copyright file="EnrollStudentCommandValidator.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using FluentValidation;
using StudentRegistration.Application.Commands;

namespace StudentRegistration.Presentation.Validators;

/// <summary>
/// FluentValidation validator for <see cref="EnrollStudentCommand"/>.
/// </summary>
public sealed class EnrollStudentCommandValidator : AbstractValidator<EnrollStudentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnrollStudentCommandValidator"/> class with all rules.
    /// </summary>
    public EnrollStudentCommandValidator()
    {
        RuleFor(x => x.SubjectId)
            .NotEmpty().WithMessage("El identificador de la materia es requerido.");
    }
}
