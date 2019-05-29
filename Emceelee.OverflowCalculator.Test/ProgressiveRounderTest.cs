using Microsoft.VisualStudio.TestTools.UnitTesting;

using Emceelee.OverflowCalculator;

namespace Emceelee.OverflowCalculator.Test
{
    [TestClass]
    public class ProgressiveRounderTest
    {
        [TestMethod]
        public void GetSplits()
        {
            //arrange
            var participants = 3;

            //act
            var result = ProgressiveRounder.GetSplits(100, participants);

            //assert
            Assert.AreEqual(participants, result.Count);
            Assert.AreEqual(33.34M, result[0]);
            Assert.AreEqual(33.33M, result[1]);
            Assert.AreEqual(33.33M, result[2]);
        }
    }
}
