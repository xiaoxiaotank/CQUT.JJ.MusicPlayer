using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CQUT.JJ.MusicPlayer.WCFService;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using System.Linq;

namespace CQUT.JJ.MusicPlayer.UnitTest
{
    /// <summary>
    /// SearchUnitTest 的摘要说明
    /// </summary>
    [TestClass]
    public class SearchUnitTest
    {
        private readonly ISearchService _searchService;

        public SearchUnitTest()
        {
            _searchService = new SearchService();
        }

        [TestMethod]
        public void Search()
        {
            var result = _searchService.Search(MusicRequestType.Song, "罗志祥",1);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Results.Count(), 20);
        }
    }
}
