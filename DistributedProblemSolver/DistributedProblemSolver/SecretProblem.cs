using DistributedDto;

namespace DistributedProblemSolver
{
    class SecretProblem : ISecretProblem
    {
        public int StartRange { get; set; }
        public int EndRange { get; set; }
        public byte[] SecretHash { get; set; }
    }
}
