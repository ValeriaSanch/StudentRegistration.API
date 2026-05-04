// <copyright file="DeleteStudentCommand.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Commands;

/// <summary>
/// Command to logically delete a student and cancel their enrollments (CU-05).
/// </summary>
public sealed class DeleteStudentCommand
{
    /// <summary>Gets or sets the student identifier to delete.</summary>
    public Guid StudentId { get; set; }
}
