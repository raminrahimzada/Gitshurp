using System.Collections.Generic;

namespace Gitshurp
{
    public interface IRepoService
    {
        IEnumerable<RepoModel> Explore();
    }
}