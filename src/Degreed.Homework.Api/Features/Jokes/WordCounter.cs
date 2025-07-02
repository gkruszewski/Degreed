namespace Degreed.Homework.Api.Features.Jokes;

internal ref struct WordCounter
{
    private readonly ReadOnlySpan<char> _text;
    private int _count;

    public WordCounter(ReadOnlySpan<char> text)
    {
        _text = text;
        _count = text.Length > 0 ? 1 : 0;
    }

    public bool TryIncrement(int index, out int count)
    {
        count = _count;

        if (index > 0 && index < _text.Length && char.IsWhiteSpace(_text[index - 1]))
        {
            var value = _text[index];

            if (char.IsLetterOrDigit(value) || char.IsPunctuation(value))
            {
                count = _count++;

                return true;
            }
        }

        return false;
    }
}
