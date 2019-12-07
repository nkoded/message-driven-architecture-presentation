namespace DistributedDto
{
    public interface ISecretResult
    {
        bool Solved { get; }
        int? Secret { get; }
    }
}
