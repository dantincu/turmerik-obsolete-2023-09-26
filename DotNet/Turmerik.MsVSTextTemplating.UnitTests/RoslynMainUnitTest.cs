using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.Text;

namespace Turmerik.MsVSTextTemplating.UnitTests
{
    [TestClass]
    public class RoslynMainUnitTest : UnitTestBase
    {
        private const string INPUT_FILE_NAME = "SourceCodeSample.cs";
        private const string XML_FULL_OUTPUT_FILE_NAME = "ParsedFullOutput.xml";
        private const string XML_OUTPUT_FILE_NAME = "ParsedOutput.xml";

        private const string JSON_OUTPUT_FILE_NAME = "ParsedOutput.json";
        private const string JSON_MIN_OUTPUT_FILE_NAME = "ParsedMinOutput.json";

        private readonly IRoslynTestComponent testComponent;

        public RoslynMainUnitTest()
        {
            testComponent = ServiceProvider.GetRequiredService<IRoslynTestComponent>();
        }

        [TestMethod]
        public void MainTest()
        {
            var basePath = AppEnv.GetTypePath(
                AppEnvDir.Data,
                GetType());

            string code = ReadText(basePath, INPUT_FILE_NAME);
            var result = testComponent.ParseCode(code);

            var resultSrlzbl = result.Select(
                item => new RoslynTestComponentNodeSerializable(item)).ToArray();

            WriteText(
                resultSrlzbl.ToXml(),
                basePath,
                XML_FULL_OUTPUT_FILE_NAME);

            WithResult(
                resultSrlzbl,
                node => node.FullText = null,
                list =>
                {
                    WriteText(
                        list.ToXml(),
                        basePath,
                        XML_OUTPUT_FILE_NAME);

                    WriteText(
                        JsonH.ToJson(list),
                        basePath,
                        JSON_OUTPUT_FILE_NAME);
                });

            WithResult(
                resultSrlzbl,
                node => node.Text = null,
                list => WriteText(
                    JsonH.ToJson(list),
                    basePath,
                    JSON_MIN_OUTPUT_FILE_NAME));
        }

        private void WriteText(
            string text,
            string basePath,
            string fileName)
        {
            string filePath = Path.Combine(
                basePath,
                fileName);

            File.WriteAllText(filePath, text);
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

        private void WithResult(
            RoslynTestComponentNodeSerializable[] list,
            Action<RoslynTestComponentNodeSerializable> callback,
            Action<RoslynTestComponentNodeSerializable[]> finalCallback = null)
        {
            foreach (var node in list)
            {
                WithResult(node, callback);
            }

            finalCallback?.Invoke(list);
        }

        private void WithResult(
            RoslynTestComponentNodeSerializable node,
            Action<RoslynTestComponentNodeSerializable> callback)
        {
            callback(node);

            if (node.ChildNodes != null)
            {
                WithResult(
                    node.ChildNodes,
                    callback);
            }
        }
    }
}
