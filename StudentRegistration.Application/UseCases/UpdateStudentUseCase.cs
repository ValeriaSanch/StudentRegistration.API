// <copyright file="UpdateStudentUseCase.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.Commands;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Exceptions;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Interfaces;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Application.UseCases;

/// <summary>
/// Updates an existing student's name and document number (CU-04).
/// </summary>
public sealed class UpdateStudentUseCase : IUseCase<UpdateStudentCommand, StudentDto>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateStudentUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStudentUseCase"/> class.
    /// </summary>
    /// <param name="studentRepository">The student repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger.</param>
    public UpdateStudentUseCase(
        IStudentRepository studentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateStudentUseCase> logger)
    {
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Updates a student's full name and document number.
    /// </summary>
    /// <param name="request">The update command.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated student DTO.</returns>
    /// <exception cref="StudentNotFoundException">Thrown when the student does not exist.</exception>
    public async Task<StudentDto> ExecuteAsync(UpdateStudentCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Updating student {StudentId}", request.StudentId);

        Student? student = await _studentRepository.GetByIdAsync(new StudentId(request.StudentId), ct);
        if (student is null)
        {
            _logger.LogWarning("Student {StudentId} not found for update", request.StudentId);
            throw new StudentNotFoundException(request.StudentId);
        }

        student.Update(request.FullName, request.DocumentNumber);
        await _studentRepository.UpdateAsync(student, ct);
        await _unitOfWork.CommitAsync(ct);

        _logger.LogInformation("Student {StudentId} updated successfully", request.StudentId);
        return _mapper.Map<StudentDto>(student);
    }
}
