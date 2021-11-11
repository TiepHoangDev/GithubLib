using NUnit.Framework;
using System.Threading.Tasks;

namespace GithubLib.Test
{
    public class Tests
    {
        GithubLib.Gists.GistHelper _gistHelper;

        [SetUp]
        public void Setup()
        {
            this._gistHelper = new Gists.GistHelper("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");
        }

        [Test]
        public async Task GetAll()
        {
            var gists = await _gistHelper.GetAll();
            Assert.IsNotNull(gists);
            Assert.IsNotEmpty(gists);
        }

        [Test]
        public async Task CreateNew()
        {
            var gists = await _gistHelper.CreateNew("ListIDGistDb", "{}");
            TestContext.WriteLine(gists);
            Assert.IsNotNull(gists);
            Assert.IsNotEmpty(gists);
        }
    }
}