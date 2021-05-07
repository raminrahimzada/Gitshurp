namespace Gitshurp
{
    public class GithubClient: IGithubClient
    {
        public IDeveloperService Developer { get; }
        public IRepoService Repository { get; }

        public GithubClient()
        {
            IGithubHttpClient httpClient = new GithubHttpClient();
            Developer=new DeveloperService(httpClient);
            Repository = new RepoService(httpClient);
        }
    }
}