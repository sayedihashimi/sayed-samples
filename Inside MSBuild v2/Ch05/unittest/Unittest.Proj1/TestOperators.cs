using NUnit.Framework;

namespace Unittest.Proj1
{
    [TestFixture]
    public class TestOperators
    {
        [Test]
        public void TestAddition()
        {
            int result = 1 + 1;
            Assert.AreEqual(2, result);

            result = 100 + 1;
            Assert.AreEqual(101, result);

            result = 1005 + (-1);
            Assert.AreEqual(1004,result);
        }
        [Test]
        public void TestSubtraction()
        {
            int result = 1 - 1;
            Assert.AreEqual(0, result);

            result = 100 - 1;
            Assert.AreEqual(99, result);

            result = 1005 - (-1);
            Assert.AreEqual(1006,result);
        }
//[Test]
//public void TestDivide()
//{
//    int numerator = 100;
//    int divisor = 20;
//    int result = numerator / divisor;
//    Assert.AreEqual(6, result);
//}
    }
}
