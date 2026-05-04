// <copyright file="Program.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StudentRegistration.Application.Abstractions;
using StudentRegistration.Application.Commands;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Mappings;
using StudentRegistration.Application.Queries;
using StudentRegistration.Application.UseCases;
using StudentRegistration.Domain.Interfaces;
using StudentRegistration.Infrastructure.Persistence;
using StudentRegistration.Infrastructure.Persistence.Seed;
using StudentRegistration.Infrastructure.Repositories;
using StudentRegistration.Presentation.Middleware;
using StudentRegistration.Presentation.Validators;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // Serilog
    builder.Host.UseSerilog((ctx, services, config) =>
    {
        config
            .ReadFrom.Configuration(ctx.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "StudentRegistration.API")
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day);
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularApp", policy =>
        {
            policy
                .WithOrigins("https://localhost:4200") // Angular
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    // EF Core + MySQL
    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));

    // AutoMapper
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    // FluentValidation
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<RegisterStudentCommandValidator>();

    // Repositories (Infrastructure adapters)
    builder.Services.AddScoped<IStudentRepository, StudentRepository>();
    builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
    builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    // Use Cases (Application services)
    builder.Services.AddScoped<IUseCase<RegisterStudentCommand, StudentDto>, RegisterStudentUseCase>();
    builder.Services.AddScoped<IUseCase<GetStudentByIdQuery, StudentDto>, GetStudentByIdUseCase>();
    builder.Services.AddScoped<IUseCase<GetAllStudentsQuery, IReadOnlyList<StudentSummaryDto>>, GetAllStudentsUseCase>();
    builder.Services.AddScoped<IUseCase<UpdateStudentCommand, StudentDto>, UpdateStudentUseCase>();
    builder.Services.AddScoped<IUseCase<DeleteStudentCommand>, DeleteStudentUseCase>();
    builder.Services.AddScoped<IUseCase<EnrollStudentCommand, EnrollmentDto>, EnrollStudentUseCase>();
    builder.Services.AddScoped<IUseCase<CancelEnrollmentCommand>, CancelEnrollmentUseCase>();
    builder.Services.AddScoped<IUseCase<GetStudentEnrollmentsQuery, IReadOnlyList<EnrollmentDto>>, GetStudentEnrollmentsUseCase>();
    builder.Services.AddScoped<IUseCase<GetClassmatesQuery, IReadOnlyList<ClassmateDto>>, GetClassmatesUseCase>();
    builder.Services.AddScoped<IUseCase<GetAllSubjectsQuery, IReadOnlyList<SubjectDto>>, GetAllSubjectsUseCase>();

    // Controllers from Presentation project
    builder.Services.AddControllers()
        .AddApplicationPart(typeof(StudentRegistration.Presentation.Controllers.StudentsController).Assembly);

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "StudentRegistration API", Version = "v1", Description = "API de Registro de Estudiantes — Inter Rapidísimo" });
    });

    WebApplication app = builder.Build();

    string aspNetUrls =
        app.Configuration["ASPNETCORE_URLS"]
        ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
        ?? string.Empty;
    bool listensOnHttps =
        aspNetUrls.Contains("https://", StringComparison.OrdinalIgnoreCase);
    bool swaggerEnabled = app.Configuration.GetValue(
        "Swagger:Enabled",
        app.Environment.IsDevelopment());

    // Seed database
    using (IServiceScope scope = app.Services.CreateScope())
    {
        ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await DataSeeder.SeedAsync(db);
    }

    app.UseCors("CorsPolicy");

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseSerilogRequestLogging();

    if (swaggerEnabled)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentRegistration API v1"));
    }

    // Evita WRN cuando el contenedor solo expone HTTP (sin ASPNETCORE_HTTPS_PORTS).
    if (listensOnHttps)
    {
        app.UseHttpsRedirection();
    }

    app.UseAuthorization();

    if (swaggerEnabled)
    {
        app.MapGet("/", () => Results.Redirect("/swagger"));
    }
    else
    {
        app.MapGet("/", () => Results.Text(
            "StudentRegistration.API activa — rutas bajo /api/v1/",
            "text/plain; charset=utf-8"));
    }

    app.MapControllers();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
