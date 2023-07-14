using EnvDTE;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Text.RegularExpressions;
using Turmerik.Collections;
using Turmerik.Text;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.TreeTraversal;
using Turmerik.CodeAnalysis.Core.Dependencies;
using static Turmerik.CodeAnalysis.Core.Dependencies.SyntaxTreeTraversal;

namespace Turmerik.MsVSTextTemplating.Components
{
    public interface IClnblTypesCodeGeneratorCore
    {
        string GetImplCsFilePath(string templateFilePath);
        string GetDefsCsFilePath(string templateFilePath);
    }

    public interface IClnblTypesCodeGenerator : IClnblTypesCodeGeneratorCore
    {
        string GenerateCode(ClnblTypesCodeGeneratorOptions.IClnbl opts);
    }

    public enum ClnblTypesCodeGeneratorTreeTraversalState
    {
        None = 0,
        File,
        TypeDef,
        PropertyDef,
        MethodDef,
        MemberDef,
        AttrDecrt
    }

    public class ClnblTypesCodeParserArgs : Args<ClnblTypesCodeParserArgs, TreeNode, ClnblTypesCodeParserOutput.Mtbl>
    {
        public ClnblTypesCodeParserArgs(
            SyntaxTreeTraversalOptsCore<ClnblTypesCodeParserArgs, TreeNode, ClnblTypesCodeParserOutput.Mtbl>.Immtbl traversalOpts,
            SyntaxTree syntaxTree,
            CompilationUnitSyntax rootNode,
            ClnblTypesCodeGeneratorConfig.IClnbl config,
            ClnblTypesCodeGeneratorOptions.IClnbl opts,
            ClnblTypesCodeParserOutput.Mtbl output) : base(
                traversalOpts,
                syntaxTree,
                rootNode)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Options = opts ?? throw new ArgumentNullException(nameof(opts));
            ParserOutput = output ?? throw new ArgumentNullException(nameof(output));
        }

        public ClnblTypesCodeGeneratorConfig.IClnbl Config { get; }
        public ClnblTypesCodeGeneratorOptions.IClnbl Options { get; }
        public ClnblTypesCodeGeneratorTreeTraversalState TrState { get; set; }
        public ClnblTypesCodeParserOutput.Mtbl ParserOutput { get; }

        public List<ParserOutputAttributeDecoration.Mtbl> CurrentAttrDecrtsList { get; set; }
        public ParserOutputAttributeDecoration.Mtbl CurrentAttrDecrt { get; set; }
        public ParserOutputTypeDefinition.Mtbl CurrentTypeDef { get; set; }
        public ParserOutputTypeMemberDeclaration.Mtbl CurrentMemberDeclr { get; set; }
    }

    public class ClnblTypesCodeGeneratorArgs
    {
        public ClnblTypesCodeGeneratorArgs(
            ClnblTypesCodeGeneratorConfig.IClnbl config,
            ClnblTypesCodeGeneratorOptions.IClnbl opts,
            ClnblTypesCodeParserOutput.Immtbl parserOutput)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Options = opts ?? throw new ArgumentNullException(nameof(opts));
            ParserOutput = parserOutput ?? throw new ArgumentNullException(nameof(parserOutput));
        }

        public ClnblTypesCodeGeneratorConfig.IClnbl Config { get; }
        public ClnblTypesCodeGeneratorOptions.IClnbl Options { get; }
        public ClnblTypesCodeGeneratorTreeTraversalState TrState { get; set; }
        public ClnblTypesCodeParserOutput.Immtbl ParserOutput { get; }
    }

    public abstract class ClnblTypesCodeGeneratorBase : SyntaxTreeTraversal, IClnblTypesCodeGeneratorCore
    {
        public const string DEFS_SFFX = ".clnbl-defs";
        public const string IMPL_SFFX = ".clnbl-impl";

        public const string TT_EXTN = ".tt";
        public const string CS_EXTN = ".cs";

        public static readonly int DefsSffxLen = DEFS_SFFX.Length;
        public static readonly int ImplSffxLen = IMPL_SFFX.Length;

        public static readonly string DefsSffxRegexStr = DEFS_SFFX.EncodeForRegex(false, true);
        public static readonly string ImplSffxRegexStr = IMPL_SFFX.EncodeForRegex(false, true);

        public static readonly Regex DefsSffxRegex = new Regex(DefsSffxRegexStr);
        public static readonly Regex ImplSffxRegex = new Regex(ImplSffxRegexStr);

        protected ClnblTypesCodeGeneratorBase(
            IAppConfig appConfig,
            ITreeTraversalComponentFactory treeTraversalComponentFactory) : base(treeTraversalComponentFactory)
        {
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
        }

        protected IAppConfig AppConfig { get; }

        public string GetImplCsFilePath(
            string templateFilePath) => GetCsFilePath(
                templateFilePath,
                IMPL_SFFX);

        public string GetDefsCsFilePath(
            string templateFilePath) => GetCsFilePath(
                templateFilePath,
                DEFS_SFFX);

        protected string GetCsFilePath(
            string templateFilePath,
            string csFileNameSffx)
        {
            string dirName = Path.GetDirectoryName(templateFilePath);
            string fileNameWithoutExtn = Path.GetFileNameWithoutExtension(templateFilePath);

            string baseFileName = GetBaseFileName(fileNameWithoutExtn);

            string csFileName = string.Concat(
                baseFileName,
                csFileNameSffx,
                CS_EXTN);

            string csFilePath = Path.Combine(dirName, csFileName);
            return csFilePath;
        }

        protected string GetBaseFileName(string fileNameWithoutExtn)
        {
            if (!ImplSffxRegex.IsMatch(fileNameWithoutExtn))
            {
                throw new InvalidOperationException(
                    string.Join(" ",
                    $@"The template file name must end with the string ""{IMPL_SFFX}{TT_EXTN}""",
                    $"The provided template file name is {fileNameWithoutExtn}"));
            }

            string baseFileName = fileNameWithoutExtn.SubStr(
                (str, len) => len - ImplSffxLen).Item1;

            return baseFileName;
        }

        protected virtual ClnblTypesCodeGeneratorOptions.IClnbl NormalizeOpts(
            ClnblTypesCodeGeneratorOptions.Mtbl options)
        {
            if (options.DefsCode == null)
            {
                string csFilePath = GetImplCsFilePath(
                    options.TemplateFilePath);

                options.DefsCode = File.ReadAllText(csFilePath);
            }

            return options.ToImmtbl();
        }

        protected virtual ClnblTypesCodeGeneratorConfig.IClnbl NormalizeConfig(
            ClnblTypesCodeGeneratorConfigSrlzbl.Mtbl opts) => new ClnblTypesCodeGeneratorConfig.Immtbl(opts);

        protected ClnblTypesCodeGeneratorConfig.IClnbl GetConfig() => AppConfig.Data;
    }

    public class ClnblTypesCodeGenerator : ClnblTypesCodeGeneratorBase, IClnblTypesCodeGenerator
    {
        private readonly IClnblTypesCodeParser codeParser;

        public ClnblTypesCodeGenerator(
            IAppConfig appConfig,
            ITreeTraversalComponentFactory treeTraversalComponentFactory,
            IClnblTypesCodeParser codeParser) : base(
                appConfig,
                treeTraversalComponentFactory)
        {
            this.codeParser = codeParser ?? throw new ArgumentNullException(nameof(codeParser));
        }

        public string GenerateCode(
            ClnblTypesCodeGeneratorOptions.IClnbl options)
        {
            options = NormalizeOpts(options.AsMtbl());

            var parserOutput = codeParser.ParseCode(options);
            var args = GetArgs(options, parserOutput);

            throw new NotImplementedException();
        }

        private ClnblTypesCodeGeneratorArgs GetArgs(
            ClnblTypesCodeGeneratorOptions.IClnbl options,
            ClnblTypesCodeParserOutput.Immtbl parserOutput) => new ClnblTypesCodeGeneratorArgs(
                this.GetConfig(),
                options,
                parserOutput);
    }
}
