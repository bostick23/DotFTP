namespace DotFTP.Test
{
    [TestClass]
    public class SFTPAgentTest
    {
        [TestMethod]
        public void TestSFTPAgent()
        {
            IFTPAgent agent = new SFTPAgent();
            List<string> list = agent.GetFiles("", 0, "", "", "", "");
            Console.Write(list.Count);
        }
    }
}