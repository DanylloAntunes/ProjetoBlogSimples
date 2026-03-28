using ErrorOr;

namespace BlogSimples.Api.Configuracao;

public static class ResultExtensions
{
    public static IResult ToResult<T>(this ErrorOr<T> result)
    {
        return result.Match(
            value => Results.Ok(value),
            errors => ToProblem(errors)
        );
    }

    private static IResult ToProblem(List<Error> errors)
    {
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var validationErrors = errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray()
                );

            return Results.ValidationProblem(validationErrors);
        }

        var firstError = errors.First();

        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => 404,
            ErrorType.Conflict => 409,
            ErrorType.Unauthorized => 401,
            _ => 500
        };

        return Results.Problem(
            statusCode: statusCode,
            title: firstError.Description
        );
    }
}
