// <copyright file="Email.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using System.Text.RegularExpressions;
using StudentRegistration.Domain.Exceptions;

namespace StudentRegistration.Domain.ValueObjects;

/// <summary>
/// Represents a validated email address as an immutable value object.
/// </summary>
public sealed class Email : IEquatable<Email>
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="Email"/> class.
    /// </summary>
    /// <param name="value">The raw email address string.</param>
    /// <exception cref="InvalidEmailException">Thrown when the format is invalid.</exception>
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !EmailRegex.IsMatch(value))
            throw new InvalidEmailException();

        Value = value.ToLowerInvariant();
    }

    /// <summary>
    /// Gets the normalized email address value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Returns whether this email equals another.
    /// </summary>
    /// <param name="other">The other email to compare.</param>
    /// <returns><c>true</c> if both emails have the same value.</returns>
    public bool Equals(Email? other) => other is not null && Value == other.Value;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Email email && Equals(email);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public override string ToString() => Value;
}
