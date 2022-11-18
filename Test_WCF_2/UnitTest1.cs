using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Test_WCF_2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ServiceReference1.WCFMatrixClient client = new ServiceReference1.WCFMatrixClient();
            List<int[]> myDT = new List<int[]>();

            myDT.Add(new int[] { 1, 1, 1, 1 });
            myDT.Add(new int[] { 2, 2, 5, 5 });
            myDT.Add(new int[] { 3, 7, 7, 3 });
            myDT.Add(new int[] { 4, 6, 6, 8 });

            string _expected = "4 8 ";

            string res = client.FilterAndOrderValues( myDT.ToArray());

            Assert.AreEqual(_expected, res, "Wrong result. Check data.");
        }

        [TestMethod]
        public void TestMethod2()
        {
            ServiceReference1.WCFMatrixClient client = new ServiceReference1.WCFMatrixClient();
            List<int[]> myDT = new List<int[]>();
            myDT.Add(new int[] { 1, 2, 3 });
            myDT.Add(new int[] { 4, 5, 2 });
            myDT.Add(new int[] { 3, 3, 3 });

            string _expected = "4 ";

            string res = client.FilterAndOrderValues(myDT.ToArray());

            Assert.AreEqual(_expected, res, "Wrong result. Check data.");
        }

    }
}
