// <copyright file="StudentsController.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.Commands;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Queries;

namespace StudentRegistration.Presentation.Controllers;

/// <summary>
/// Manages student registration, retrieval, update, and logical deletion.
/// </summary>
[ApiController]
[Route("api/v1/students")]
[Produces("application/json")]
public sealed class StudentsController : ControllerBase
{
    private readonly IUseCase<RegisterStudentCommand, StudentDto> _register;
    private readonly IUseCase<GetAllStudentsQuery, IReadOnlyList<StudentSummaryDto>> _getAll;
    private readonly IUseCase<GetStudentByIdQuery, StudentDto> _getById;
    private readonly IUseCase<UpdateStudentCommand, StudentDto> _update;
    private readonly IUseCase<DeleteStudentCommand> _delete;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudentsController"/> class.
    /// </summary>
    /// <param name="register">Use case for registering a student.</param>
    /// <param name="getAll">Use case for listing all students.</param>
    /// <param name="getById">Use case for retrieving a student by ID.</param>
    /// <param name="update">Use case for updating a student.</param>
    /// <param name="delete">Use case for deleting a student.</param>
    public StudentsController(
        IUseCase<RegisterStudentCommand, StudentDto> register,
        IUseCase<GetAllStudentsQuery, IReadOnlyList<StudentSummaryDto>> getAll,
        IUseCase<GetStudentByIdQuery, StudentDto> getById,
        IUseCase<UpdateStudentCommand, StudentDto> update,
        IUseCase<DeleteStudentCommand> delete)
    {
        _register = register;
        _getAll = getAll;
        _getById = getById;
        _update = update;
        _delete = delete;
    }

    /// <summary>
    /// Registers a new student.
    /// </summary>
    /// <param name="command">The registration data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The newly created student.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(StudentDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Register([FromBody] RegisterStudentCommand command, CancellationToken ct)
    {
        StudentDto result = await _register.ExecuteAsync(command, ct);
        return CreatedAtAction(nameof(GetById), new { studentId = result.StudentId }, result);
    }

    /// <summary>
    /// Retrieves all active students with non-sensitive fields.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of student summaries.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StudentSummaryDto>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        IReadOnlyList<StudentSummaryDto> result = await _getAll.ExecuteAsync(new GetAllStudentsQuery(), ct);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a student by their unique identifier.
    /// </summary>
    /// <param name="studentId">The student identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The student details.</returns>
    [HttpGet("{studentId:guid}")]
    [ProducesResponseType(typeof(StudentDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid studentId, CancellationToken ct)
    {
        StudentDto result = await _getById.ExecuteAsync(new GetStudentByIdQuery { StudentId = studentId }, ct);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing student's name and document number.
    /// </summary>
    /// <param name="studentId">The student identifier.</param>
    /// <param name="command">The update data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated student details.</returns>
    [HttpPut("{studentId:guid}")]
    [ProducesResponseType(typeof(StudentDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> Update(Guid studentId, [FromBody] UpdateStudentCommand command, CancellationToken ct)
    {
        command.StudentId = studentId;
        StudentDto result = await _update.ExecuteAsync(command, ct);
        return Ok(result);
    }

    /// <summary>
    /// Logically deletes a student and cancels their active enrollments.
    /// </summary>
    /// <param name="studentId">The student identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{studentId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid studentId, CancellationToken ct)
    {
        await _delete.ExecuteAsync(new DeleteStudentCommand { StudentId = studentId }, ct);
        return NoContent();
    }
}
