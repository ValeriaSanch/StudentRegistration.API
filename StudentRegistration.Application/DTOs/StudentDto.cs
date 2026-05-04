// <copyright file="StudentDto.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.DTOs;

/// <summary>
/// Full data transfer object for a student, including computed credits and enrollments.
/// </summary>
public sealed class StudentDto
{
    /// <summary>Gets or sets the student identifier.</summary>
    public Guid StudentId { get; set; }

    /// <summary>Gets or sets the student's full name.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Gets or sets the student's email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the student's document number.</summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>Gets or sets the UTC creation timestamp.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the UTC last-update timestamp.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Gets or sets a value indicating whether the student is active.</summary>
    public bool IsActive { get; set; }

    /// <summary>Gets or sets the total credits from active enrollments (active enrollments × 3).</summary>
    public int TotalCredits { get; set; }

    /// <summary>Gets or sets the list of enrollments for this student.</summary>
    public List<EnrollmentDto> Enrollments { get; set; } = new();
}
