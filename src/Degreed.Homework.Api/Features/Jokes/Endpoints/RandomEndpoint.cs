using System.Net.Mime;

namespace Degreed.Homework.Api.Features.Jokes.Endpoints;

internal sealed class RandomEndpoint
    : EndpointWithoutRequest
{
    private readonly DadJokeHttpClient _dadJokeHttpClient;

    public RandomEndpoint(DadJokeHttpClient dadJokeHttpClient)
    {
        _dadJokeHttpClient = dadJokeHttpClient;
    }

    public override void Configure()
    {
        Get("/joke/random");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var resultResponse = await _dadJokeHttpClient.Random(cancellationToken);
        var response = resultResponse.Result.Match(response => Format(response.Joke), exception => exception.Message);

        await SendStringAsync(response, resultResponse.StatusCode, MediaTypeNames.Text.Html, cancellationToken);
    }

    private string Format(string message)
    {
        return $"""
            <p>
                {message}
            </p>
            """;
    }
}
