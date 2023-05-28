﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Core.FileSystem;
using Turmerik.Core.Text;
using Turmerik.Testing.Core;
using Xunit;

namespace Turmerik.UnitTests.Tests
{
    public class FsPathNormalizerUnitTest : UnitTestBase
    {
        private readonly Dictionary<string, Tuple<string, bool>> testDataDictnr;

        private readonly string[] invalidPaths = new string[]
        {
            "C:\\..",
            "C:\\..\\",
            "C:\\..\\asdf",
            "/../",
            "/../asdf",
            "\\..\\",
            "\\..\\asdf",
        };

        private readonly string[] validUriSchemeLikeStrs = new string[]
        {
            "https:",
            "http:",
            "asdf:",
            "C:"
        };

        private readonly string[] invalidUriSchemeLikeStrs = new string[]
        {
            "https::",
            "http:/",
            "asdf:\\",
            "/https:",
            "\\http:",
            "/:asdf",
        };

        public FsPathNormalizerUnitTest()
        {
            testDataDictnr = GetTestDataDictnr();
        }

        [Fact]
        public void UriSchemeRegexTest()
        {
            foreach (var validStr in validUriSchemeLikeStrs)
            {
                bool isValid = UriH.UriSchemeRegex.IsMatch(validStr);
                Assert.True(isValid);
            }

            foreach (var invalidStr in invalidUriSchemeLikeStrs)
            {
                bool isValid = UriH.UriSchemeRegex.IsMatch(invalidStr);
                Assert.False(isValid);
            }
        }

        [Fact]
        public void MainTest()
        {
            var component = ServiceProviderContainer.Instance.Value.Services.GetRequiredService<IFsPathNormalizer>();
            int i = 0;

            for (i = 0; i < invalidPaths.Length; i++)
            {
                var path = invalidPaths[i];
                var result = component.TryNormalizePath(path);

                Assert.False(result.IsValid);
            }

            i = 0;

            foreach (var kvp in testDataDictnr)
            {
                var result = component.TryNormalizePath(kvp.Key);

                if (string.IsNullOrEmpty(kvp.Key))
                {
                    Assert.True(result.IsEmpty);
                }
                else
                {
                    Assert.True(result.IsValid);
                }

                Assert.Equal(kvp.Value.Item1, result.NormalizedPath);
                Assert.Equal(kvp.Value.Item2, result.IsRooted);

                i++;
            }
        }

        private Dictionary<string, Tuple<string, bool>> GetTestDataDictnr()
        {
            var dictnr = new Dictionary<string, Tuple<string, bool>>();

            AddTestData(dictnr, "", "", false);
            AddTestData(dictnr, ".", "", false);
            AddTestData(dictnr, "./", "", false);
            AddTestData(dictnr, ".\\", "", false);

            AddTestData(dictnr, "/./", Path.DirectorySeparatorChar.ToString(), true);
            AddTestData(dictnr, "\\.\\", Path.DirectorySeparatorChar.ToString(), true);

            AddTestData(dictnr, "C:", "C:", true);
            AddTestData(dictnr, "C:\\", "C:", true);
            AddTestData(dictnr, "C:/", "C:", true);
            AddTestData(dictnr, "/asdf", "\\asdf", true);
            AddTestData(dictnr, "/asdf/", "\\asdf", true);
            AddTestData(dictnr, "\\asdf", "\\asdf", true);
            AddTestData(dictnr, "\\asdf\\", "\\asdf", true);
            AddTestData(dictnr, "\\asdf/", "\\asdf", true);

            AddTestData(dictnr, "C:\\asdf", "C:\\asdf", true);
            AddTestData(dictnr, "C:\\asdf\\", "C:\\asdf", true);
            AddTestData(dictnr, "C:\\asdf/", "C:\\asdf", true);

            AddTestData(dictnr, "C:\\asdf\\qwer", "C:\\asdf\\qwer", true);
            AddTestData(dictnr, "C:\\asdf\\qwer\\", "C:\\asdf\\qwer", true);
            AddTestData(dictnr, "C:\\asdf/qwer/", "C:\\asdf\\qwer", true);

            AddTestData(dictnr, "C:\\asdf\\..", "C:", true);
            AddTestData(dictnr, "C:\\asdf\\..\\", "C:", true);
            AddTestData(dictnr, "C:\\asdf\\../", "C:", true);

            AddTestData(dictnr, "C:\\asdf\\.\\qwer", "C:\\asdf\\qwer", true);
            AddTestData(dictnr, "C:\\asdf\\.\\qwer\\", "C:\\asdf\\qwer", true);
            AddTestData(dictnr, "C:\\asdf/./qwer/", "C:\\asdf\\qwer", true);

            AddTestData(dictnr, "\\asdf\\qwer", "\\asdf\\qwer", true);
            AddTestData(dictnr, "\\asdf\\qwer\\", "\\asdf\\qwer", true);
            AddTestData(dictnr, "\\asdf/qwer/", "\\asdf\\qwer", true);

            AddTestData(dictnr, "\\asdf\\.\\qwer", "\\asdf\\qwer", true);
            AddTestData(dictnr, "\\asdf\\.\\qwer\\", "\\asdf\\qwer", true);
            AddTestData(dictnr, "\\asdf/./qwer/", "\\asdf\\qwer", true);

            AddTestData(dictnr, "F:\\X\\test-src", "F:\\X\\test-src", true);

            return dictnr;
        }

        private void AddTestData(
            Dictionary<string, Tuple<string, bool>> dictnr,
            string inputPath,
            string normPath,
            bool isRooted)
        {
            var tuple = new Tuple<string, bool>(
                normPath,
                isRooted);

            dictnr.Add(inputPath, tuple);
        }
    }
}
