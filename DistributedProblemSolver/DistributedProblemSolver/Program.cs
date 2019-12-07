using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DistributedDto;
using MassTransit;
using Shared;

namespace DistributedProblemSolver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter Secret to Solve:");
            byte[] secretHash = null;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                while (secretHash == null)
                {
                    var input = Console.ReadLine();
                    if (int.TryParse(input, out int secretNumber))
                    {
                        if (secretNumber < 0)
                        {
                            secretHash = sha256Hash.ComputeHash(Encoding.ASCII.GetBytes(9999999999.ToString()));
                        }
                        else
                        {
                            secretHash = sha256Hash.ComputeHash(Encoding.ASCII.GetBytes(input.ToString()));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Please enter a valid number");
                    }
                }
            }

            Console.WriteLine("Calculating Secret");
            var stopWatch = Stopwatch.StartNew();
            var solvedSecretNumber = await SolveProblem(secretHash);
            stopWatch.Stop();
            Console.WriteLine($"Your Secret Is: {solvedSecretNumber}");
            Console.WriteLine($"Solved In: {stopWatch.ElapsedMilliseconds}ms");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static async Task<int> SolveProblem(byte[] secretHash)
        {
            var request = new SecretProblem()
            {
                SecretHash = secretHash,
            };

            var bus = await StaticBus.Get();
            var secret = (await bus.Request<ISecretProblem, ISecretResult>(request)).Message.Secret.Value;
            await StaticBus.StopAsync();

            return secret;
        }
    }
}
