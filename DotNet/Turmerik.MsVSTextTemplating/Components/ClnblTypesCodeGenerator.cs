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

namespace Turmerik.MsVSTextTemplating.Components
{
    public interface IClnblTypesCodeGenerator
    {
        string GenerateCode(ClnblTypesCodeGeneratorOpts.IClnbl opts);
    }

    public class ClnblTypesCodeGeneratorCodeParserArgsCore
    {
        public ClnblTypesCodeGeneratorCodeParserArgsCore(
            ClnblTypesCodeGeneratorConfig.IClnbl config,
            ClnblTypesCodeGeneratorOpts.IClnbl opts)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Opts = opts ?? throw new ArgumentNullException(nameof(opts));
        }

        public ClnblTypesCodeGeneratorConfig.IClnbl Config { get; }
        public ClnblTypesCodeGeneratorOpts.IClnbl Opts { get; }
    }

    public class ClnblTypesCodeGeneratorCodeParserArgs : ClnblTypesCodeGeneratorCodeParserArgsCore
    {
        public ClnblTypesCodeGeneratorCodeParserArgs(
            ClnblTypesCodeGeneratorConfig.IClnbl config,
            ClnblTypesCodeGeneratorOpts.IClnbl opts,
            ClnblTypesCodeParserOutput.Mtbl output,
            ITreeTraversalComponent<TreeNode> treeTraversal) : base(
                config,
                opts)
        {
            ParserOutput = output ?? throw new ArgumentNullException(nameof(output));
            TreeTraversal = treeTraversal ?? throw new ArgumentNullException(nameof(treeTraversal));
        }

        public ClnblTypesCodeParserOutput.Mtbl ParserOutput { get; }
        public ITreeTraversalComponent<TreeNode> TreeTraversal { get; }
        public TreeTraversalComponent<TreeNode>.Args TrArgs { get; set; }

        public class TreeNode
        {
            public TreeNode(SyntaxNode node)
            {
                Node = node ?? throw new ArgumentNullException(nameof(node));
            }

            public SyntaxNode Node { get; }
        }
    }

    public class ClnblTypesCodeGeneratorCodeGeneratorArgs : ClnblTypesCodeGeneratorCodeParserArgsCore
    {
        public ClnblTypesCodeGeneratorCodeGeneratorArgs(
            ClnblTypesCodeGeneratorConfig.IClnbl config,
            ClnblTypesCodeGeneratorOpts.IClnbl opts,
            ClnblTypesCodeParserOutput.Immtbl parserOutput) : base(
                config,
                opts)
        {
            ParserOutput = parserOutput ?? throw new ArgumentNullException(nameof(parserOutput));
        }

        public ClnblTypesCodeParserOutput.Immtbl ParserOutput { get; }
    }

    public abstract class ClnblTypesCodeGeneratorBase
    {
        protected ClnblTypesCodeGeneratorBase(
            IAppConfig appConfig,
            ITreeTraversalComponentFactory treeTraversalComponentFactory)
        {
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));

            TreeTraversalComponentFactory = treeTraversalComponentFactory ?? throw new ArgumentNullException(
                nameof(treeTraversalComponentFactory));
        }

        protected IAppConfig AppConfig { get; }
        protected ITreeTraversalComponentFactory TreeTraversalComponentFactory { get; }

        protected virtual ClnblTypesCodeGeneratorOpts.IClnbl NormalizeOpts(
            ClnblTypesCodeGeneratorOpts.Mtbl opts) => opts.ToImmtbl();

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
            ClnblTypesCodeGeneratorOpts.IClnbl opts)
        {
            var parserOutput = codeParser.ParseCode(opts);
            var args = GetArgs(opts, parserOutput);

            throw new NotImplementedException();
        }

        private ClnblTypesCodeGeneratorCodeGeneratorArgs GetArgs(
            ClnblTypesCodeGeneratorOpts.IClnbl opts,
            ClnblTypesCodeParserOutput.Immtbl parserOutput) => new ClnblTypesCodeGeneratorCodeGeneratorArgs(
                GetConfig(),
                opts,
                parserOutput);
    }
}
