using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Gitshurp
{
    public class RepoService : IRepoService
    {
        private readonly IGithubHttpClient _httpClient;

        public RepoService(IGithubHttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IEnumerable<RepoModel> Explore()
        {
            var html = _httpClient.Get("https://github.com/explore");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var main = doc.DocumentNode.SelectSingleNode("//main");

            var container = main.Descendants().First(x => x.HasClass("p-responsive"));
            ;
            var articles = container.SelectNodes("//article").ToArray();
            foreach (var article in articles)
            {
                RepoModel repo = new RepoModel();
                {
                    var s = article.SelectSingleNode("//div[@class='px-3']//div[@class='d-flex flex-auto']//h1")
                        ?.InnerText;
                    var arr = s.Split('/');
                    repo.Name = arr[1].Trim('\n').Trim();
                    repo.AuthorUsername = arr[0].Trim('\n').Trim();
                    ;
                }
                yield return repo;
            }
        }
    }
}