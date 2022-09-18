using System.Diagnostics.CodeAnalysis;
using Rinkudesu.Gateways.MessageQueues.Messages;
using Rinkudesu.Kafka.Dotnet.Base;

namespace Rinkudesu.Gateways.MessageQueues;

[ExcludeFromCodeCoverage]
public static class KafkaProducerExtensions
{
    public static async Task ProduceUserDeleted(this IKafkaProducer producer, Guid userId, CancellationToken cancellationToken = default)
        => await producer.Produce(Constants.TOPIC_USER_DELETED, new UserDeletedMessage(userId), cancellationToken).ConfigureAwait(false);
}
