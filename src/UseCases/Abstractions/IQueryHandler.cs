namespace UseCases.Abstractions;

public interface IQueryHandler<TQuery, TResponse>
    where TQuery : struct
    where TResponse : class
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default);
}

public readonly record struct NoQuery
{
    public static NoQuery Instance { get; } = new NoQuery();
}
