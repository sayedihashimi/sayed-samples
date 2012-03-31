using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Unittest.Proj2
{
    [TestFixture]
    public class TestStirng
    {
        [Test]
        public void TestConstructors()
        {
            string str01 = new string("string here".ToCharArray());
            Assert.AreEqual("string here", str01);

            char[] stringChars = new char[] { 's', 't', 'r', 'i', 'n', 'g' };
            string str02 = new string(stringChars);
            Assert.AreEqual("string", str02);

            string str03 = new string("string here".ToCharArray(),0,6);
            Assert.AreEqual("string", str03);
        }
    }
}
