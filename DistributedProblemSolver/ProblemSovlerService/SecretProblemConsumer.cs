using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DistributedDto;
using MassTransit;

namespace ProblemSovlerService
{
    class SecretProblemConsumer : IConsumer<ISecretProblem>
    {
        public Task Consume(ConsumeContext<ISecretProblem> context)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                for (int i = context.Message.StartRange; i <= context.Message.EndRange; i++)
                {
                    var hash = sha256Hash.ComputeHash(Encoding.ASCII.GetBytes(i.ToString()));
                    if (context.Message.SecretHash.SequenceEqual(hash))
                    {
                        context.Respond<ISecretResult>(new SecretResponse()
                        {
                            Solved = true,
                            Secret = i,
                        });
                        return Task.CompletedTask;
                    }
                }
            }

            context.Respond<ISecretResult>(new SecretResponse());
            return Task.CompletedTask;
        }
    }
}
