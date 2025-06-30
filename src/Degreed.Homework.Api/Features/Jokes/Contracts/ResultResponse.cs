using LanguageExt.Common;

namespace Degreed.Homework.Api.Features.Jokes.Contracts;

internal class ResultResponse<T> where T: class
{
    public required int StatusCode { get; init; }

    public required Result<T> Result { get; init; }
}
