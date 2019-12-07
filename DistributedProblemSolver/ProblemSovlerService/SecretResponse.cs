using DistributedDto;

namespace ProblemSovlerService
{
    class SecretResponse : ISecretResult
    {
        public int? Secret { get; set; }
    }
}
