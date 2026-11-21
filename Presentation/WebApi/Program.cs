using Infrastructure;
using Microsoft.OpenApi.Models;
using WebApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Academic Performance API",
        Version = "v1",
        Description = "API for managing organizations and students"
    });
});
    
builder.Services.AddProjectDependencies(builder.Configuration);

builder.Services.AddExceptionHandler<StudentExceptionHandler>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.SerializeAsV2 = true);

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
