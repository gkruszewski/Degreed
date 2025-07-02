namespace Degreed.Homework.Api.Features.Jokes;

internal readonly ref struct Emphasizer
{
    private readonly ReadOnlySpan<char> _term;
    private readonly ReadOnlySpan<char> _emphisizedTerm;

    public Emphasizer(ReadOnlySpan<char> term, Emphasize emphasize)
    {
        _term = term;
        _emphisizedTerm = emphasize switch
        {
            Emphasize.AngleBrackets => $"&lt;{term}&gt;",
            Emphasize.Quotes => $"'{term}'",
            Emphasize.Uppercase => ToUpper(term),
            _ => term
        };
    }

    public bool TryMatch(ReadOnlySpan<char> value, int index, StringComparison stringComparison, out ReadOnlySpan<char> emphasizedTerm)
    {
        var isMatch = _term.Length > 0 && index + _term.Length <= value.Length && value
            .Slice(index, _term.Length)
            .Equals(_term, stringComparison);

        emphasizedTerm = null;

        if (isMatch)
        {
            emphasizedTerm = _emphisizedTerm;

            return true;
        }

        return false;
    }

    public int DecrementLength()
    {
        return _term.Length - 1;
    }

    private ReadOnlySpan<char> ToUpper(ReadOnlySpan<char> term)
    {
        Span<char> destination = stackalloc char[term.Length];

        term.ToUpperInvariant(destination);

        return new string(destination);
    }
}
