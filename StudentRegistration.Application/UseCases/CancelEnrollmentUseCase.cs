// <copyright file="CancelEnrollmentUseCase.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using System.Linq;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.Commands;
using StudentRegistration.Application.Exceptions;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Interfaces;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Application.UseCases;

/// <summary>
/// Cancels a student's active enrollment (CU-07).
/// </summary>
public sealed class CancelEnrollmentUseCase : IUseCase<CancelEnrollmentCommand>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CancelEnrollmentUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelEnrollmentUseCase"/> class.
    /// </summary>
    /// <param name="studentRepository">The student repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public CancelEnrollmentUseCase(
        IStudentRepository studentRepository,
        IUnitOfWork unitOfWork,
        ILogger<CancelEnrollmentUseCase> logger)
    {
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Cancels a specific enrollment for a student.
    /// </summary>
    /// <param name="request">The cancel enrollment command.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="StudentNotFoundException">Thrown when the student does not exist.</exception>
    /// <exception cref="EnrollmentNotFoundException">Thrown when the enrollment does not belong to the student.</exception>
    public async Task ExecuteAsync(CancelEnrollmentCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Cancelling enrollment {EnrollmentId} for student {StudentId}", request.EnrollmentId, request.StudentId);

        Student? student = await _studentRepository.GetByIdAsync(new StudentId(request.StudentId), ct);
        if (student is null)
        {
            _logger.LogWarning("Student {StudentId} not found", request.StudentId);
            throw new StudentNotFoundException(request.StudentId);
        }

        Enrollment? enrollment = student.Enrollments.FirstOrDefault(e => e.EnrollmentId.Value == request.EnrollmentId);
        if (enrollment is null)
        {
            _logger.LogWarning("Enrollment {EnrollmentId} not found for student {StudentId}", request.EnrollmentId, request.StudentId);
            throw new EnrollmentNotFoundException(request.EnrollmentId);
        }

        enrollment.Cancel();
        await _studentRepository.UpdateAsync(student, ct);
        await _unitOfWork.CommitAsync(ct);

        _logger.LogInformation("Enrollment {EnrollmentId} cancelled", request.EnrollmentId);
    }
}
