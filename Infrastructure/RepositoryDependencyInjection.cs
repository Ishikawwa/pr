using Application.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Infrastructure;

public static class RepositoryDependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStudyGroupRepository, StudyGroupRepository>();
        services.AddScoped<IAcademicPerformanceRecordRepository, AcademicPerformanceRecordRepository>();
        services.AddScoped<IStudentDisciplineGradeRepository, StudentDisciplineGradeRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ISuborganizationRepository, SuborganizationRepository>();
        services.AddScoped<IStudyProgramRepository, StudyProgramRepository>();
        services.AddScoped<IStudyPlanRepository, StudyPlanRepository>();
        services.AddScoped<IFgosDisciplineRepository, FgosDisciplineRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployeePositionRepository, EmployeePositionRepository>();

        return services;
    }
}