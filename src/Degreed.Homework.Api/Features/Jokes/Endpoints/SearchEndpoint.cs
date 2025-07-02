using Degreed.Homework.Api.Features.Jokes.Contracts;
using System.Net.Mime;
using System.Text;

namespace Degreed.Homework.Api.Features.Jokes.Endpoints;

internal sealed partial class SearchEndpoint
    : EndpointWithoutRequest
{
    private readonly DadJokeHttpClient _dadJokeHttpClient;

    private enum Size
    {
        Small,
        Medium,
        Large
    }

    public SearchEndpoint(DadJokeHttpClient dadJokeHttpClient)
    {
        _dadJokeHttpClient = dadJokeHttpClient;
    }

    public override void Configure()
    {
        Get("/joke/search");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var term = Query<string>(nameof(JokePaginationResponse.Term), false);
        var resultResponse = await _dadJokeHttpClient.Search(term: term, cancellationToken: cancellationToken);
        var response = resultResponse.Result.Match(response => GroupAndEmphasize(response.Results, term), exception => exception.Message);

        await SendStringAsync(response, resultResponse.StatusCode, MediaTypeNames.Text.Html, cancellationToken);
    }

    private string GroupAndEmphasize(IEnumerable<JokeResponse> jokes, string term)
    {
        var messages = new Dictionary<Size, List<string>>();
        var emphasizer = new Emphasizer(term, Emphasize.AngleBrackets);

        foreach (var joke in jokes.Select(jokeResponse => jokeResponse.Joke))
        {
            var (size, characters) = GroupAndEmphasize(joke, emphasizer);
            var item = new string([.. characters]);

            if (messages.TryGetValue(size, out var values))
            {
                values.Add(item);
            }
            else
            {
                messages.Add(size, [item]);
            }
        }

        return Format(messages);
    }

    private (Size Size, IEnumerable<char> Characters) GroupAndEmphasize(ReadOnlySpan<char> joke, Emphasizer emphasizer)
    {
        var characters = new List<char>();
        var wordCount = joke.Length > 0 ? 1 : 0;

        for (var i = 0; i < joke.Length; i++)
        {
            var value = joke[i];

            if (i > 0 && char.IsWhiteSpace(joke[i - 1]) && (char.IsLetterOrDigit(value) || char.IsPunctuation(value)))
            {
                wordCount++;
            }

            if (emphasizer.TryMatch(joke, i, StringComparison.OrdinalIgnoreCase, out var emphasizeTerm))
            {
                i += emphasizer.DecrementLength(1);
                characters.AddRange(emphasizeTerm);
            }
            else
            {
                characters.Add(value);
            }
        }

        return wordCount switch
        {
            < 10 => (Size.Small, characters),
            < 20 => (Size.Medium, characters),
            _ => (Size.Large, characters)
        };
    }

    private string Format(IDictionary<Size, List<string>> messages)
    {
        var builder = new StringBuilder();

        builder.Append("<p>");

        foreach (var message in messages.OrderBy(selector => selector.Key))
        {
            builder.Append($"<dt><b>{message.Key}</b></dt>");
            builder.Append($"<dd>{string.Join("</dd><dd>", message.Value)}</dd>");
        }

        builder.Append("</p>");

        return builder.ToString();
    }
}
