using GithubLib.Gists;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GithubLib.Test
{
    class DatabaseByGist_Test
    {
        GithubConnectInfo info = new GithubConnectInfo
        {
            DatabaseName = "Test123",
            ID= "",
            Key = "tiephp",
            ///https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token
            Token = "ghp_YrRu56vRHygcaBg11DRUhOhj8pOOw34g8ee" //token here. 
        };

        [Test]
        public async Task Write()
        {
            var x = await new DatabaseByGist(info).Write(info);
            var y = await new DatabaseByGist(info).Read<GithubConnectInfo>();
            Assert.IsNotNull(x);
            Assert.AreEqual(y, info);
        }
    }
}
