namespace Degreed.Homework.Api.Features.Jokes.Highlighters;

internal sealed class NoneHighlighter
    : IHighlight
{
    public ReadOnlySpan<char> Format(ReadOnlySpan<char> value) => value;
}
