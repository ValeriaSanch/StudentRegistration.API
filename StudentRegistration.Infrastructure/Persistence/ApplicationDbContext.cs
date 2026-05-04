// <copyright file="ApplicationDbContext.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Infrastructure.Persistence;

/// <summary>
/// Entity Framework Core database context for the student registration system.
/// </summary>
public sealed class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The context options.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>Gets or sets the students table.</summary>
    public DbSet<Student> Students => Set<Student>();

    /// <summary>Gets or sets the professors table.</summary>
    public DbSet<Professor> Professors => Set<Professor>();

    /// <summary>Gets or sets the subjects table.</summary>
    public DbSet<Subject> Subjects => Set<Subject>();

    /// <summary>Gets or sets the enrollments table.</summary>
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
