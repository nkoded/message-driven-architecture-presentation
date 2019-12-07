using System.Threading.Tasks;
using MassTransit;

namespace Shared
{
    public static class StaticBus
    {
        static IBusControl bus;

        public static async Task<IBus> Get()
        {
            if (bus == null)
            {
                bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Durable = false;
                    cfg.AutoDelete = true;
                    cfg.Host(BusConfig.Host);
                });
                await bus.StartAsync();
            }
            return bus;
        }

        public static async Task StopAsync()
        {
            await bus.StopAsync();
        }
    }
}
