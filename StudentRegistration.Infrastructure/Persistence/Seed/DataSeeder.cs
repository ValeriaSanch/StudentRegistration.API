// <copyright file="DataSeeder.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.ValueObjects;

namespace StudentRegistration.Infrastructure.Persistence.Seed;

/// <summary>
/// Seeds the required 5 professors and 10 subjects on first run (BR-06, BR-07).
/// </summary>
public static class DataSeeder
{
    // Fixed professor GUIDs for idempotent seeding
    private static readonly Guid ProfAnaMartinez = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid ProfCarlosLopez = Guid.Parse("10000000-0000-0000-0000-000000000002");
    private static readonly Guid ProfMariaTorres = Guid.Parse("10000000-0000-0000-0000-000000000003");
    private static readonly Guid ProfJuanRodriguez = Guid.Parse("10000000-0000-0000-0000-000000000004");
    private static readonly Guid ProfSofiaRamirez = Guid.Parse("10000000-0000-0000-0000-000000000005");

    // Fixed subject GUIDs for idempotent seeding
    private static readonly Guid SubjMatematicas = Guid.Parse("20000000-0000-0000-0000-000000000001");
    private static readonly Guid SubjFisica = Guid.Parse("20000000-0000-0000-0000-000000000002");
    private static readonly Guid SubjHistoria = Guid.Parse("20000000-0000-0000-0000-000000000003");
    private static readonly Guid SubjGeografia = Guid.Parse("20000000-0000-0000-0000-000000000004");
    private static readonly Guid SubjQuimica = Guid.Parse("20000000-0000-0000-0000-000000000005");
    private static readonly Guid SubjBiologia = Guid.Parse("20000000-0000-0000-0000-000000000006");
    private static readonly Guid SubjProgramacion = Guid.Parse("20000000-0000-0000-0000-000000000007");
    private static readonly Guid SubjBasesDatos = Guid.Parse("20000000-0000-0000-0000-000000000008");
    private static readonly Guid SubjIngles = Guid.Parse("20000000-0000-0000-0000-000000000009");
    private static readonly Guid SubjComunicacion = Guid.Parse("20000000-0000-0000-0000-000000000010");

    /// <summary>
    /// Creates the schema (if not exists) and seeds data on first run.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Professors.AnyAsync())
            return;

        Professor[] professors = new[]
        {
            Professor.Create(new ProfessorId(ProfAnaMartinez), "Prof. Ana Martínez"),
            Professor.Create(new ProfessorId(ProfCarlosLopez), "Prof. Carlos López"),
            Professor.Create(new ProfessorId(ProfMariaTorres), "Prof. María Torres"),
            Professor.Create(new ProfessorId(ProfJuanRodriguez), "Prof. Juan Rodríguez"),
            Professor.Create(new ProfessorId(ProfSofiaRamirez), "Prof. Sofía Ramírez"),
        };

        await context.Professors.AddRangeAsync(professors);

        Subject[] subjects = new[]
        {
            Subject.Create(new SubjectId(SubjMatematicas), "Matemáticas", 3, new ProfessorId(ProfAnaMartinez)),
            Subject.Create(new SubjectId(SubjFisica), "Física", 3, new ProfessorId(ProfAnaMartinez)),
            Subject.Create(new SubjectId(SubjHistoria), "Historia", 3, new ProfessorId(ProfCarlosLopez)),
            Subject.Create(new SubjectId(SubjGeografia), "Geografía", 3, new ProfessorId(ProfCarlosLopez)),
            Subject.Create(new SubjectId(SubjQuimica), "Química", 3, new ProfessorId(ProfMariaTorres)),
            Subject.Create(new SubjectId(SubjBiologia), "Biología", 3, new ProfessorId(ProfMariaTorres)),
            Subject.Create(new SubjectId(SubjProgramacion), "Programación", 3, new ProfessorId(ProfJuanRodriguez)),
            Subject.Create(new SubjectId(SubjBasesDatos), "Bases de Datos", 3, new ProfessorId(ProfJuanRodriguez)),
            Subject.Create(new SubjectId(SubjIngles), "Inglés", 3, new ProfessorId(ProfSofiaRamirez)),
            Subject.Create(new SubjectId(SubjComunicacion), "Comunicación", 3, new ProfessorId(ProfSofiaRamirez)),
        };

        await context.Subjects.AddRangeAsync(subjects);
        await context.SaveChangesAsync();
    }
}
