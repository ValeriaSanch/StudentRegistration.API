// <copyright file="GetStudentEnrollmentsUseCase.cs" company="Inter Rapidísimo">
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
/// Retrieves active enrollments for a student with subject and professor details (CU-08).
/// </summary>
public sealed class GetStudentEnrollmentsUseCase : IUseCase<GetStudentEnrollmentsQuery, IReadOnlyList<EnrollmentDto>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetStudentEnrollmentsUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetStudentEnrollmentsUseCase"/> class.
    /// </summary>
    /// <param name="studentRepository">The student repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger.</param>
    public GetStudentEnrollmentsUseCase(IStudentRepository studentRepository, IMapper mapper, ILogger<GetStudentEnrollmentsUseCase> logger)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Returns all active enrollments for the specified student.
    /// </summary>
    /// <param name="request">The query containing the student identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of enrollment DTOs.</returns>
    /// <exception cref="StudentNotFoundException">Thrown when the student does not exist.</exception>
    public async Task<IReadOnlyList<EnrollmentDto>> ExecuteAsync(GetStudentEnrollmentsQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Getting enrollments for student {StudentId}", request.StudentId);

        Student? student = await _studentRepository.GetByIdAsync(new StudentId(request.StudentId), ct);
        if (student is null)
        {
            _logger.LogWarning("Student {StudentId} not found", request.StudentId);
            throw new StudentNotFoundException(request.StudentId);
        }

        List<Enrollment> active = student.Enrollments.Where(e => e.IsActive).ToList();
        _logger.LogInformation("Found {Count} active enrollments for student {StudentId}", active.Count, request.StudentId);
        return _mapper.Map<IReadOnlyList<EnrollmentDto>>(active);
    }
}
