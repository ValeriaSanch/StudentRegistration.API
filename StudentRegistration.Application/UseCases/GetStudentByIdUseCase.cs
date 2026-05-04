// <copyright file="GetStudentByIdUseCase.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

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
/// Retrieves a student by their unique identifier (CU-02).
/// </summary>
public sealed class GetStudentByIdUseCase : IUseCase<GetStudentByIdQuery, StudentDto>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetStudentByIdUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetStudentByIdUseCase"/> class.
    /// </summary>
    /// <param name="studentRepository">The student repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger.</param>
    public GetStudentByIdUseCase(IStudentRepository studentRepository, IMapper mapper, ILogger<GetStudentByIdUseCase> logger)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Finds and returns a student by identifier.
    /// </summary>
    /// <param name="request">The query containing the student identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The student DTO.</returns>
    /// <exception cref="StudentNotFoundException">Thrown when the student does not exist.</exception>
    public async Task<StudentDto> ExecuteAsync(GetStudentByIdQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Getting student {StudentId}", request.StudentId);

        Student? student = await _studentRepository.GetByIdAsync(new StudentId(request.StudentId), ct);
        if (student is null)
        {
            _logger.LogWarning("Student {StudentId} not found", request.StudentId);
            throw new StudentNotFoundException(request.StudentId);
        }

        _logger.LogInformation("Student {StudentId} retrieved", request.StudentId);
        return _mapper.Map<StudentDto>(student);
    }
}
