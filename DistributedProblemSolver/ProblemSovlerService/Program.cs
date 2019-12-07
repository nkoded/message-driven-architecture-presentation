using System;
using System.Threading.Tasks;
using MassTransit;
using Shared;

namespace ProblemSovlerService
{
    class Program
    {
        static IBusControl bus;

        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += (s, ev) => {
                if(bus != null)
                {
                    Console.WriteLine("Stopping Bus");
                    bus.Stop();
                    Console.WriteLine("Bus Stopped");
                }
                
                ev.Cancel = true;
                Environment.Exit(0);
            };

            Console.WriteLine("Connecting Consumer");
            bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Durable = false;
                cfg.AutoDelete = true;
                cfg.Host(BusConfig.Host);
                cfg.ReceiveEndpoint("ProblemSolver.Secret", e =>
                {
                    e.Durable = false;
                    e.AutoDelete = true;
                    e.PrefetchCount = 1;
                    e.ConsumerPriority = 10;
                    e.Consumer<SecretProblemConsumer>();
                });
            });
            await bus.StartAsync();

            Console.WriteLine("Hello World!");
            Console.ReadKey();

        }
    }
}
