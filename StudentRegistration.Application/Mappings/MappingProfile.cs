// <copyright file="MappingProfile.cs" company="Inter Rapidísimo">
//   Copyright (c) Inter Rapidísimo. All rights reserved.
// </copyright>

using AutoMapper;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Application.Mappings;

/// <summary>
/// AutoMapper profile defining all mappings between domain entities and DTOs.
/// </summary>
public sealed class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class and registers all maps.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Student, StudentDto>()
            .ForMember(d => d.StudentId, o => o.MapFrom(s => s.StudentId.Value))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email.Value))
            .ForMember(d => d.TotalCredits, o => o.MapFrom(s => s.Enrollments.Count(e => e.IsActive) * 3))
            .ForMember(d => d.Enrollments, o => o.MapFrom(s => s.Enrollments));

        CreateMap<Student, StudentSummaryDto>()
            .ForMember(d => d.StudentId, o => o.MapFrom(s => s.StudentId.Value))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email.Value));

        CreateMap<Student, ClassmateDto>()
            .ForMember(d => d.StudentId, o => o.MapFrom(s => s.StudentId.Value));

        CreateMap<Enrollment, EnrollmentDto>()
            .ForMember(d => d.EnrollmentId, o => o.MapFrom(s => s.EnrollmentId.Value))
            .ForMember(d => d.SubjectId, o => o.MapFrom(s => s.SubjectId.Value))
            .ForMember(d => d.SubjectName, o => o.MapFrom(s => s.Subject != null ? s.Subject.Name : string.Empty))
            .ForMember(d => d.Credits, o => o.MapFrom(s => s.Subject != null ? s.Subject.Credits : 0))
            .ForMember(d => d.ProfessorName, o => o.MapFrom(s => s.Subject != null && s.Subject.Professor != null ? s.Subject.Professor.FullName : string.Empty));

        CreateMap<Subject, SubjectDto>()
            .ForMember(d => d.SubjectId, o => o.MapFrom(s => s.SubjectId.Value))
            .ForMember(d => d.ProfessorId, o => o.MapFrom(s => s.ProfessorId.Value))
            .ForMember(d => d.ProfessorName, o => o.MapFrom(s => s.Professor != null ? s.Professor.FullName : string.Empty));
    }
}
