using Base.AzureServices.BlobStorage;
using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using Base.Generic.Persistence.Repositories;
using Base.Security.Sha256M;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;
using SquirrelsBox.Storage.Persistence.Repositories;
using SquirrelsBox.Storage.Services;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

// Add services to the container.

builder.Services.AddControllers();

// Repositories init
builder.Services.AddScoped<IGenericSearchRepository, ASearchRepository>();

builder.Services.AddScoped<IGenericRepository<SharedBox>, SharedBoxRepository>();
builder.Services.AddScoped<IGenericReadRepository<SharedBox>, SharedBoxRepository>();

builder.Services.AddScoped<IGenericRepositoryWithCascade<Box>, BoxRepository>();
builder.Services.AddScoped<IGenericReadRepository<Box>, BoxRepository>();

builder.Services.AddScoped<IGenericRepositoryWithCascade<BoxSectionRelationship>, SectionRepository>();
builder.Services.AddScoped<IGenericReadRepository<BoxSectionRelationship>, SectionRepository>();

builder.Services.AddScoped<IGenericRepository<SectionItemRelationship>, ItemRepository>();
builder.Services.AddScoped<IGenericReadRepository<SectionItemRelationship>, ItemRepository>();

builder.Services.AddScoped<IGenericRepositoryWithMassive<Spec>, SpecRepository>();
builder.Services.AddScoped<IGenericReadRepository<Spec>, SpecRepository>();

// Services init
builder.Services.AddScoped<IGenericSearchService, ASearchService>();

builder.Services.AddScoped<IGenericService<SharedBox, SharedBoxResponse>, SharedBoxService>();
builder.Services.AddScoped<IGenericReadService<SharedBox, SharedBoxResponse>, SharedBoxService>();

builder.Services.AddScoped<IGenericServiceWithCascade<Box, BoxResponse>, BoxService>();
builder.Services.AddScoped<IGenericReadService<Box, BoxResponse>, BoxService>();

builder.Services.AddScoped<IGenericServiceWithCascade<BoxSectionRelationship, BoxSectionRelationshipResponse>, SectionService>();
builder.Services.AddScoped<IGenericReadService<BoxSectionRelationship, BoxSectionRelationshipResponse>, SectionService>();

builder.Services.AddScoped<IGenericService<SectionItemRelationship, SectionItemRelationshipResponse>, ItemService>();
builder.Services.AddScoped<IGenericReadService<SectionItemRelationship, SectionItemRelationshipResponse>, ItemService>();

builder.Services.AddScoped<IGenericServiceWithMassive<Spec, ItemSpecRelationshipResponse>, SpecService>();
builder.Services.AddScoped<IGenericReadService<Spec, ItemSpecRelationshipResponse>, SpecService>();

builder.Services.AddScoped<IContainerService, ContainerService>();

builder.Services.AddScoped<IUnitOfWork<AppDbContext>, UnitOfWork<AppDbContext>>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpClient();

if (environment.IsDevelopment())
{
    // Register BlobStorageCredentials configuration for development environment
    builder.Services.Configure<BlobStorageCredentials>(builder.Configuration.GetSection("BlobStorage"));
}
else
{
    // Optionally configure for production, or load from environment variables as needed
    var blobStorageConnectionString = Environment.GetEnvironmentVariable("BlobStorageConnectionString");
    var blobStorageContainerName = Environment.GetEnvironmentVariable("BlobStorageContainerName");

    builder.Services.Configure<BlobStorageCredentials>(options =>
    {
        options.ConnectionString = blobStorageConnectionString;
        options.ContainerName = blobStorageContainerName;
    });
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString;
    if (environment.IsDevelopment())
    {
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    }
    else
    {
        connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
    }

    options.UseSqlServer(connectionString);
});

//builder.Services.Configure<JwtKeys>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<JwtKeys>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Read from appsettings.json in development
        options.Key = builder.Configuration["Jwt:Key"];
        options.Issuer = builder.Configuration["Jwt:Issuer"];
        options.Audience = builder.Configuration["Jwt:Audience"];
    }
    else
    {
        // Read from environment variables in production
        options.Key = Environment.GetEnvironmentVariable("Jwt__Key");
        options.Issuer = Environment.GetEnvironmentVariable("Jwt__Issuer");
        options.Audience = Environment.GetEnvironmentVariable("Jwt__Audience");
    }
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Squirrel's Box Storage API", Version = "v1" });

    // Define the OAuth2.0 scheme that's in use (i.e., Implicit Flow)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
