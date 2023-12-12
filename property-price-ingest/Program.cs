﻿using Microsoft.Extensions.Options;
using property_price_api.Data;
using property_price_ingest;
using property_price_ingest.Services;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using property_price_ingest.Models;
using property_price_api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<PropertyPriceApiDatabaseSettings>(
    builder.Configuration.GetSection("PropertyPriceApiDatabase"));
builder.Services.AddSingleton(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<PropertyPriceApiDatabaseSettings>>().Value;
    return new MongoDbContext(
        Environment.GetEnvironmentVariable("MONGO_DB_CONNECTION_STRING") ??
        settings.ConnectionString,
        settings.DatabaseName);
});

builder.Services.Configure<CloudPubSubConsumerOptions>(
    builder.Configuration.GetSection(CloudPubSubConsumerOptions.CloudPubSub));
builder.Services.AddHostedService<IngestWorker>();
builder.Services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
builder.Services.AddScoped<ICloudPubSubMessagePullService, CloudPubSubMessagePullService>();

// Configure HTTP client
builder.Services.AddHttpClient(HttpClientConstants.jsonPlaceholderHttpClientName, httpClient =>
{
    httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/json");
});

var app = builder.Build();
app.MapGet("/", () => "up!");

app.Run();