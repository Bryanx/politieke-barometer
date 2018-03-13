﻿using System;
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
    public class TestDataManager
    {
        private IDataManager dataManager;
        
        [TestMethod]
        public void Setup()
        {
            dataManager = new DataManager();
        }
        
        [TestMethod]
        public int TestGetNumberInfo()
        {
            throw new NotImplementedException();
        }
    }
}
