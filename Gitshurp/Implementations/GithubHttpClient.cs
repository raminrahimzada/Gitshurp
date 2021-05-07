using System;
using System.Net;

namespace Gitshurp
{
    public class GithubHttpClient:IGithubHttpClient
    {
        public string Get(string url)
        {
            var request = WebRequest.Create(new Uri(url));
            request.Method = "GET";
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();
            var buffer = responseStream.ReadToEnd();
            return buffer.GetString();
        }
    }
}