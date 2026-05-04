// <copyright file="GetStudentEnrollmentsQuery.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

namespace StudentRegistration.Application.Queries;

/// <summary>
/// Query to retrieve active enrollments for a student (CU-08).
/// </summary>
public sealed class GetStudentEnrollmentsQuery
{
    /// <summary>Gets or sets the student identifier.</summary>
    public Guid StudentId { get; set; }
}
