﻿using AutoMapper;
using Microsoft.Extensions.Options;
using property_price_api.Data;
using property_price_api.Models;
using property_price_api.Profiles;
using property_price_api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<PropertyPriceApiDatabaseSettings>(
    builder.Configuration.GetSection("PropertyPriceApiDatabase"));
builder.Services.AddSingleton(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<PropertyPriceApiDatabaseSettings>>().Value;
    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    {
        return new MongoDbContext(settings.ConnectionString, settings.DatabaseName);
    } else
    {
        return new MongoDbContext(Environment.GetEnvironmentVariable("MONGO_DB_CONNECTION_STRING"), settings.DatabaseName);

    }
});

builder.Services.AddSingleton<PropertyService>();
builder.Services.AddSingleton<UserService>();

// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new UserProfile());
    mc.AddProfile(new PropertyProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

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

app.MapGet("/ping", () => "pong!");

app.Run();

