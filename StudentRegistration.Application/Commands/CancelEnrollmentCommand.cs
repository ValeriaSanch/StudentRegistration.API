// <copyright file="CancelEnrollmentCommand.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Commands;

/// <summary>
/// Command to cancel an active enrollment for a student (CU-07).
/// </summary>
public sealed class CancelEnrollmentCommand
{
    /// <summary>Gets or sets the student identifier.</summary>
    public Guid StudentId { get; set; }

    /// <summary>Gets or sets the enrollment identifier to cancel.</summary>
    public Guid EnrollmentId { get; set; }
}
