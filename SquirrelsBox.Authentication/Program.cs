using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using Base.Generic.Persistence.Repositories;
using Base.Security;
using Base.Security.Sha256M;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SquirrelsBox.Authentication.Domain.Communication;
using SquirrelsBox.Authentication.Domain.Interfaces;
using SquirrelsBox.Authentication.Domain.Models;
using SquirrelsBox.Authentication.Persistence.Context;
using SquirrelsBox.Authentication.Persistence.Repositories;
using SquirrelsBox.Authentication.Services;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IGenericReadRepository<AssignedPermission>, AssignedPermissionRepository>();
builder.Services.AddScoped<IGenericRepository<AssignedPermission>, AssignedPermissionRepository>();
builder.Services.AddScoped<IGenericReadService<AssignedPermission, AssignedPermissionResponse>, AssignedPermissionsService>();
builder.Services.AddScoped<IGenericService<AssignedPermission, AssignedPermissionResponse>, AssignedPermissionsService>();

builder.Services.AddScoped<IAccessSessionRepository, AccessSessionRepository>();
builder.Services.AddScoped<IAccessSessionService, AccessSessionService>();
builder.Services.AddScoped<IGenericRepository<AccessSession>, AccessSessionRepository>();
builder.Services.AddScoped<IGenericRepository<DeviceSession>, DeviceSessionRepository>();
builder.Services.AddScoped<IDeviceSessionRepository, DeviceSessionRepository>();
builder.Services.AddScoped<IGenericRepository<UserSession>, UserSessionRespository>();
builder.Services.AddScoped<IUserSessionRepository, UserSessionRespository>();
builder.Services.AddScoped<IGenericService<AccessSession, AccessSessionResponse>, AccessSessionService>();
builder.Services.AddScoped<IGenericService<DeviceSession, DeviceSessionResponse>, DeviceTokenService>();
builder.Services.AddScoped<IGenericService<UserSession, UserSessionResponse>, UserSessionService>();

builder.Services.AddScoped<IGenericRepository<UserData>, UserDataRepository>();
builder.Services.AddScoped<IGenericService<UserData, UserDataResponse>, UserDataService>();

builder.Services.AddScoped<IUnitOfWork<AppDbContext>, UnitOfWork<AppDbContext>>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpClient();

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

//builder.Services.Configure<AESConstantes>(builder.Configuration.GetSection("EncryptionSettings"));
builder.Services.Configure<AESConstantes>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Read from appsettings.json in development
        options.Key = builder.Configuration["EncryptionSettings:Key"];
        options.IV = builder.Configuration["EncryptionSettings:IV"];
    }
    else
    {
        // Read from environment variables in production
        options.Key = Environment.GetEnvironmentVariable("EncryptionSettings__Key");
        options.IV = Environment.GetEnvironmentVariable("EncryptionSettings__IV");
    }
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Squirrel's Box Authentication API", Version = "v1" });

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
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
