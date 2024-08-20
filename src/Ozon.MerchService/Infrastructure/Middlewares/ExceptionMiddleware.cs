using System.Net;
using System.Text.Json;
using Confluent.Kafka;
using FluentValidation;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;

namespace Ozon.MerchService.Infrastructure.Middlewares;

    /// <summary>
    /// Middleware to handle exceptions
    /// </summary>
    /// <param name="next">Next middleware in pipeline</param>
    /// <param name="logger">Logger instance</param>
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        /// <summary>
        /// Handle different types of application exceptions
        /// </summary>
        /// <param name="context">Http context of request</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                await HandleException(context, operationCanceledException.Message, (int)HttpStatusCode.NoContent);
            }
            catch (ValidationException validationException)
            {
                await HandleException(context, validationException.Message, (int)HttpStatusCode.BadRequest);
            }
            catch (RepositoryOperationException repositoryOperationException)
            {
                await HandleException(context, repositoryOperationException.Message,
                    (int)HttpStatusCode.InternalServerError);
            }
            catch (ConsumeException consumeException)
            {
                if (consumeException.Error.Code == ErrorCode.UnknownTopicOrPart)
                {
                    await HandleException(context, $"Consume topic error (reason - {consumeException.Error.Reason}):" + consumeException.Message, (int)HttpStatusCode.ServiceUnavailable);
                }
                else
                {
                    await HandleException(context, $"Consume error (reason - {consumeException.Error.Reason}):" + consumeException.Message, (int)HttpStatusCode.ServiceUnavailable);
                }
            }
            catch (BrokerException brokerException)
            {
                await HandleException(context, $"Produce error :" + brokerException.Message, (int)HttpStatusCode.ServiceUnavailable);
            }
            catch (KafkaException kafkaException)
            {
                await HandleException(context, $"Kafka error (reason - {kafkaException.Error.Reason}):" + kafkaException.Message, (int)HttpStatusCode.ServiceUnavailable);
            }
            catch (Exception exception)
            {
                await HandleException(context, exception.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleException(
            HttpContext context, 
            string exceptionMessage, 
            int statusCode)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = statusCode;

            logger.LogError("Status code: {statusCode} - exception message: {exceptionMessage}", statusCode, exceptionMessage);
            
            var result = JsonSerializer.Serialize(new { message = exceptionMessage });

            await response.WriteAsync(result);
        }
    }