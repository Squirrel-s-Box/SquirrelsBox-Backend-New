using Base.Generic.Domain.Repositories;
using Base.Generic.Domain.Services;
using Base.Generic.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using SquirrelsBox.Storage.Domain.Communication;
using SquirrelsBox.Storage.Domain.Models;
using SquirrelsBox.Storage.Persistence.Context;
using SquirrelsBox.Storage.Persistence.Repositories;
using SquirrelsBox.Storage.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Repositories init
builder.Services.AddScoped<IGenericRepositoryWithCascade<Box>, BoxRepository>();
builder.Services.AddScoped<IGenericReadRepository<Box>, BoxRepository>();
builder.Services.AddScoped<IGenericRepository<BoxSectionRelationship>, BoxSectionRelationshipRepository>();
builder.Services.AddScoped<IGenericReadRepository<BoxSectionRelationship>, BoxSectionRelationshipRepository>();
builder.Services.AddScoped<IGenericRepository<SectionItemRelationship>, SectionItemRelationshipRepository>();
builder.Services.AddScoped<IGenericReadRepository<SectionItemRelationship>, SectionItemRelationshipRepository>();
builder.Services.AddScoped<IGenericRepository<ItemSpecRelationship>, ItemSpecRelationshipRepository>();
builder.Services.AddScoped<IGenericReadRepository<ItemSpecRelationship>, ItemSpecRelationshipRepository>();

// Services init
builder.Services.AddScoped<IGenericServiceWithCascade<Box, BoxResponse>, BoxService>();
builder.Services.AddScoped<IGenericReadService<Box, BoxResponse>, BoxService>();

builder.Services.AddScoped<IGenericService<BoxSectionRelationship, BoxSectionRelationshipResponse>, BoxSectionRelationshipService>();
builder.Services.AddScoped<IGenericReadService<BoxSectionRelationship, BoxSectionRelationshipResponse>, BoxSectionRelationshipService>();

builder.Services.AddScoped<IGenericService<SectionItemRelationship, SectionItemRelationshipResponse>, SectionItemRelationshipService>();
builder.Services.AddScoped<IGenericReadService<SectionItemRelationship, SectionItemRelationshipResponse>, SectionItemRelationshipService>();

builder.Services.AddScoped<IGenericService<ItemSpecRelationship, ItemSpecRelationshipResponse>, ItemSpecRelationshipService>();
builder.Services.AddScoped<IGenericReadService<ItemSpecRelationship, ItemSpecRelationshipResponse>, ItemSpecRelationshipService>();

builder.Services.AddScoped<IUnitOfWork<AppDbContext>, UnitOfWork<AppDbContext>>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpClient();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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

app.UseAuthorization();

app.MapControllers();

app.Run();
