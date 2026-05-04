// <copyright file="GetClassmatesQuery.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Queries;

/// <summary>
/// Query to retrieve classmates for a student in a given subject (CU-09).
/// </summary>
public sealed class GetClassmatesQuery
{
    /// <summary>Gets or sets the requesting student identifier.</summary>
    public Guid StudentId { get; set; }

    /// <summary>Gets or sets the subject identifier.</summary>
    public Guid SubjectId { get; set; }
}
