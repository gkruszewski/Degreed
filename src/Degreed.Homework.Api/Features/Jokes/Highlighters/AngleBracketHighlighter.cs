namespace Degreed.Homework.Api.Features.Jokes.Highlighters;

internal sealed class AngleBracketHighlighter
    : IHighlight
{
    public ReadOnlySpan<char> Format(ReadOnlySpan<char> value) => $"&lt;{value}&gt;";
}
