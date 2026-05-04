// <copyright file="ClassmatesController.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Queries;

namespace StudentRegistration.Presentation.Controllers;

/// <summary>
/// Exposes classmate information for students enrolled in the same subject.
/// Only full name is exposed per BR-11.
/// </summary>
[ApiController]
[Route("api/v1/students/{studentId:guid}/subjects/{subjectId:guid}/classmates")]
[Produces("application/json")]
public sealed class ClassmatesController : ControllerBase
{
    private readonly IUseCase<GetClassmatesQuery, IReadOnlyList<ClassmateDto>> _getClassmates;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClassmatesController"/> class.
    /// </summary>
    /// <param name="getClassmates">Use case for retrieving classmates.</param>
    public ClassmatesController(IUseCase<GetClassmatesQuery, IReadOnlyList<ClassmateDto>> getClassmates)
    {
        _getClassmates = getClassmates;
    }

    /// <summary>
    /// Retrieves all classmates of a student in a specific subject.
    /// </summary>
    /// <param name="studentId">The requesting student identifier.</param>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of classmates with only their full name.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ClassmateDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetClassmates(Guid studentId, Guid subjectId, CancellationToken ct)
    {
        IReadOnlyList<ClassmateDto> result = await _getClassmates.ExecuteAsync(
            new GetClassmatesQuery { StudentId = studentId, SubjectId = subjectId },
            ct);
        return Ok(result);
    }
}
