// <copyright file="EnrollStudentUseCaseTests.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StudentRegistration.Application.Commands;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Exceptions;
using StudentRegistration.Application.Mappings;
using StudentRegistration.Application.UseCases;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Exceptions;
using StudentRegistration.Domain.Interfaces;
using StudentRegistration.Domain.ValueObjects;
using Xunit;

namespace StudentRegistration.Tests.Application;

/// <summary>
/// Unit tests for <see cref="EnrollStudentUseCase"/>.
/// </summary>
public sealed class EnrollStudentUseCaseTests
{
    private readonly Mock<IStudentRepository> _studentRepo = new();
    private readonly Mock<ISubjectRepository> _subjectRepo = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnrollStudentUseCaseTests"/> class.
    /// </summary>
    public EnrollStudentUseCaseTests()
    {
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _unitOfWork.Setup(u => u.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
    }

    private EnrollStudentUseCase CreateUseCase() =>
        new(_studentRepo.Object, _subjectRepo.Object, _unitOfWork.Object, _mapper, NullLogger<EnrollStudentUseCase>.Instance);

    private static Student MakeStudent() =>
        Student.Create("Jane Doe", new Email("jane@example.com"), "99999999");

    private static Subject MakeSubject(Guid? profId = null) =>
        Subject.Create(new SubjectId(Guid.NewGuid()), "Matemáticas", 3, new ProfessorId(profId ?? Guid.NewGuid()));

    // ── Happy path ───────────────────────────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_ValidCommand_ReturnsEnrollmentDto()
    {
        Student student = MakeStudent();
        Subject subject = MakeSubject();
        EnrollStudentCommand command = new EnrollStudentCommand { StudentId = student.StudentId.Value, SubjectId = subject.SubjectId.Value };

        _studentRepo.Setup(r => r.GetByIdAsync(It.IsAny<StudentId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _subjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<SubjectId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        EnrollmentDto result = await CreateUseCase().ExecuteAsync(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.SubjectId.Should().Be(subject.SubjectId.Value);
    }

    // ── Student not found ─────────────────────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_StudentNotFound_ThrowsStudentNotFoundException()
    {
        _studentRepo.Setup(r => r.GetByIdAsync(It.IsAny<StudentId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        EnrollStudentCommand command = new EnrollStudentCommand { StudentId = Guid.NewGuid(), SubjectId = Guid.NewGuid() };
        Func<Task> act = async () => await CreateUseCase().ExecuteAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<StudentNotFoundException>();
    }

    // ── Subject not found ─────────────────────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_SubjectNotFound_ThrowsSubjectNotFoundException()
    {
        Student student = MakeStudent();
        _studentRepo.Setup(r => r.GetByIdAsync(It.IsAny<StudentId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _subjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<SubjectId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subject?)null);

        EnrollStudentCommand command = new EnrollStudentCommand { StudentId = student.StudentId.Value, SubjectId = Guid.NewGuid() };
        Func<Task> act = async () => await CreateUseCase().ExecuteAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<SubjectNotFoundException>();
    }

    // ── BR-01: Max enrollments exceeded ──────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_StudentHasThreeEnrollments_ThrowsMaxEnrollmentsExceededException()
    {
        Student student = MakeStudent();
        student.AddEnrollment(MakeSubject());
        student.AddEnrollment(MakeSubject());
        student.AddEnrollment(MakeSubject());

        Subject fourthSubject = MakeSubject();

        _studentRepo.Setup(r => r.GetByIdAsync(It.IsAny<StudentId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _subjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<SubjectId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fourthSubject);

        EnrollStudentCommand command = new EnrollStudentCommand { StudentId = student.StudentId.Value, SubjectId = fourthSubject.SubjectId.Value };
        Func<Task> act = async () => await CreateUseCase().ExecuteAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<MaxEnrollmentsExceededException>();
    }

    // ── BR-02: Same professor conflict ────────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_SameProfessor_ThrowsSameProfessorConflictException()
    {
        Guid sharedProfId = Guid.NewGuid();
        Student student = MakeStudent();
        student.AddEnrollment(MakeSubject(sharedProfId));

        Subject conflictSubject = MakeSubject(sharedProfId);

        _studentRepo.Setup(r => r.GetByIdAsync(It.IsAny<StudentId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _subjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<SubjectId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(conflictSubject);

        EnrollStudentCommand command = new EnrollStudentCommand { StudentId = student.StudentId.Value, SubjectId = conflictSubject.SubjectId.Value };
        Func<Task> act = async () => await CreateUseCase().ExecuteAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<SameProfessorConflictException>();
    }

    // ── BR-04: Inactive student ───────────────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_InactiveStudent_ThrowsStudentInactiveException()
    {
        Student student = MakeStudent();
        student.Deactivate();
        Subject subject = MakeSubject();

        _studentRepo.Setup(r => r.GetByIdAsync(It.IsAny<StudentId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _subjectRepo.Setup(r => r.GetByIdAsync(It.IsAny<SubjectId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subject);

        EnrollStudentCommand command = new EnrollStudentCommand { StudentId = student.StudentId.Value, SubjectId = subject.SubjectId.Value };
        Func<Task> act = async () => await CreateUseCase().ExecuteAsync(command, CancellationToken.None);

        await act.Should().ThrowAsync<StudentInactiveException>();
    }
}
