using System;
using BAR.BL.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestBL
{
    /// <summary>
    /// Further implementation can only be possible if we
    /// make some kind of a mock-repository so that we only test
    /// on the logic of the managers and not their repositories.
    /// Because if something changes in the repositories, 
    /// the test becomes redundant.
    /// </summary>
    [TestClass]
    public class TestItemManager
    {
        private IItemManager itemManager;

        [TestInitialize]
        public void Setup()
        {
            itemManager = new ItemManager();
        }

        public void TestDetermineTrending()
        {
            throw new NotImplementedException();
        }

        double TestGetTrendingPer()
        {
            throw new NotImplementedException();
        }

    }
}
