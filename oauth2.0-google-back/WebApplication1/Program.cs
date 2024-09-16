using Microsoft.Extensions.Configuration;
using WebApplication1.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<OAuthSettings>();

var allowOriginsCors = "allowOriginsCors";

builder.Services.AddCors(opt =>
    opt.AddPolicy(name: allowOriginsCors,
        policy => {
            policy.AllowAnyHeader().
            AllowAnyOrigin().
            AllowAnyMethod();
        })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(allowOriginsCors);

app.MapControllers();

app.Run();
