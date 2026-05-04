// <copyright file="GetAllStudentsUseCase.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Queries;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Interfaces;

namespace StudentRegistration.Application.UseCases;

/// <summary>
/// Retrieves all active students with non-sensitive fields only (CU-03).
/// </summary>
public sealed class GetAllStudentsUseCase : IUseCase<GetAllStudentsQuery, IReadOnlyList<StudentSummaryDto>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllStudentsUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllStudentsUseCase"/> class.
    /// </summary>
    /// <param name="studentRepository">The student repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger.</param>
    public GetAllStudentsUseCase(IStudentRepository studentRepository, IMapper mapper, ILogger<GetAllStudentsUseCase> logger)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Returns a summary list of all active students.
    /// </summary>
    /// <param name="request">Empty query object.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of student summaries.</returns>
    public async Task<IReadOnlyList<StudentSummaryDto>> ExecuteAsync(GetAllStudentsQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Getting all active students");

        IReadOnlyList<Student> students = await _studentRepository.GetAllAsync(ct);
        List<Student> active = students.Where(s => s.IsActive).ToList();

        _logger.LogInformation("Found {Count} active students", active.Count);
        return _mapper.Map<IReadOnlyList<StudentSummaryDto>>(active);
    }
}
