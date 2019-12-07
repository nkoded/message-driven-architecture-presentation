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
                StopBus();
                
                ev.Cancel = true;
                Environment.Exit(0);
            };

            await AddConsumer(10);
            await AddConsumer(9);

            Console.WriteLine("Ready to solve problems!");
            Console.ReadKey();
            StopBus();
        }

        static async Task AddConsumer(int priority)
        {
            Console.WriteLine($"Connecting Consumer {priority}");
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
                    e.ConsumerPriority = priority;
                    e.Consumer<SecretProblemConsumer>();
                });
            });
            await bus.StartAsync();
        }

        static void StopBus()
        {
            if (bus != null)
            {
                Console.WriteLine("Stopping Bus");
                bus.Stop();
                Console.WriteLine("Bus Stopped");
            }
        }

    }
}
