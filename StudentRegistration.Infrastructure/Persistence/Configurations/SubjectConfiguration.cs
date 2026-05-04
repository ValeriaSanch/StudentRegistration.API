// <copyright file="SubjectConfiguration.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the <see cref="Subject"/> entity.
/// </summary>
public sealed class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    /// <summary>
    /// Configures the Subject entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("subjects");

        builder.HasKey(s => s.SubjectId);
        builder.Property(s => s.SubjectId)
            .HasColumnName("subject_id")
            .HasColumnType("char(36)")
            .HasConversion(id => id.Value, g => new SubjectId(g));

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(s => s.Name).IsUnique();

        builder.Property(s => s.Credits)
            .HasColumnName("credits")
            .IsRequired()
            .HasDefaultValue(3);

        builder.Property(s => s.ProfessorId)
            .HasColumnName("professor_id")
            .HasColumnType("char(36)")
            .HasConversion(id => id.Value, g => new ProfessorId(g));

        builder.HasOne(s => s.Professor)
            .WithMany(p => p.Subjects)
            .HasForeignKey(s => s.ProfessorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(s => s.Enrollments)
            .HasField("_enrollments")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
