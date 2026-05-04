// <copyright file="RegisterStudentUseCase.cs" company="Inter Rapidísimo">
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
/// Registers a new student in the system (CU-01).
/// </summary>
public sealed class RegisterStudentUseCase : IUseCase<RegisterStudentCommand, StudentDto>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<RegisterStudentUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterStudentUseCase"/> class.
    /// </summary>
    /// <param name="studentRepository">The student repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger.</param>
    public RegisterStudentUseCase(
        IStudentRepository studentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<RegisterStudentUseCase> logger)
    {
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Registers a new student after verifying uniqueness of email and document number.
    /// </summary>
    /// <param name="request">The registration command.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The newly created student DTO.</returns>
    /// <exception cref="EmailAlreadyExistsException">Thrown when the email is already registered.</exception>
    /// <exception cref="DocumentAlreadyExistsException">Thrown when the document number is already registered.</exception>
    public async Task<StudentDto> ExecuteAsync(RegisterStudentCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Registering student with email {Email}", request.Email);

        Email email = new Email(request.Email);

        Student? existing = await _studentRepository.GetByEmailAsync(email, ct);
        if (existing is not null)
        {
            _logger.LogWarning("Email {Email} already exists", request.Email);
            throw new EmailAlreadyExistsException(request.Email);
        }

        Student? byDoc = await _studentRepository.GetByDocumentAsync(request.DocumentNumber, ct);
        if (byDoc is not null)
        {
            _logger.LogWarning("Document {DocumentNumber} already exists", request.DocumentNumber);
            throw new DocumentAlreadyExistsException(request.DocumentNumber);
        }

        Student student = Student.Create(request.FullName, email, request.DocumentNumber);
        await _studentRepository.AddAsync(student, ct);
        await _unitOfWork.CommitAsync(ct);

        _logger.LogInformation("Student {StudentId} registered successfully", student.StudentId.Value);
        return _mapper.Map<StudentDto>(student);
    }
}
