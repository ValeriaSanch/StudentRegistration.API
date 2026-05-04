// <copyright file="UpdateStudentCommandValidator.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using FluentValidation;
using StudentRegistration.Application.Commands;

namespace StudentRegistration.Presentation.Validators;

/// <summary>
/// FluentValidation validator for <see cref="UpdateStudentCommand"/>.
/// </summary>
public sealed class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStudentCommandValidator"/> class with all rules.
    /// </summary>
    public UpdateStudentCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("El identificador del estudiante es requerido.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("El nombre completo es requerido.")
            .Length(3, 150).WithMessage("El nombre completo debe tener entre 3 y 150 caracteres.");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("El número de documento es requerido.")
            .Length(5, 20).WithMessage("El número de documento debe tener entre 5 y 20 caracteres.");
    }
}
