using System.Threading;
using System.Threading.Tasks;
using DistributedDto;
using MassTransit;
using Shared;

namespace DistributedProblemSolver
{
    class ProblemSolverOrchestrator
    {
        const int batchSize = 1000000;
        int index = 0;
        readonly SemaphoreSlim indexLock = new SemaphoreSlim(0, 1);

        public async Task<int> SolveProblem(byte[] secretHash)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var problemSolvers = new Task<int?>[]
            {
                RunProblemSolver(secretHash, token),
                RunProblemSolver(secretHash, token),
                RunProblemSolver(secretHash, token),
                RunProblemSolver(secretHash, token),
            };

            var secret = await(await Task.WhenAny(problemSolvers));
            tokenSource.Cancel();
            return secret.Value;
        }

        async Task<int?> RunProblemSolver(byte[] secretHash, CancellationToken ct)
        {
            var bus = await StaticBus.Get();
            while (!ct.IsCancellationRequested)
            {
                var start = await GetStartIndex();
                var request = new SecretProblem()
                {
                    StartRange = start,
                    EndRange = start + batchSize - 1,
                    SecretHash = secretHash,
                };
                var result = (await bus.Request<ISecretProblem, ISecretResult>(request)).Message;
                if (result.Solved)
                {
                    return result.Secret;
                }
            }

            return null;
        }

        async Task<int> GetStartIndex()
        {
            await indexLock.WaitAsync();
            var start = index;
            index += batchSize;
            indexLock.Release();
            return start;
        }
    }
}
