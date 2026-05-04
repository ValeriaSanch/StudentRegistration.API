// <copyright file="EnrollStudentCommand.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Commands;

/// <summary>
/// Command to enroll a student in a subject (CU-06).
/// </summary>
public sealed class EnrollStudentCommand
{
    /// <summary>Gets or sets the student identifier.</summary>
    public Guid StudentId { get; set; }

    /// <summary>Gets or sets the subject identifier.</summary>
    public Guid SubjectId { get; set; }
}
