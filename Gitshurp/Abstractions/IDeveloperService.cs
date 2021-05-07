using System.Collections.Generic;

namespace Gitshurp
{
    public interface IDeveloperService
    {
        IEnumerable<DeveloperModel> Trending();
        DeveloperModel Detail(string username);
        IEnumerable<DeveloperModel> Search(string keyword);
    }
}