using Degreed.Homework.Api.Features.Jokes.Contracts;
using System.Net.Mime;
using System.Text;

namespace Degreed.Homework.Api.Features.Jokes.Endpoints;

internal sealed class SearchEndpoint
    : EndpointWithoutRequest
{
    private readonly DadJokeHttpClient _dadJokeHttpClient;

    private enum Emphasize
    {
        None,
        AngleBrackets,
        Quotes,
        Uppercase
    }

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
        var response = resultResponse.Result.Match(response => Format(response.Results, term, Emphasize.AngleBrackets), exception => exception.Message);

        await SendStringAsync(response, resultResponse.StatusCode, MediaTypeNames.Text.Html, cancellationToken);
    }

    private string Format(IEnumerable<JokeResponse> jokes, string term, Emphasize emphasize)
    {
        var messages = new Dictionary<Size, List<string>>();

        foreach (var joke in jokes.Select(jokeResponse => jokeResponse.Joke))
        {
            var (size, characters) = Format(joke, term, emphasize);
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

    private (Size Size, IEnumerable<char> Characters) Format(ReadOnlySpan<char> joke, ReadOnlySpan<char> term, Emphasize emphasize)
    {
        var characters = new List<char>();
        var wordCount = joke.Length > 0 ? 1 : 0;

        for (var i = 0; i < joke.Length; i++)
        {
            var value = joke[i];
            var termMatch = term.Length > 0 && i + term.Length <= joke.Length && joke
                .Slice(i, term.Length)
                .Equals(term, StringComparison.OrdinalIgnoreCase);

            if (i > 0 && char.IsWhiteSpace(joke[i - 1]) && (char.IsLetterOrDigit(value) || char.IsPunctuation(value)))
            {
                wordCount++;
            }

            if (termMatch)
            {
                i += term.Length - 1;
                characters.AddRange(EmphasizeTerm(term, emphasize));
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

    private ReadOnlySpan<char> EmphasizeTerm(ReadOnlySpan<char> term, Emphasize emphasize)
    {
        static ReadOnlySpan<char> ToUpper(ReadOnlySpan<char> term)
        {
            Span<char> destination = stackalloc char[term.Length];

            term.ToUpperInvariant(destination);

            return new string(destination);
        }

        return emphasize switch
        {
            Emphasize.AngleBrackets => $"&lt;{term}&gt;",
            Emphasize.Quotes => $"'{term}'",
            Emphasize.Uppercase => ToUpper(term),
            _ => term
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
