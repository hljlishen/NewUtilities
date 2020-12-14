using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewUtilitiesTests.Ini;
using Utilities.Ini;

namespace NewUtilities.Ini.Tests
{
    [TestClass()]
    public class IniSynconizerTests
    {
        [TestMethod()]
        public void CreateIniObjectTest()
        {
        }

        [TestMethod()]
        public void SaveSettingsTest()
        {
            var ini = new IniSynconizer();
            //var obj = ini.CreateIniObject<TestClass>("C:\\code\\1.ini");
            //ini.SaveSettings(obj);
        }
    }
}