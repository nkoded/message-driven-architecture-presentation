using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace Shared
{
    public static class StaticBus
    {
        static IBusControl bus;
        static readonly SemaphoreSlim busLock = new SemaphoreSlim(1, 1);

        public static async Task<IBus> Get()
        {
            if (bus == null)
            {
                await busLock.WaitAsync();
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
                busLock.Release();
            }
            return bus;
        }

        public static async Task StopAsync()
        {
            await bus.StopAsync();
        }
    }
}
