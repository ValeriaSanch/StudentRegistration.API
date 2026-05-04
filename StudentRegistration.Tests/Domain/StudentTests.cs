// <copyright file="StudentTests.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using System;
using System.Linq;
using FluentAssertions;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Exceptions;
using StudentRegistration.Domain.ValueObjects;
using Xunit;

namespace StudentRegistration.Tests.Domain;

/// <summary>
/// Unit tests for domain business rules on the Student aggregate root.
/// </summary>
public sealed class StudentTests
{
    private static Student CreateActiveStudent() =>
        Student.Create("John Doe", new Email("john@example.com"), "12345678");

    private static Subject CreateSubject(Guid? subjectId = null, Guid? professorId = null) =>
        Subject.Create(
            new SubjectId(subjectId ?? Guid.NewGuid()),
            "Test Subject",
            3,
            new ProfessorId(professorId ?? Guid.NewGuid()));

    // ── BR-01: Maximum 3 active enrollments ──────────────────────────────────

    [Fact]
    public void AddEnrollment_WhenThreeAlreadyActive_ThrowsMaxEnrollmentsExceededException()
    {
        Student student = CreateActiveStudent();

        student.AddEnrollment(CreateSubject());
        student.AddEnrollment(CreateSubject());
        student.AddEnrollment(CreateSubject());

        Subject fourthSubject = CreateSubject();
        Action act = () => student.AddEnrollment(fourthSubject);

        act.Should().Throw<MaxEnrollmentsExceededException>();
    }

    [Fact]
    public void AddEnrollment_WhenCancelledEnrollmentsExist_CountsOnlyActive()
    {
        Student student = CreateActiveStudent();
        Subject subject1 = CreateSubject();
        Subject subject2 = CreateSubject();
        Subject subject3 = CreateSubject();

        student.AddEnrollment(subject1);
        student.AddEnrollment(subject2);
        student.AddEnrollment(subject3);

        // Cancel one so only 2 are active
        student.Enrollments.First().Cancel();

        // Now we should be able to add a 3rd active
        Subject subject4 = CreateSubject();
        Action act = () => student.AddEnrollment(subject4);

        act.Should().NotThrow();
    }

    // ── BR-02: Same professor conflict ───────────────────────────────────────

    [Fact]
    public void AddEnrollment_WhenSameProfessor_ThrowsSameProfessorConflictException()
    {
        Student student = CreateActiveStudent();
        Guid sharedProfId = Guid.NewGuid();

        Subject subject1 = CreateSubject(professorId: sharedProfId);
        student.AddEnrollment(subject1);

        Subject subject2 = CreateSubject(professorId: sharedProfId);
        Action act = () => student.AddEnrollment(subject2);

        act.Should().Throw<SameProfessorConflictException>();
    }

    // ── BR-04: Inactive student cannot enroll ────────────────────────────────

    [Fact]
    public void AddEnrollment_WhenStudentInactive_ThrowsStudentInactiveException()
    {
        Student student = CreateActiveStudent();
        student.Deactivate();

        Action act = () => student.AddEnrollment(CreateSubject());

        act.Should().Throw<StudentInactiveException>();
    }

    // ── BR-09: Duplicate enrollment ──────────────────────────────────────────

    [Fact]
    public void AddEnrollment_WhenDuplicateSubject_ThrowsDuplicateEnrollmentException()
    {
        Student student = CreateActiveStudent();
        Guid subjectId = Guid.NewGuid();
        Subject subject = CreateSubject(subjectId: subjectId);

        student.AddEnrollment(subject);

        Subject sameProfOtherSubject = CreateSubject(subjectId: subjectId);
        Action act = () => student.AddEnrollment(sameProfOtherSubject);

        act.Should().Throw<DuplicateEnrollmentException>();
    }

    // ── Deactivate cancels all active enrollments ─────────────────────────────

    [Fact]
    public void Deactivate_CancelsAllActiveEnrollments()
    {
        Student student = CreateActiveStudent();
        student.AddEnrollment(CreateSubject());
        student.AddEnrollment(CreateSubject());

        student.Deactivate();

        student.IsActive.Should().BeFalse();
        student.Enrollments.All(e => !e.IsActive).Should().BeTrue();
    }

    // ── BR-10: After cancel a slot frees up ───────────────────────────────────

    [Fact]
    public void AddEnrollment_AfterCancellation_AllowsNewEnrollment()
    {
        Student student = CreateActiveStudent();
        Subject subjectToCancel = CreateSubject();

        student.AddEnrollment(CreateSubject());
        student.AddEnrollment(CreateSubject());
        student.AddEnrollment(subjectToCancel);

        student.Enrollments.Last().Cancel();

        Subject newSubject = CreateSubject();
        Action act = () => student.AddEnrollment(newSubject);

        act.Should().NotThrow();
    }
}
