using Autofac;

namespace SomeJob
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessQueueMessage("direction", "integration", "batchId", "instanceId");
        }

        static void ProcessQueueMessage(string direction, string integration, string batchId, string instanceId)
        {
            //var direction = string.IsNullOrWhiteSpace(orchestrationData.DistributorInterfaceName) ? "inbound" : "outbound";
            //var integrationType = string.IsNullOrWhiteSpace(orchestrationData.DistributorInterfaceName) ? orchestrationData.ProviderInterfaceName : orchestrationData.DistributorInterfaceName;
            //var integration = integrationType.Split('.').Last().ToLower();

            var container = Configuration.Configure(direction, integration, batchId, instanceId);

                var app = container.Resolve<ISomeProcess>();
                app.Run();

        }
    }
}
