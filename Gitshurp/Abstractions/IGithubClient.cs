namespace Gitshurp
{
    public interface IGithubClient
    {
        IDeveloperService Developer { get; }
        IRepoService Repository { get; }
    }
}