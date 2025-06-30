using Degreed.Homework.Api.Features.Jokes.Contracts;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Json;

namespace Degreed.Homework.Api.Features.Jokes;

internal class DadJokeHttpClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public DadJokeHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResultResponse<JokeResponse>> Random(CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync(string.Empty, cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return new()
        {
            StatusCode = (int)response.StatusCode,
            Result = response.IsSuccessStatusCode ? JsonSerializer.Deserialize<JokeResponse>(content, JsonSerializerOptions) : new Result<JokeResponse>(new InvalidOperationException(content))
        };
    }

    public async Task<ResultResponse<JokePaginationResponse>> Search(int page, int limit, string term, CancellationToken cancellationToken)
    {
        var queryBuilder = new QueryBuilder
        {
            { nameof(page), page.ToString() },
            { nameof(limit), limit.ToString() },
            { nameof(term), term }
        };

        using var response = await _httpClient.GetAsync("search" + queryBuilder, cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return new()
        {
            StatusCode = (int)response.StatusCode,
            Result = response.IsSuccessStatusCode ? JsonSerializer.Deserialize<JokePaginationResponse>(content, JsonSerializerOptions) : new Result<JokePaginationResponse>(new InvalidOperationException(content))
        };
    }
}
