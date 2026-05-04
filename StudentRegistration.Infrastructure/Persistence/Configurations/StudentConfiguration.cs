// <copyright file="StudentConfiguration.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the <see cref="Student"/> entity.
/// </summary>
public sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    /// <summary>
    /// Configures the Student entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("students");

        builder.HasKey(s => s.StudentId);
        builder.Property(s => s.StudentId)
            .HasColumnName("student_id")
            .HasColumnType("char(36)")
            .HasConversion(id => id.Value, g => new StudentId(g));

        builder.Property(s => s.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(s => s.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired()
            .HasConversion(e => e.Value, s => new Email(s));

        builder.HasIndex(s => s.Email).IsUnique();

        builder.Property(s => s.DocumentNumber)
            .HasColumnName("document_number")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(s => s.DocumentNumber).IsUnique();

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        builder.Property(s => s.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(s => s.DocumentNumber).HasDatabaseName("idx_students_document");
        builder.HasIndex(s => s.Email).HasDatabaseName("idx_students_email");

        builder.HasMany(s => s.Enrollments)
            .WithOne()
            .HasForeignKey(e => e.StudentId);

        builder.Navigation(s => s.Enrollments).HasField("_enrollments");
    }
}
