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
using System.IO;
using System.Text.RegularExpressions;
using Turmerik.Collections;
using Turmerik.Text;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.TreeTraversal;
using Turmerik.CodeAnalysis.Core.Dependencies;

namespace Turmerik.MsVSTextTemplating.Components
{
    public interface IClnblTypesCodeGenerator
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

    public class ClnblTypesCodeParserArgs : SyntaxTreeTraversal.Args<ClnblTypesCodeParserArgs, ClnblTypesCodeParserOutput.Mtbl>
    {
        public ClnblTypesCodeParserArgs(
            SyntaxTreeTraversalOptsCore<ClnblTypesCodeParserArgs, ClnblTypesCodeParserOutput.Mtbl>.Immtbl traversalOpts,
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
        public ParserOutputClnblTypeMemberDeclaration.Mtbl CurrentMemberDeclr { get; set; }
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

    public abstract class ClnblTypesCodeGeneratorBase : SyntaxTreeTraversal
    {
        protected ClnblTypesCodeGeneratorBase(
            IAppConfig appConfig,
            ITreeTraversalComponentFactory treeTraversalComponentFactory) : base(treeTraversalComponentFactory)
        {
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
        }

        protected IAppConfig AppConfig { get; }

        protected virtual ClnblTypesCodeGeneratorOptions.IClnbl NormalizeOpts(
            ClnblTypesCodeGeneratorOptions.Mtbl opts) => opts.ToImmtbl();

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
