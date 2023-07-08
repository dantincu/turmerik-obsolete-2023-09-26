using EnvDTE;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Turmerik.MsVSTextTemplating.Components
{
    public class ClnblTypesGenerator
    {
        /// <summary>
        /// The full type name of the host object passed in from a .tt T4 template file is
        /// <see cref="Microsoft.VisualStudio.TextTemplating.VSHost.TextTemplatingService"/>
        /// but I don't have access to it from code as its marked as internal (and sealed).
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string GenerateCode(
            ITextTemplatingEngineHost host) => ServiceProviderContainer.Instance.Value.Services.GetRequiredService<IClnblTypesCodeGenerator>(
                ).GenerateCode(new ClnblTypesCodeGeneratorOptions.Mtbl
                {
                    TemplateFilePath = host.TemplateFile
                });
    }
}
