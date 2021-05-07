using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using HtmlAgilityPack;

namespace Gitshurp
{
    public class DeveloperService : IDeveloperService
    {
        private readonly IGithubHttpClient _httpClient;

        public DeveloperService(IGithubHttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IEnumerable<DeveloperModel> Trending()
        {
            var html = _httpClient.Get("https://github.com/trending/developers");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var main = doc.DocumentNode.SelectSingleNode("//main");
            var trendingArticles = main.SelectNodes("//article").ToArray();
            var text = trendingArticles.Select(x => x.Descendants().FirstOrDefault(y => y.Name == "p"))
                .Select(x => x?.InnerText?.Trim()?.Trim('\n'))
                .Where(x => x != null)
                .ToArray();

            foreach (var s in text)
            {
                DeveloperModel d = new DeveloperModel();
                d.Username = s;
                yield return d;
            }
        }

        public DeveloperModel Detail(string username)
        {
            var url = $"https://github.com/{username}/";
            var html = _httpClient.Get(url);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
            DeveloperModel model = new DeveloperModel();
            model.Username = username;

            model.FullName = doc.DocumentNode.FindByProp("itemprop", "name");
            model.Twitter = doc.DocumentNode.FindByProp("itemprop", "twitter");

            var bioDiv = doc.DocumentNode.Descendants().FirstOrDefault(x => x.HasClass("user-profile-bio"));
            if (bioDiv != null)
            {
                model.Bio = bioDiv.InnerText.Trim();
            }

            model.PopularRepos = new List<RepoModel>();

            var ol = doc.DocumentNode.Descendants().FirstOrDefault(x => x.Name == "ol");
            if (ol != null)
            {
                var liArr = ol.Descendants().Where(x => x.Name == "li").ToArray();
                foreach (var li in liArr)
                {
                    var nameSpan = li.Descendants().FirstOrDefault(x => x.HasClass("repo"));
                    var txtArr = li.Descendants().Where(x => x.Name == "p").Select(x => x.InnerText).ToArray();
                    //TODO parse gists here
                    if (txtArr.Length == 0) continue;
                    var repoDescription = txtArr[0].Trim();
                    var split = txtArr.Last().Split('\n').Select(x => x.Trim()).Where(x=>!string.IsNullOrWhiteSpace(x)).ToArray();
                    if (txtArr.Length == 3)
                    {
                        repoDescription = txtArr[1];
                    }
                    string stars = null, forks = null;
                    bool isFork = false;
                    if (split.Length != 0)
                    {
                        stars = split[0];
                        forks = split.Last();
                        isFork = true;
                    }
                    
                    var language = li.FindByProp("itemprop", "programmingLanguage");
                    model.PopularRepos.Add(new RepoModel
                    {
                        Name = nameSpan?.InnerText,
                        AuthorUsername = username,
                        Description=repoDescription,
                        Stars=stars,
                        Forks=forks,
                        IsFork=isFork,
                        Language= language
                    });
                }
            }

            ;
            return model;
        }

        public IEnumerable<DeveloperModel> Search(string keyword)
        {
            var url = $@"https://github.com/search?q={keyword}&type=users";
            var html = _httpClient.Get(url);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var count = doc.DocumentNode.FindByProp("data-search-type", "Users");
            var resultsDiv = doc.DocumentNode.Descendants().FirstOrDefault(x => x.Id == "user_search_results");
            if (resultsDiv != null)
            {
                var list = resultsDiv.Descendants().Where(x => x.HasClass("flex-auto")).ToArray();
                foreach (var devNode in list)
                {
                    var fullName = devNode.Descendants().FirstOrDefault(x => x.Name == "p")?.InnerText?.Trim();
                    var username = devNode.Descendants().FirstOrDefault(x => x.Name == "em")?.InnerText?.Trim();
                    if (string.IsNullOrWhiteSpace(username))
                    {
                        continue;
                    }
                    ;
                    yield return new DeveloperModel()
                    {
                        FullName = fullName,
                        Username = username
                    };
                }
            }

            ;
            yield break;
        }
    }
}