using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Turmerik.Text;
using System.Xml.Linq;
using Turmerik.TreeTraversal;
using ParserArgs = Turmerik.MsVSTextTemplating.Components.ClnblTypesCodeGeneratorCodeParserArgs;
using Turmerik.Collections;

namespace Turmerik.MsVSTextTemplating.Components
{
    public interface IClnblTypesCodeParser
    {
        ClnblTypesCodeParserOutput.Immtbl ParseCode(
            ClnblTypesCodeGeneratorOpts.IClnbl opts);

        string GetCsFilePath(string templateFileName);
    }

    public class ClnblTypesCodeParser : ClnblTypesCodeGeneratorBase, IClnblTypesCodeParser
    {
        public const string SUFFIX = "generated";
        public const string CS_EXTN = ".cs";

        public static readonly int SuffixLength = SUFFIX.Length + 1;
        public static readonly Regex SuffixRegex = new Regex($@"\-{SUFFIX}$");

        public ClnblTypesCodeParser(
            IAppConfig appConfig,
            TreeTraversal.ITreeTraversalComponentFactory treeTraversalComponentFactory) : base(
                appConfig,
                treeTraversalComponentFactory)
        {
        }

        public ClnblTypesCodeParserOutput.Immtbl ParseCode(
            ClnblTypesCodeGeneratorOpts.IClnbl opts)
        {
            opts = NormalizeOpts(opts.AsMtbl());

            string csFilePath = GetCsFilePath(
                opts.TemplateFilePath);

            string csCode = File.ReadAllText(csFilePath);

            SyntaxTree tree = CSharpSyntaxTree.ParseText(csCode);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            var args = GetArgs(
                opts,
                new ClnblTypesCodeParserOutput.Mtbl
                {
                    SyntaxTree = tree,
                    Root = root,
                    UsingNamespaceStatements = new List<string>(),
                    NamespaceAliases = new Dictionary<string, string>(),
                    StaticallyUsedNamespaces = new List<string>(),
                    UsedNamespaces = new List<string>(),
                });

            ParseCode(args);

            var output = args.ParserOutput.ToImmtbl();
            return output;
        }

        public string GetCsFilePath(string templateFileName)
        {
            string dirName = Path.GetDirectoryName(templateFileName);
            string fileNameWithoutExtn = Path.GetFileNameWithoutExtension(templateFileName);

            if (!SuffixRegex.IsMatch(fileNameWithoutExtn))
            {
                throw new InvalidOperationException(
                    string.Join(" ",
                    $@"The template file name must end with the string ""-{SUFFIX}"".",
                    $"The provided template file name is {fileNameWithoutExtn}"));
            }

            string restOfFileName = fileNameWithoutExtn.SubStr(
                (str, len) => len - SuffixLength).Item1;

            string csFileName = string.Concat(
                restOfFileName,
                CS_EXTN);

            string csFilePath = Path.Combine(dirName, csFileName);
            return csFilePath;
        }

        protected ParserArgs GetArgs(
            ClnblTypesCodeGeneratorOpts.IClnbl opts,
            ClnblTypesCodeParserOutput.Mtbl output)
        {
            var args = new ParserArgs(
                GetConfig(),
                NormalizeOpts(opts.AsMtbl()),
                output,
                TreeTraversalComponentFactory.Create<ParserArgs.TreeNode>());

            return args;
        }

        private void ParseCode(ParserArgs args)
        {
            args.TreeTraversal.Traverse(
                new TreeTraversalComponentOpts.Mtbl<ParserArgs.TreeNode>
            {
                ArgsCreated = trArgs => args.TrArgs = trArgs,
                RootNode = new ParserArgs.TreeNode(args.ParserOutput.Root),
                ChildNodesNmrtrRetriever = (arg, treeNode) => new TransformedEnumerator<SyntaxNode, ParserArgs.TreeNode>(
                    treeNode.Node.DescendantNodes().GetEnumerator(),
                    node => new ParserArgs.TreeNode(node)),
                GoNextPredicate = (trArgs, treeNode) => true,
                OnAscend = (arg, treeNode) =>
                {
                    var kind = treeNode.Node.Kind();

                    switch (kind)
                    {
                        case SyntaxKind.UsingDirective:
                            HandleUsingDirective(args);
                            break;

                        case SyntaxKind.FileScopedNamespaceDeclaration:
                            HandleFileScopedNamespaceDeclaration(args);
                            break;

                        case SyntaxKind.NamespaceDeclaration:
                            HandleNamespaceDeclaration(args);
                            break;

                        case SyntaxKind.ClassDeclaration:
                            HandleClassDeclaration(args);
                            break;

                        case SyntaxKind.InterfaceDeclaration:
                            HandleInterfaceDeclaration(args);
                            break;

                        case SyntaxKind.Attribute:
                            HandleAttribute(args);
                            break;

                        default:
                            break;
                    }
                },
                OnDescend = (arg, treeNode) =>
                {

                }
            });
        }

        private void HandleAttribute(
            ParserArgs args)
        {
            var node = args.TrArgs.CurrentTreeNode.Data.Node as UsingDirectiveSyntax;
            var output = args.ParserOutput;
        }

        private void HandleClassDeclaration(
            ParserArgs args)
        {
            var node = args.TrArgs.CurrentTreeNode.Data.Node as UsingDirectiveSyntax;
            var output = args.ParserOutput;
        }

        private void HandleInterfaceDeclaration(
            ParserArgs args)
        {
            var node = args.TrArgs.CurrentTreeNode.Data.Node as UsingDirectiveSyntax;
            var output = args.ParserOutput;
        }

        private void HandleUsingDirective(
            ParserArgs args)
        {
            AssertNoNamespaceDeclaration(args);
            var node = args.TrArgs.CurrentTreeNode.Data.Node as UsingDirectiveSyntax;
            var output = args.ParserOutput;

            output.UsingNamespaceStatements.Add(node.ToFullString());

            if (node.StaticKeyword != default)
            {
                output.StaticallyUsedNamespaces.Add(node.Name.ToString());
            }
            else if (node.Alias != null)
            {
                output.NamespaceAliases.Add(
                    node.Alias.ToString(),
                    node.Name.ToString());
            }
            else
            {
                output.UsedNamespaces = output.UsedNamespaces ?? new List<string>();
            }
        }

        private void HandleNamespaceDeclaration(
            ParserArgs args)
        {
            AssertNoNamespaceDeclaration(args);
            var nsNode = args.TrArgs.CurrentTreeNode.Data.Node as NamespaceDeclarationSyntax;
            var output = args.ParserOutput;

            output.NamespaceDeclaration = nsNode.ToFullString();
            output.Namespace = nsNode.Name.ToString();
        }

        private void HandleFileScopedNamespaceDeclaration(
            ParserArgs args)
        {
            AssertNoNamespaceDeclaration(args);
            var fsNsNode = args.TrArgs.CurrentTreeNode.Data.Node as FileScopedNamespaceDeclarationSyntax;
            var output = args.ParserOutput;

            output.FileScopedNamespaceDeclaration = fsNsNode.ToFullString();
            output.Namespace = fsNsNode.Name.ToString();
        }

        private void AssertNoNamespaceDeclaration(
            ParserArgs args)
        {
            if (args.ParserOutput.Namespace != null)
            {
                throw new InvalidOperationException(
                    "Source code cannot contain multiple namespace declarations");
            }
        }
    }
}
