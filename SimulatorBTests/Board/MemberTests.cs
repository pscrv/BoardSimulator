using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimulatorB.Tests
{
    [TestClass()]
    public class MemberTests_B
    {
        Member m1 = new Member(MemberParameterCollection.DefaultCollection);
        Member m2 = new Member(MemberParameterCollection.DefaultCollection);
        Member m3 = new Member(MemberParameterCollection.DefaultCollection);

        [TestMethod()]
        public void MemberTest()
        {
            Assert.AreEqual(1 + m1.ID, m2.ID);
            Assert.AreEqual(1 + m2.ID, m3.ID);
        }
    }
}