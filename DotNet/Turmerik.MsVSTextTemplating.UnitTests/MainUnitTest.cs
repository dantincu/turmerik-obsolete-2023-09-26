using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.MsVSTextTemplating.Components;

namespace Turmerik.MsVSTextTemplating.UnitTests
{
    [TestClass]
    public class MainUnitTest : UnitTestBase
    {
        [TestMethod]
        public void TemplateFileNameParseTestMethod()
        {
            string templateFileName = "TextTemplate.clnbl-impl.tt";

            string csFilePath = ServiceProvider.GetRequiredService<IClnblTypesCodeParser>().GetImplCsFilePath(
                templateFileName);

            Assert.AreEqual(csFilePath, "TextTemplate.clnbl-impl.cs");
        }

        [TestMethod]
        public void MainTestMethod()
        {
        }
    }
}
