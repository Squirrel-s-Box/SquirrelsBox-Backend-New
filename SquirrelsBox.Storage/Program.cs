using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;
using SquirrelsBox.Storage.Persistence.Repositories;
using SquirrelsBox.Storage.Services;
using System;

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

//builder.Services.AddTransient<IImageUploadService>(provider =>
//{
//    string blobStorageConnectionString;
//    if (builder.Environment.IsDevelopment())
//    {
//        blobStorageConnectionString = builder.Configuration.GetConnectionString("BlobStorageConnectionString");
//    }
//    else
//    {
//        blobStorageConnectionString = Environment.GetEnvironmentVariable("BlobStorageConnectionString");
//    }
//    string containerName = builder.Configuration["ContainerName"];
//    return new ImageUploadService(blobStorageConnectionString, containerName);
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
