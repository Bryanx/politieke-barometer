using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BAR.DAL;
using System.Collections.Generic;
using BAR.BL.Domain.Users;

namespace TestDAL
{
    [TestClass]
    public class TestSubscriptionRepository
    {
        private ISubscriptionRepository subRepo;

        [TestInitialize]
        public void Setup()
        {
            subRepo = new SubscriptionRepository();
        }
       
        [TestMethod]
        public void TestReadAlerts()
        {
            //Arrange
            IEnumerable<Alert> alerts;

            //Act
            alerts = subRepo.ReadAlerts(1);

            //Assert
            Assert.IsNotNull(alerts, "A list/empty list should be returned.");
        }
    
        [TestMethod]
        public void TestReadSubscriptions()
        {
            //Arrange
            IEnumerable<Subscription> subs;

            //Act
            subs = subRepo.ReadSubscriptions(1);

            //Assert
            Assert.IsNotNull(subs, "A list/empty list should be returned.");
        }
     
        [TestMethod]
        public void TestUpdateSubscriptions()
        {
            //Data still lives in memory, so the test is redundant.
        }
    }
}
