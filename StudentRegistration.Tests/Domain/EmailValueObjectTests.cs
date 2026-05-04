// <copyright file="EmailValueObjectTests.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using System;
using FluentAssertions;
using StudentRegistration.Domain.Exceptions;
using StudentRegistration.Domain.ValueObjects;
using Xunit;

namespace StudentRegistration.Tests.Domain;

/// <summary>
/// Unit tests for the <see cref="Email"/> value object.
/// </summary>
public sealed class EmailValueObjectTests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("User.Name+tag@sub.domain.org")]
    [InlineData("test123@test.io")]
    public void Email_WithValidFormat_CreatesSuccessfully(string address)
    {
        Action act = () => new Email(address);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("notanemail")]
    [InlineData("@nodomain.com")]
    [InlineData("noatsign.com")]
    public void Email_WithInvalidFormat_ThrowsInvalidEmailException(string address)
    {
        Action act = () => new Email(address);
        act.Should().Throw<InvalidEmailException>();
    }

    [Fact]
    public void Email_NormalizesToLowerCase()
    {
        Email email = new Email("User@EXAMPLE.COM");
        email.Value.Should().Be("user@example.com");
    }

    [Fact]
    public void Email_EqualityByValue_WhenSameAddress()
    {
        Email a = new Email("user@example.com");
        Email b = new Email("USER@EXAMPLE.COM");

        a.Should().Be(b);
        a.Equals(b).Should().BeTrue();
    }

    [Fact]
    public void Email_NotEqual_WhenDifferentAddress()
    {
        Email a = new Email("a@example.com");
        Email b = new Email("b@example.com");

        a.Should().NotBe(b);
    }
}
