using DistributedDto;

namespace DistributedProblemSolver
{
    class SecretProblem : ISecretProblem
    {
        public byte[] SecretHash { get; set; }
    }
}
