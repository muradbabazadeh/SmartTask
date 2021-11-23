using System.Threading.Tasks;

namespace SmartTask.SharedKernel.Infrastructure.IntegrationEvents
{
    public interface IEventBus
    {
         Task PublishAsync(string topicName, IntegrationEvent integrationEvent);
    }
}
