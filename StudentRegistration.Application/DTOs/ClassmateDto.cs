// <copyright file="ClassmateDto.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.DTOs;

/// <summary>
/// Data transfer object for a classmate. Only exposes full name for privacy (BR-11).
/// </summary>
public sealed class ClassmateDto
{
    /// <summary>Gets or sets the classmate's student identifier.</summary>
    public Guid StudentId { get; set; }

    /// <summary>Gets or sets the classmate's full name.</summary>
    public string FullName { get; set; } = string.Empty;
}
