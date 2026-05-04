// <copyright file="GetAllSubjectsUseCase.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Queries;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Interfaces;

namespace StudentRegistration.Application.UseCases;

/// <summary>
/// Retrieves all subjects with professor information (CU-10).
/// </summary>
public sealed class GetAllSubjectsUseCase : IUseCase<GetAllSubjectsQuery, IReadOnlyList<SubjectDto>>
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllSubjectsUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllSubjectsUseCase"/> class.
    /// </summary>
    /// <param name="subjectRepository">The subject repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger.</param>
    public GetAllSubjectsUseCase(ISubjectRepository subjectRepository, IMapper mapper, ILogger<GetAllSubjectsUseCase> logger)
    {
        _subjectRepository = subjectRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Returns all subjects in the system.
    /// </summary>
    /// <param name="request">Empty query object.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of subject DTOs.</returns>
    public async Task<IReadOnlyList<SubjectDto>> ExecuteAsync(GetAllSubjectsQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Getting all subjects");

        IReadOnlyList<Subject> subjects = await _subjectRepository.GetAllAsync(ct);

        _logger.LogInformation("Found {Count} subjects", subjects.Count);
        return _mapper.Map<IReadOnlyList<SubjectDto>>(subjects);
    }
}
