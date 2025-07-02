using Degreed.Homework.Api.Features.Jokes.Enums;

namespace Degreed.Homework.Api.Features.Jokes;

internal readonly ref struct Emphasizer
{
    private readonly ReadOnlySpan<char> _term;
    private readonly Emphasize _emphisize;

    public Emphasizer(ReadOnlySpan<char> term, Emphasize emphasize)
    {
        _term = term;
        _emphisize = emphasize;
    }

    public bool TryMatch(ReadOnlySpan<char> value, int index, StringComparison stringComparison, out ReadOnlySpan<char> emphasizedTerm)
    {
        emphasizedTerm = null;

        if (_term.Length > 0 && index + _term.Length <= value.Length)
        {
            var part = value.Slice(index, _term.Length);

            if (part.Equals(_term, stringComparison))
            {
                emphasizedTerm = _emphisize switch
                {
                    Emphasize.AngleBrackets => $"&lt;{part}&gt;",
                    Emphasize.Quotes => $"'{part}'",
                    Emphasize.Uppercase => ToUpper(part),
                    _ => part
                };

                return true;
            }
        }

        return false;
    }

    public int MoveIndexForward()
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
