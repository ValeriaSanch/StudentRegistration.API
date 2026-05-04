// <copyright file="UpdateStudentCommand.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Commands;

/// <summary>
/// Command to update an existing student's information (CU-04).
/// </summary>
public sealed class UpdateStudentCommand
{
    /// <summary>Gets or sets the student identifier.</summary>
    public Guid StudentId { get; set; }

    /// <summary>Gets or sets the new full name.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Gets or sets the new document number.</summary>
    public string DocumentNumber { get; set; } = string.Empty;
}
