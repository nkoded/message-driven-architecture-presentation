namespace DistributedDto
{
    public interface ISecretProblem
    {
        int StartRange { get; }
        int EndRange { get; }
        byte[] SecretHash { get; }
    }
}
