using Scalar.AspNetCore;
using Api.Extensions;
using Api.Middlewares;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddSecurities(builder.Configuration);

builder.Services.AddDependencyServices();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();        // scalar
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseMiddleware<AntiforgeryValidationMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseCsrfTokenCookie();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();

