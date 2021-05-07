using System.Collections.Generic;

namespace Gitshurp
{
    public class DeveloperModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string Twitter { get; set; }
        public List<RepoModel> PopularRepos { get; set; }

        public override string ToString()
        {
            return $"{FullName} (@{Username})";
        }
    }
}