using Microsoft.VisualStudio.TestPlatform.TestHost;
using Tests;

namespace TestProjectClient
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Tests.Program.Main(null);
            Tests.Program program = new Tests.Program();
            program.Test();
        }
    }
}