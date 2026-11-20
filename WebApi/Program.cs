
using Application.Features.Students.Commands;
using Application.Interfaces.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repositories;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Student API",
        Version = "v1",
        Description = "API for managing students"
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(
        typeof(CreateStudentCommand).Assembly);
});

builder.Services.AddValidatorsFromAssemblyContaining<
    CreateStudentCommandValidator>();

builder.Services.AddExceptionHandler<StudentExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseDeveloperExceptionPage();

//app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "Student API v1");

        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
