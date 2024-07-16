namespace Ozon.MerchService.Infrastructure.Configuration.MessageBroker;

public class KafkaConfiguration
{
    /// <summary>
    /// Consumer group id
    /// </summary>
    public string GroupId { get; set; }
        
    /// <summary>
    /// Merch service message topic
    /// </summary>
    public string? EmployeeNotificationEventTopic { get; set; }
    public string? StockReplenishedEventTopic { get; set; }
        
    /// <summary>
    /// Kafka servers addresses 
    /// </summary>
    public string BootstrapServers { get; set; }
}