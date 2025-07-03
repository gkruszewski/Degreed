namespace Degreed.Homework.Api.Features.Jokes.Highlighters;

internal sealed class QuoteHighlighter
    : IHighlight
{
    public ReadOnlySpan<char> Format(ReadOnlySpan<char> value) => $"'{value}'";
}
