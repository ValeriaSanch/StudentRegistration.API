// <copyright file="GetStudentByIdQuery.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Queries;

/// <summary>
/// Query to retrieve a student by their identifier (CU-02).
/// </summary>
public sealed class GetStudentByIdQuery
{
    /// <summary>Gets or sets the student identifier to retrieve.</summary>
    public Guid StudentId { get; set; }
}
