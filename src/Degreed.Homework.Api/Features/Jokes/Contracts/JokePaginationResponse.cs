namespace Degreed.Homework.Api.Features.Jokes.Contracts;

internal class JokePaginationResponse
{
    public int CurrentPage { get; set; }

    public int Limit { get; set; }

    public int NextPage { get; set; }

    public int PreviousPage { get; set; }

    public IEnumerable<JokeResponse> Results { get; set; }

    public string Term { get; set; }

    public int TotalJokes { get; set; }

    public int TotalPages { get; set; }
}
