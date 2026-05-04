// <copyright file="EnrollmentsController.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.Commands;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Queries;

namespace StudentRegistration.Presentation.Controllers;

/// <summary>
/// Manages enrollment and cancellation of subjects for a student.
/// </summary>
[ApiController]
[Route("api/v1/students/{studentId:guid}/enrollments")]
[Produces("application/json")]
public sealed class EnrollmentsController : ControllerBase
{
    private readonly IUseCase<EnrollStudentCommand, EnrollmentDto> _enroll;
    private readonly IUseCase<GetStudentEnrollmentsQuery, IReadOnlyList<EnrollmentDto>> _getEnrollments;
    private readonly IUseCase<CancelEnrollmentCommand> _cancel;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnrollmentsController"/> class.
    /// </summary>
    /// <param name="enroll">Use case for enrolling a student.</param>
    /// <param name="getEnrollments">Use case for listing enrollments.</param>
    /// <param name="cancel">Use case for cancelling an enrollment.</param>
    public EnrollmentsController(
        IUseCase<EnrollStudentCommand, EnrollmentDto> enroll,
        IUseCase<GetStudentEnrollmentsQuery, IReadOnlyList<EnrollmentDto>> getEnrollments,
        IUseCase<CancelEnrollmentCommand> cancel)
    {
        _enroll = enroll;
        _getEnrollments = getEnrollments;
        _cancel = cancel;
    }

    /// <summary>
    /// Enrolls a student in a subject.
    /// </summary>
    /// <param name="studentId">The student identifier.</param>
    /// <param name="command">The enrollment data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created enrollment.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(EnrollmentDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> Enroll(Guid studentId, [FromBody] EnrollStudentCommand command, CancellationToken ct)
    {
        command.StudentId = studentId;
        EnrollmentDto result = await _enroll.ExecuteAsync(command, ct);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Retrieves all active enrollments for a student.
    /// </summary>
    /// <param name="studentId">The student identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of active enrollments.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EnrollmentDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetEnrollments(Guid studentId, CancellationToken ct)
    {
        IReadOnlyList<EnrollmentDto> result = await _getEnrollments.ExecuteAsync(new GetStudentEnrollmentsQuery { StudentId = studentId }, ct);
        return Ok(result);
    }

    /// <summary>
    /// Cancels an active enrollment for a student.
    /// </summary>
    /// <param name="studentId">The student identifier.</param>
    /// <param name="enrollmentId">The enrollment identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{enrollmentId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Cancel(Guid studentId, Guid enrollmentId, CancellationToken ct)
    {
        await _cancel.ExecuteAsync(new CancelEnrollmentCommand { StudentId = studentId, EnrollmentId = enrollmentId }, ct);
        return NoContent();
    }
}
