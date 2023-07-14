using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.MsVSTextTemplating.Components;

namespace Turmerik.MsVSTextTemplating.UnitTests
{
    [TestClass]
    public class MainUnitTest : UnitTestBase
    {
        private const string INPUT_FILE_NAME = "SourceCodeSample.cs";

        private readonly IClnblTypesCodeParser clnblTypesCodeParser;
        private readonly IClnblTypesCodeGenerator clnblTypesCodeGenerator;

        public MainUnitTest()
        {
            clnblTypesCodeParser = ServiceProvider.GetRequiredService<IClnblTypesCodeParser>();
            clnblTypesCodeGenerator = ServiceProvider.GetRequiredService<IClnblTypesCodeGenerator>();
        }

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
            var basePath = AppEnv.GetPath(
                AppEnvDir.Data,
                GetType());

            string code = ReadText(basePath, INPUT_FILE_NAME);

            var result = clnblTypesCodeParser.ParseCode(
                new ClnblTypesCodeGeneratorOptions.Mtbl
                {
                    DefsCode = code
                });
        }

        private string ReadText(
            string basePath,
            string fileName)
        {
            string filePath = Path.Combine(
                basePath,
                fileName);

            string text = File.ReadAllText(filePath);
            return text;
        }
    }
}
