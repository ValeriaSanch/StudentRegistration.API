// <copyright file="EnrollmentDto.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.DTOs;

/// <summary>
/// Data transfer object representing an enrollment with subject and professor details.
/// </summary>
public sealed class EnrollmentDto
{
    /// <summary>Gets or sets the enrollment identifier.</summary>
    public Guid EnrollmentId { get; set; }

    /// <summary>Gets or sets the subject identifier.</summary>
    public Guid SubjectId { get; set; }

    /// <summary>Gets or sets the subject name.</summary>
    public string SubjectName { get; set; } = string.Empty;

    /// <summary>Gets or sets the subject credits.</summary>
    public int Credits { get; set; }

    /// <summary>Gets or sets the professor full name.</summary>
    public string ProfessorName { get; set; } = string.Empty;

    /// <summary>Gets or sets the UTC timestamp when the enrollment was created.</summary>
    public DateTime EnrolledAt { get; set; }

    /// <summary>Gets or sets a value indicating whether the enrollment is active.</summary>
    public bool IsActive { get; set; }
}
