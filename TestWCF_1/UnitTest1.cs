using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;

namespace TestWCF_1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            HRC_WCFRef.WCFMatrixClient client = new HRC_WCFRef.WCFMatrixClient();
            List<int[]> myDT = new List<int[]>();
            
            myDT.Add(new int[] { 1, 1, 1, 1 });
            myDT.Add(new int[] { 2, 2, 5, 5 });
            myDT.Add(new int[] { 3, 7, 7, 3 });
            myDT.Add(new int[] {4, 6, 6, 8 });

            int _expected = -48;  
            //https://www.bing.com/ck/a?!&&p=42643bf4a13580a0JmltdHM9MTY2ODY0MzIwMCZpZ3VpZD0zODUyNDMwNi05ODY5LTZhYTgtMGY4OS01Mjk0OTliNDZiZDImaW5zaWQ9NTE3NQ&ptn=3&hsh=3&fclid=38524306-9869-6aa8-0f89-529499b46bd2&psq=calculate+determinant+4x4+on+line&u=a1aHR0cHM6Ly93d3cud29sZnJhbWFscGhhLmNvbS93aWRnZXRzL3ZpZXcuanNwP2lkPTNjNjYyNTVlMDhlYmQ0ZWU5YzIxMWVmMDg1MmRkYzI4&ntb=1
            
            int res = client.CalcDeterminant(myDT.ToArray());

            Assert.AreEqual(_expected, res, "Wrong result. Check data.");
        }

        [TestMethod]
        public void TestMethod2()
        {
            HRC_WCFRef.WCFMatrixClient client = new HRC_WCFRef.WCFMatrixClient();
            List<int[]> myDT = new List<int[]>(); 
            myDT.Add(new int[] { 1, 2, 3 });
            myDT.Add(new int[] { 4, 5, 2 });
            myDT.Add(new int[] { 3, 3, 3 });

            int _expected = -12;
            //https://www.bing.com/ck/a?!&&p=8bbd0d19216c7107JmltdHM9MTY2ODY0MzIwMCZpZ3VpZD0zODUyNDMwNi05ODY5LTZhYTgtMGY4OS01Mjk0OTliNDZiZDImaW5zaWQ9NTE5Mw&ptn=3&hsh=3&fclid=38524306-9869-6aa8-0f89-529499b46bd2&psq=calculate+determinant+3x3+on+line&u=a1aHR0cHM6Ly93d3cud29sZnJhbWFscGhhLmNvbS93aWRnZXRzL3ZpZXcuanNwP2lkPTdmY2IwYTJjMGYwZjQxZDlmNDQ1NGFjMmQ4ZWQ3YWQ2&ntb=1

            int res = client.CalcDeterminant(myDT.ToArray());

            Assert.AreEqual(_expected, res, "Wrong result. Check data.");
        }

        [TestMethod]
        public void TestMethod3()
        {
            HRC_WCFRef.WCFMatrixClient client = new HRC_WCFRef.WCFMatrixClient();
            List<int[]> myDT = new List<int[]>();

            myDT.Add(new int[] { 1, 2 });
            myDT.Add(new int[] { 2, 2 });

            int _expected = -2;
            int res = client.CalcDeterminant(myDT.ToArray());

            Assert.AreEqual(_expected, res, "Wrong result. Check data.");
        }

        [TestMethod]
        public void TestMethod4()
        {
            HRC_WCFRef.WCFMatrixClient client = new HRC_WCFRef.WCFMatrixClient();
            List<int[]> myDT = new List<int[]>();

            myDT.Add(new int[] { 1 });
            int _expected = 1;
            int res = client.CalcDeterminant(myDT.ToArray());

            Assert.AreEqual(_expected, res, "Wrong result. Check data.");
        }


    }
}
