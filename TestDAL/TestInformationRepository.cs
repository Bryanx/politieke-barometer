using Microsoft.VisualStudio.TestTools.UnitTesting;
using BAR.DAL;
using System;

namespace TestDAL
{
    /// <summary>
    /// Some rules:
    /// For testing we use MS test
    /// We use the AAA-pattren if we write a test, this means:
    /// - Arrange
    /// - Act
    /// - Assert
    /// </summary>
    [TestClass]
    public class TestInformationRepository
    {
        private IInformationRepository infoRepo;

        [TestInitialize]
        public void Setup()
        {
            infoRepo = new InformationRepository();
        }

        /// <summary>
        /// Im not sure of this implementations yet
        /// we need to test the method when it acctually gives back a number.
        /// </summary>
        [TestMethod]
        public void TestGetNumberInfo()
        {
            //Arrange
            int aantal;

            //Act
            aantal = infoRepo.GetNumberInfo(1, DateTime.Now.AddDays(-7));

            //Assert
            Assert.AreNotEqual(aantal, 0, "Aantal shoud not be 0.");
        }
    }
}
