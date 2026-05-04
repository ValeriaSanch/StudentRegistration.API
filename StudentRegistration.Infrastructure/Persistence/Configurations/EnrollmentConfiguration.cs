// <copyright file="EnrollmentConfiguration.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the <see cref="Enrollment"/> entity.
/// </summary>
public sealed class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    /// <summary>
    /// Configures the Enrollment entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("enrollments");

        builder.HasKey(e => e.EnrollmentId);
        builder.Property(e => e.EnrollmentId)
            .HasColumnName("enrollment_id")
            .HasColumnType("char(36)")
            .HasConversion(id => id.Value, g => new EnrollmentId(g));

        builder.Property(e => e.StudentId)
            .HasColumnName("student_id")
            .HasColumnType("char(36)")
            .HasConversion(id => id.Value, g => new StudentId(g));

        builder.Property(e => e.SubjectId)
            .HasColumnName("subject_id")
            .HasColumnType("char(36)")
            .HasConversion(id => id.Value, g => new SubjectId(g));

        builder.Property(e => e.ProfessorId)
            .HasColumnName("professor_id")
            .HasColumnType("char(36)")
            .HasConversion(id => id.Value, g => new ProfessorId(g));

        builder.Property(e => e.EnrolledAt)
            .HasColumnName("enrolled_at")
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasColumnName("is_active")
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(e => new { e.StudentId, e.SubjectId, e.IsActive })
            .HasDatabaseName("uq_student_subject_active");

        builder.HasIndex(e => e.StudentId).HasDatabaseName("idx_enrollments_student");
        builder.HasIndex(e => e.SubjectId).HasDatabaseName("idx_enrollments_subject");

        builder.HasOne(e => e.Subject)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
