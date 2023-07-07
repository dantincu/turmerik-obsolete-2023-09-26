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
            string templateFileName = "TextTemplate-generated.tt";

            string csFilePath = ServiceProvider.GetRequiredService<IClnblTypesCodeParser>().GetCsFilePath(
                templateFileName);

            Assert.AreEqual(csFilePath, "TextTemplate.cs");
        }

        [TestMethod]
        public void MainTestMethod()
        {
            var path = AppEnv.GetPath(
                AppEnvDir.Data,
                GetType(),
                "SourceCodeSample.cs");
        }
    }
}
