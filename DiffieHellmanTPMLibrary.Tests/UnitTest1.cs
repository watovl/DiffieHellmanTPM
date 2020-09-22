using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiffieHellmanTPMLibrary.Tests {
    [TestClass]
    public class UnitTest1 {
        private TreeParityMachine Machine;

        [TestInitialize]
        public void TestInitialize() {
            Machine = new TreeParityMachine(2, 3, 2);
            
        }

        [TestMethod]
        public void TestMethod1() {
            
        }
    }
}
