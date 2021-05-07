namespace Gitshurp
{
    public class RepoModel
    {
        public string Name { get; set; }
        public string AuthorUsername { get; set; }
        public string Description { get; set; }
        public string Stars { get; set; }
        public string Forks { get; set; }
        public string Language { get; set; }
        public bool IsFork { get; set; }
        public override string ToString()
        {
            return $"{Name}({Description})";
        }
    }
}