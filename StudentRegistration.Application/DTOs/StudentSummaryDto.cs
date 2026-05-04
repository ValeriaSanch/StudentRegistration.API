// <copyright file="StudentSummaryDto.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.DTOs;

/// <summary>
/// Lightweight data transfer object exposing non-sensitive student fields (BR-12).
/// </summary>
public sealed class StudentSummaryDto
{
    /// <summary>Gets or sets the student identifier.</summary>
    public Guid StudentId { get; set; }

    /// <summary>Gets or sets the student's full name.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Gets or sets the student's email address.</summary>
    public string Email { get; set; } = string.Empty;
}
