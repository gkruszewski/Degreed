namespace Degreed.Homework.Api.Features.Jokes.Highlighters;

internal sealed class UppercaseHighlighter
    : IHighlight
{
    public ReadOnlySpan<char> Format(ReadOnlySpan<char> value)
    {
        static ReadOnlySpan<char> ToUpper(ReadOnlySpan<char> term)
        {
            Span<char> destination = stackalloc char[term.Length];

            term.ToUpperInvariant(destination);

            return new string(destination);
        }

        return ToUpper(value);
    }
}
