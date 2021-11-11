using GithubLib.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GithubLib.Gists
{
    /// <summary>
    /// https://docs.github.com/en/rest/reference/gists
    /// </summary>
    public class GistHelper
    {
        protected string _token;

        public GistHelper(string token)
        {
            this._token = token;
        }

        HttpClient _http()
        {
            var http = new HttpClient();
            http.BaseAddress = new Uri("https://api.github.com/");
            http.DefaultRequestHeaders.Add("Authorization", $"token {_token}");
            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36");
            return http;
        }

        public async Task<string> GetAll()
        {
            using (var h = _http())
            {
                return await h.GetStringAsync("gists");
            }
        }

        public async Task<string> GetByUser(string username)
        {
            using (var h = _http())
            {
                return await h.GetStringAsync($"users/{username}/gists");
            }
        }

        public static async Task<string> Get(string requestUri)
        {
            using (var http = new HttpClient())
            {
                return await http.GetStringAsync(requestUri);
            }
        }

        public async Task<string> CreateNew(string filename, string content)
        {
            using (var h = _http())
            {
                var r = await h.PostAsJsonAsync("gists", new
                {
                    files = new
                    {
                        file = new
                        {
                            filename = filename,
                            content = content
                        }
                    }
                });
                var text = await r.Content.ReadAsStringAsync();
                var o = JsonConvert.DeserializeAnonymousType(text, new { id = "" });
                return o.id;
            }
        }

        public async Task<string> Update(string gist_id, string filename, string content)
        {
            using (var h = _http())
            {
                var r = await h.PostAsJsonAsync($"gists/{gist_id}", new
                {
                    files = new
                    {
                        file = new
                        {
                            filename = filename,
                            content = content
                        }
                    }
                });
                return await r.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> Delete(string gist_id)
        {
            using (var h = _http())
            {
                var r = await h.DeleteAsync($"gists/{gist_id}");
                return await r.Content.ReadAsStringAsync();
            }
        }


        /// <summary>
        /// {
        ///    "url": "https://api.github.com/gists/aa5a315d61ae9438b18d",
        ///    "forks_url": "https://api.github.com/gists/aa5a315d61ae9438b18d/forks",
        ///    "commits_url": "https://api.github.com/gists/aa5a315d61ae9438b18d/commits",
        ///    "id": "aa5a315d61ae9438b18d",
        ///    "node_id": "MDQ6R2lzdGFhNWEzMTVkNjFhZTk0MzhiMThk",
        ///    "git_pull_url": "https://gist.github.com/aa5a315d61ae9438b18d.git",
        ///    "git_push_url": "https://gist.github.com/aa5a315d61ae9438b18d.git",
        ///    "html_url": "https://gist.github.com/aa5a315d61ae9438b18d",
        ///    "created_at": "2010-04-14T02:15:15Z",
        ///    "updated_at": "2011-06-20T11:34:15Z",
        ///    "description": "Hello World Examples",
        ///    "comments": 0,
        ///    "comments_url": "https://api.github.com/gists/aa5a315d61ae9438b18d/comments/"
        ///}
        /// </summary>
        /// <param name="gist_id"></param>
        /// <returns></returns>
        public async Task<string> GetById(string gist_id)
        {
            using (var h = _http())
            {
                return await h.GetStringAsync($"gists/{gist_id}");
            }
        }
    }
}
