// <copyright file="RegisterStudentCommand.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Commands;

/// <summary>
/// Command to register a new student in the system (CU-01).
/// </summary>
public sealed class RegisterStudentCommand
{
    /// <summary>Gets or sets the student's full name.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Gets or sets the student's email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the student's document number.</summary>
    public string DocumentNumber { get; set; } = string.Empty;
}
