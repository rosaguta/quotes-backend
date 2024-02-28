using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.EnableAnnotations();
    opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Quotes/Rizz Back(shots)end" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new String[] { }
        }
    });
});

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = false,

        ValidIssuer = "DigitalIndividuals",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisisasecretkey@1234567890123456"))
    };
});

// Add CORS services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        a =>
        {
            a.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "Quotes/Rizz Back(shots)end";
    c.InjectStylesheet("/Content/css/DIV.css");
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "There is no other version why even bother clicking here");
});

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Content")),
    RequestPath = "/Content"
});

app.UseHttpsRedirection();

app.UseRouting();

// Use CORS middleware with the previously defined policy.
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
