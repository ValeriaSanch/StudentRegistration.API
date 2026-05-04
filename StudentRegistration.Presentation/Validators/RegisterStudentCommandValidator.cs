// <copyright file="RegisterStudentCommandValidator.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using FluentValidation;
using StudentRegistration.Application.Commands;

namespace StudentRegistration.Presentation.Validators;

/// <summary>
/// FluentValidation validator for <see cref="RegisterStudentCommand"/>.
/// </summary>
public sealed class RegisterStudentCommandValidator : AbstractValidator<RegisterStudentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterStudentCommandValidator"/> class with all rules.
    /// </summary>
    public RegisterStudentCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("El nombre completo es requerido.")
            .Length(3, 150).WithMessage("El nombre completo debe tener entre 3 y 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido.")
            .EmailAddress().WithMessage("El formato del correo electrónico es inválido.");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("El número de documento es requerido.")
            .Length(5, 20).WithMessage("El número de documento debe tener entre 5 y 20 caracteres.")
            .Matches(@"^[0-9A-Za-z-]+$").WithMessage("El número de documento solo puede contener letras, números y guiones.");
    }
}
