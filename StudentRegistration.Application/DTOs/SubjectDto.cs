// <copyright file="SubjectDto.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.DTOs;

/// <summary>
/// Data transfer object representing an academic subject.
/// </summary>
public sealed class SubjectDto
{
    /// <summary>Gets or sets the subject identifier.</summary>
    public Guid SubjectId { get; set; }

    /// <summary>Gets or sets the subject name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the credit value (always 3).</summary>
    public int Credits { get; set; }

    /// <summary>Gets or sets the professor identifier.</summary>
    public Guid ProfessorId { get; set; }

    /// <summary>Gets or sets the professor full name.</summary>
    public string ProfessorName { get; set; } = string.Empty;
}
