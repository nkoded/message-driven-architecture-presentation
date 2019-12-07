using DistributedDto;

namespace ProblemSovlerService
{
    class SecretResponse : ISecretResult
    {
        public bool Solved { get; set; }
        public int? Secret { get; set; }
    }
}
