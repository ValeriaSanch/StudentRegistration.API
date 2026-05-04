// <copyright file="SubjectsController.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Queries;

namespace StudentRegistration.Presentation.Controllers;

/// <summary>
/// Exposes the read-only catalog of academic subjects.
/// </summary>
[ApiController]
[Route("api/v1/subjects")]
[Produces("application/json")]
public sealed class SubjectsController : ControllerBase
{
    private readonly IUseCase<GetAllSubjectsQuery, IReadOnlyList<SubjectDto>> _getAllSubjects;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectsController"/> class.
    /// </summary>
    /// <param name="getAllSubjects">Use case for listing all subjects.</param>
    public SubjectsController(IUseCase<GetAllSubjectsQuery, IReadOnlyList<SubjectDto>> getAllSubjects)
    {
        _getAllSubjects = getAllSubjects;
    }

    /// <summary>
    /// Retrieves all subjects in the system.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of subjects with professor information.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SubjectDto>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        IReadOnlyList<SubjectDto> result = await _getAllSubjects.ExecuteAsync(new GetAllSubjectsQuery(), ct);
        return Ok(result);
    }
}
