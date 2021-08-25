using ClassLibrary1;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace TestProject1
{

    [TestFixture]
    public class TestsMoqPattern
    {
        private ParentDi testClass;
        private Mock<IDataAccess> dummyObj;
        //private Mock<IDataAccess> dummyObj = new Mock<IDataAccess>();

        [SetUp]
        public void Setup()
        {
            // SetupはテストメソッドごとにCallされるので、ここでnewしたい場合も外でnewしたい場合もある
            // たとえば外でnewするとTestClassの中で同一のObjectをもつことになるので、Callテストが意図通りに動かない可能性がある(何回もよばれたことになるので)
            dummyObj = new Mock<IDataAccess>();
            testClass = new ParentDi(dummyObj.Object);
        }

        [TestCase(1, "1")]
        [TestCase(2, "2")]
        [TestCase(3, "3")]
        public void Test1(int inputData, string exp)
        {
            dummyObj.Setup(x => x.Get(inputData)).Returns(inputData.ToString());
            var res = testClass.GetData(inputData);
            Assert.That(res, Is.EqualTo(exp));
        }


        [Test()]
        public void Test2()
        {
            var res = testClass.GetData(1);
            dummyObj.Verify(x => x.Get(1), Times.Once);
            Assert.Pass();
        }
    }
}