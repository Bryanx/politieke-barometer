using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BAR.DAL;
using BAR.BL.Domain.Items;

namespace TestDAL
{
    [TestClass]
    public class TestItemRepository
    {
        private IItemRepository itemRepo;

        [TestInitialize]
        public void Setup()
        {
            itemRepo = new ItemRepository();
        }

        [TestMethod]
        public void TestReadItem()
        {
            //Arrange
            Item item;

            //Act
            item = itemRepo.ReadItem(1);

            //Assert
            Assert.IsNotNull(item, "The item shoud not be null.");
        }

        [TestMethod]
        public void TestUpdateItemTrending()
        {
            //Data still lives in memory, so the test is redundant.
        }

        [TestMethod]
        public void TestpdateLastUpdated()
        {
            //Data still lives in memory, so the test is redundant.
        }
    }
}
