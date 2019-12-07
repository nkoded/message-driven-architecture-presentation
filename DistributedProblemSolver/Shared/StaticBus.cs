using System.Threading.Tasks;
using MassTransit;

namespace Shared
{
    public static class StaticBus
    {
        static IBusControl bus;

        public static IBus Get()
        {
            if (bus == null)
            {
                bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Durable = false;
                    cfg.AutoDelete = true;
                    cfg.Host(BusConfig.Host);
                });
            }
            return bus;
        }

        public static async Task Stop()
        {
            await bus.StopAsync();
        }
    }
}
