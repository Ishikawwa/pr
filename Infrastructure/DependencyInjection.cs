using Application.Behaviour.Organizations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string DefaultConnection not found.");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services.AddRepositories();

        services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(GetOrganizationsQuery).Assembly));

        services.AddValidatorsFromAssembly(typeof(CreateOrganizationValidator).Assembly);

        return services;
    }
}