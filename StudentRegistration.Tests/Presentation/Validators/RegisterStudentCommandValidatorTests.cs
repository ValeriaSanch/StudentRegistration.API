// <copyright file="RegisterStudentCommandValidatorTests.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using FluentAssertions;
using FluentValidation.TestHelper;
using StudentRegistration.Application.Commands;
using StudentRegistration.Presentation.Validators;
using Xunit;

namespace StudentRegistration.Tests.Presentation.Validators;

/// <summary>
/// Unit tests for <see cref="RegisterStudentCommandValidator"/>.
/// </summary>
public sealed class RegisterStudentCommandValidatorTests
{
    private readonly RegisterStudentCommandValidator _validator = new();

    // ── Valid command ─────────────────────────────────────────────────────────

    [Fact]
    public void Validate_ValidCommand_PassesAllRules()
    {
        RegisterStudentCommand command = new RegisterStudentCommand
        {
            FullName = "Juan García",
            Email = "juan@example.com",
            DocumentNumber = "123456",
        };

        TestValidationResult<RegisterStudentCommand> result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    // ── FullName validations ──────────────────────────────────────────────────

    [Fact]
    public void Validate_EmptyFullName_HasValidationError()
    {
        RegisterStudentCommand command = new RegisterStudentCommand { FullName = string.Empty, Email = "x@x.com", DocumentNumber = "12345" };
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.FullName);
    }

    [Theory]
    [InlineData("ab")]         // too short
    [InlineData("")]           // empty
    public void Validate_ShortFullName_HasValidationError(string name)
    {
        RegisterStudentCommand command = new RegisterStudentCommand { FullName = name, Email = "x@x.com", DocumentNumber = "12345" };
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.FullName);
    }

    // ── Email validations ─────────────────────────────────────────────────────

    [Fact]
    public void Validate_InvalidEmail_HasValidationError()
    {
        RegisterStudentCommand command = new RegisterStudentCommand { FullName = "Valid Name", Email = "notanemail", DocumentNumber = "12345" };
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Validate_EmptyEmail_HasValidationError()
    {
        RegisterStudentCommand command = new RegisterStudentCommand { FullName = "Valid Name", Email = string.Empty, DocumentNumber = "12345" };
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.Email);
    }

    // ── DocumentNumber validations ────────────────────────────────────────────

    [Fact]
    public void Validate_TooShortDocumentNumber_HasValidationError()
    {
        RegisterStudentCommand command = new RegisterStudentCommand { FullName = "Valid Name", Email = "x@x.com", DocumentNumber = "123" };
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.DocumentNumber);
    }

    [Fact]
    public void Validate_DocumentWithSpecialChars_HasValidationError()
    {
        RegisterStudentCommand command = new RegisterStudentCommand { FullName = "Valid Name", Email = "x@x.com", DocumentNumber = "123@#!" };
        _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.DocumentNumber);
    }

    [Theory]
    [InlineData("ABC123")]
    [InlineData("123-456-7890")]
    public void Validate_ValidDocumentNumber_PassesValidation(string doc)
    {
        RegisterStudentCommand command = new RegisterStudentCommand { FullName = "Valid Name", Email = "x@x.com", DocumentNumber = doc };
        _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(c => c.DocumentNumber);
    }
}
