global using FastEndpoints;
using Degreed.Homework.Api.Features.Jokes;
using System.Net.Http.Headers;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder();

builder
    .Services
    .AddFastEndpoints()
    .AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins", builder =>
        {
            builder
                .WithOrigins("http://localhost:5078")
                .WithMethods(nameof(HttpMethod.Get))
                .AllowAnyHeader();
        });
    })
    .AddHttpClient<DadJokeHttpClient>(configureClient =>
    {
        configureClient.BaseAddress = new("https://icanhazdadjoke.com/");
        configureClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    })
    .AddStandardResilienceHandler();

var app = builder.Build();

app
    .UseDefaultExceptionHandler()
    .UseFastEndpoints()
    .UseCors("AllowSpecificOrigins");

app.Run();