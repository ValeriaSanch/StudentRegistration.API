// <copyright file="EnrollStudentUseCase.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.Commands;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Exceptions;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Exceptions;
using StudentRegistration.Domain.Interfaces;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Application.UseCases;

/// <summary>
/// Enrolls a student in a subject, enforcing all domain business rules (CU-06).
/// </summary>
public sealed class EnrollStudentUseCase : IUseCase<EnrollStudentCommand, EnrollmentDto>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EnrollStudentUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnrollStudentUseCase"/> class.
    /// </summary>
    /// <param name="studentRepository">The student repository.</param>
    /// <param name="subjectRepository">The subject repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger.</param>
    public EnrollStudentUseCase(
        IStudentRepository studentRepository,
        ISubjectRepository subjectRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<EnrollStudentUseCase> logger)
    {
        _studentRepository = studentRepository;
        _subjectRepository = subjectRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Enrolls the student in the specified subject after validating domain rules.
    /// </summary>
    /// <param name="request">The enrollment command.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The enrollment DTO.</returns>
    /// <exception cref="StudentNotFoundException">Thrown when the student does not exist.</exception>
    /// <exception cref="SubjectNotFoundException">Thrown when the subject does not exist.</exception>
    /// <exception cref="StudentInactiveException">Thrown when the student is inactive (BR-04).</exception>
    /// <exception cref="MaxEnrollmentsExceededException">Thrown when BR-01 is violated.</exception>
    /// <exception cref="SameProfessorConflictException">Thrown when BR-02 is violated.</exception>
    /// <exception cref="DuplicateEnrollmentException">Thrown when BR-09 is violated.</exception>
    public async Task<EnrollmentDto> ExecuteAsync(EnrollStudentCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Enrolling student {StudentId} in subject {SubjectId}", request.StudentId, request.SubjectId);

        Student? student = await _studentRepository.GetByIdAsync(new StudentId(request.StudentId), ct);
        if (student is null)
        {
            _logger.LogWarning("Student {StudentId} not found", request.StudentId);
            throw new StudentNotFoundException(request.StudentId);
        }

        Subject? subject = await _subjectRepository.GetByIdAsync(new SubjectId(request.SubjectId), ct);
        if (subject is null)
        {
            _logger.LogWarning("Subject {SubjectId} not found", request.SubjectId);
            throw new SubjectNotFoundException(request.SubjectId);
        }

        student.AddEnrollment(subject);

        await _studentRepository.UpdateAsync(student, ct);
        await _unitOfWork.CommitAsync(ct);

        Enrollment enrollment = student.Enrollments.Last();
        EnrollmentDto dto = _mapper.Map<EnrollmentDto>(enrollment);
        dto.SubjectName = subject.Name;
        dto.Credits = subject.Credits;
        dto.ProfessorName = subject.Professor?.FullName ?? string.Empty;

        _logger.LogInformation("Student {StudentId} enrolled in subject {SubjectId}", request.StudentId, request.SubjectId);
        return dto;
    }
}
