using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BAR.BL.Managers;

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
    public class TestSubscriptionManager
    {
        private ISubscriptionManager subManager;

        public void Setup()
        {
            subManager = new SubscriptionManager();
        }
        
        [TestMethod]
        public void GenerateAlerts()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetAllAlerts()
        {
            throw new NotImplementedException();
        }
    }
}
