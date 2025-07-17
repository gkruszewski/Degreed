namespace Degreed.Homework.Api.Features.Jokes.Highlighters;

internal sealed class UppercaseHighlighter
    : IHighlight
{
    public ReadOnlySpan<char> Format(ReadOnlySpan<char> value)
    {
        Span<char> destination = stackalloc char[value.Length];

        value.ToUpperInvariant(destination);

        return new string(destination);
    }
}
