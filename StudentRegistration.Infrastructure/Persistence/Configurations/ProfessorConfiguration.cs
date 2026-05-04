// <copyright file="ProfessorConfiguration.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the <see cref="Professor"/> entity.
/// </summary>
public sealed class ProfessorConfiguration : IEntityTypeConfiguration<Professor>
{
    /// <summary>
    /// Configures the Professor entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Professor> builder)
    {
        builder.ToTable("professors");

        builder.HasKey(p => p.ProfessorId);
        builder.Property(p => p.ProfessorId)
            .HasColumnName("professor_id")
            .HasColumnType("char(36)")
            .HasConversion(id => id.Value, g => new ProfessorId(g));

        builder.Property(p => p.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Navigation(p => p.Subjects).HasField("_subjects");
    }
}
