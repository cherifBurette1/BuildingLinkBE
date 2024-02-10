using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Executing handler for {typeof(TRequest).Name}");

            _logger.LogInformation($"Input: {JsonConvert.SerializeObject(request)}");

            var response = await next();

            _logger.LogInformation($"Output: {JsonConvert.SerializeObject(response)}");

            _logger.LogInformation($"Handler execution completed for {typeof(TRequest).Name}");

            return response;
        }
    }
}
