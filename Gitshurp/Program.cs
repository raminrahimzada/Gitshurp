using System;
using System.Linq;

namespace Gitshurp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client=new GithubClient();
            
            //search for developers
            var developerSearch = client.Developer.Search("mdisec").ToArray();
            
            //explore famous repos
            var repos = client.Repository.Explore().ToArray();
            
            //explore trending developers
            var developers = client.Developer.Trending().ToArray();

            foreach (var dev in developers)
            {
                Console.WriteLine("==========================================================");
                var developer = client.Developer.Detail(dev.Username);
                Console.WriteLine("developer info  " + developer);
                Console.WriteLine("famous repos : ");
                foreach (var repo in developer.PopularRepos)
                {
                    Console.WriteLine(repo);
                }
            }
        }
    }
}
