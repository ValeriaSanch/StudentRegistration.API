// <copyright file="DeleteStudentUseCase.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.Commands;
using StudentRegistration.Application.Exceptions;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Interfaces;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Application.UseCases;

/// <summary>
/// Logically deletes a student and cancels all their active enrollments (CU-05).
/// </summary>
public sealed class DeleteStudentUseCase : IUseCase<DeleteStudentCommand>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteStudentUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteStudentUseCase"/> class.
    /// </summary>
    /// <param name="studentRepository">The student repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public DeleteStudentUseCase(
        IStudentRepository studentRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteStudentUseCase> logger)
    {
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Deactivates a student and cancels all their active enrollments.
    /// </summary>
    /// <param name="request">The delete command.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="StudentNotFoundException">Thrown when the student does not exist.</exception>
    public async Task ExecuteAsync(DeleteStudentCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Deleting student {StudentId}", request.StudentId);

        Student? student = await _studentRepository.GetByIdAsync(new StudentId(request.StudentId), ct);
        if (student is null)
        {
            _logger.LogWarning("Student {StudentId} not found for deletion", request.StudentId);
            throw new StudentNotFoundException(request.StudentId);
        }

        student.Deactivate();
        await _studentRepository.UpdateAsync(student, ct);
        await _unitOfWork.CommitAsync(ct);

        _logger.LogInformation("Student {StudentId} deactivated", request.StudentId);
    }
}
