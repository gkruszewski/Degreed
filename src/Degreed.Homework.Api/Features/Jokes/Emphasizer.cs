namespace Degreed.Homework.Api.Features.Jokes;

internal readonly ref struct Emphasizer
{
    private readonly ReadOnlySpan<char> _term;
    private readonly IHighlight _highlight;

    public Emphasizer(ReadOnlySpan<char> term, IHighlight highlight)
    {
        _term = term;
        _highlight = highlight;
    }

    public bool TryMatch(ReadOnlySpan<char> text, int index, StringComparison stringComparison, out ReadOnlySpan<char> emphasizedTerm)
    {
        emphasizedTerm = null;

        if (_term.Length > 0 && index + _term.Length <= text.Length)
        {
            var part = text.Slice(index, _term.Length);

            if (part.Equals(_term, stringComparison))
            {
                emphasizedTerm = _highlight.Format(part);

                return true;
            }
        }

        return false;
    }

    public int MoveIndexForward()
    {
        return _term.Length - 1;
    }
}
