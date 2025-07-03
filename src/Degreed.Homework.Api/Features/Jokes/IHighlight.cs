namespace Degreed.Homework.Api.Features.Jokes;

internal interface IHighlight
{
    ReadOnlySpan<char> Format(ReadOnlySpan<char> value);
}
