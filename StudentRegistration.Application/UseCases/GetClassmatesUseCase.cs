// <copyright file="GetClassmatesUseCase.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Exceptions;
using StudentRegistration.Application.Queries;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Interfaces;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Application.UseCases;

/// <summary>
/// Retrieves classmates (other students enrolled in the same subject) for a student (CU-09).
/// Only exposes full name for privacy per BR-11.
/// </summary>
public sealed class GetClassmatesUseCase : IUseCase<GetClassmatesQuery, IReadOnlyList<ClassmateDto>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetClassmatesUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetClassmatesUseCase"/> class.
    /// </summary>
    /// <param name="studentRepository">The student repository.</param>
    /// <param name="enrollmentRepository">The enrollment repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger.</param>
    public GetClassmatesUseCase(
        IStudentRepository studentRepository,
        IEnrollmentRepository enrollmentRepository,
        IMapper mapper,
        ILogger<GetClassmatesUseCase> logger)
    {
        _studentRepository = studentRepository;
        _enrollmentRepository = enrollmentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Returns classmates for the specified student in the given subject.
    /// </summary>
    /// <param name="request">The classmates query.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of classmate DTOs.</returns>
    /// <exception cref="StudentNotFoundException">Thrown when the student does not exist.</exception>
    /// <exception cref="EnrollmentNotFoundException">Thrown when the student is not enrolled in the subject.</exception>
    public async Task<IReadOnlyList<ClassmateDto>> ExecuteAsync(GetClassmatesQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Getting classmates for student {StudentId} in subject {SubjectId}", request.StudentId, request.SubjectId);

        Student? student = await _studentRepository.GetByIdAsync(new StudentId(request.StudentId), ct);
        if (student is null)
        {
            _logger.LogWarning("Student {StudentId} not found", request.StudentId);
            throw new StudentNotFoundException(request.StudentId);
        }

        SubjectId subjectId = new SubjectId(request.SubjectId);
        bool isEnrolled = student.Enrollments.Any(e => e.IsActive && e.SubjectId == subjectId);
        if (!isEnrolled)
        {
            _logger.LogWarning("Student {StudentId} has no active enrollment in subject {SubjectId}", request.StudentId, request.SubjectId);
            throw new EnrollmentNotFoundException(request.SubjectId);
        }

        IReadOnlyList<Student> classmates = await _enrollmentRepository.GetClassmatesAsync(
            subjectId,
            new StudentId(request.StudentId),
            ct);

        _logger.LogInformation("Found {Count} classmates for student {StudentId}", classmates.Count, request.StudentId);
        return _mapper.Map<IReadOnlyList<ClassmateDto>>(classmates);
    }
}
