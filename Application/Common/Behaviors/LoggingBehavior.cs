using MediatR;

namespace Application.Common.Behaviors;

/// <summary>
/// Logging behavior for MediatR pipeline
/// </summary>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        try
        {
            var response = await next();
            return response;
        }
        catch
        {
            throw;
        }
    }
}
