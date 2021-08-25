using ClassLibrary1;
using NUnit.Framework;
using System.Collections.Generic;

namespace TestProject1
{
    public class DummyDataAccess : IDataAccess
    {
        private int _dummyData;
        public int dummyData
        {
            set {
                _dummyData = value;
            }
        }
        public void Delete(int id)
        {}

        public string Get(int id)
        {
            return _dummyData.ToString();
        }
        public IEnumerable<string> Gets()
        {
            return null;
        }
        public void Update(string data)
        {}
    }

    [TestFixture]
    public class TestsNotMoqPattern
    {
        private ParentDi testClass;
        private DummyDataAccess dummyObj = new DummyDataAccess();

        [SetUp]
        public void Setup()
        {
            // 固定値を返却するDummyClassをnewしてそのオブジェクトをInjectする
            testClass = new ParentDi(dummyObj);
        }

        [TestCase(1, "1")]
        [TestCase(2, "2")]
        [TestCase(3, "3")]
        public void Test1(int inputData, string exp)
        {
            dummyObj.dummyData = inputData;
            var res = testClass.GetData(inputData);
            Assert.That(res, Is.EqualTo(exp));
        }
    }
}