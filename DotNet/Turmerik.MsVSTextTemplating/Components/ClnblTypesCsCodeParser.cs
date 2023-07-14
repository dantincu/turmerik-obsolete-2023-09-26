using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.TreeTraversal;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Turmerik.MsVSTextTemplating.Components
{
    public class ClnblTypesCsCodeParser : ClnblTypesCodeGeneratorBase, IClnblTypesCodeParser
    {
        public ClnblTypesCsCodeParser(
            IAppConfig appConfig,
            ITreeTraversalComponentFactory treeTraversalComponentFactory) : base(
                appConfig,
                treeTraversalComponentFactory)
        {
        }


        public ClnblTypesCodeParserOutput.Immtbl ParseCode(
            ClnblTypesCodeGeneratorOptions.IClnbl options)
        {
            options = NormalizeOpts(options.AsMtbl());
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(options.DefsCode);
            CompilationUnitSyntax rootNode = syntaxTree.GetCompilationUnitRoot();

            var args = new ClnblTypesCsCodeParserArgs(
                GetConfig(),
                options,
                new ClnblTypesCodeParserOutput.Mtbl
                {
                    SyntaxTree = syntaxTree,
                    Root = rootNode,
                    UsingNamespaceStatements = new List<string>(),
                    NamespaceAliases = new Dictionary<string, string>(),
                    StaticallyUsedNamespaces = new List<string>(),
                    UsedNamespaces = new List<string>(),
                    ClassDefinitions = new List<ParserOutputClassDefinition.Mtbl>(),
                    InterfaceDefinitions = new List<ParserOutputInterfaceDefinition.Mtbl>()
                },
                rootNode.ChildNodes());

            ParseCode(args);

            var result = args.Output.ToImmtbl();
            return result;
        }

        private void ParseCode(ClnblTypesCsCodeParserArgs args)
        {
            AddNamespaces(args);

            GetCsFileNodes(args);

            AddNodes<ParserOutputTypeDefinition.Mtbl>(
                args,
                args.NsDeclrNodes,
                null);
        }

        private void GetCsFileNodes(
            ClnblTypesCsCodeParserArgs args)
        {
            var rootNodes = args.RootNodes;
            var output = args.Output;

            var nsDeclrNode = rootNodes.OfType<NamespaceDeclarationSyntax>().SingleOrDefault();

            if (nsDeclrNode != null)
            {
                output.Namespace = nsDeclrNode.Name.ToString();
                args.NsDeclrNodes = nsDeclrNode.ChildNodes();
            }
            else
            {
                var fileScopedNsDeclrNode = rootNodes.OfType<FileScopedNamespaceDeclarationSyntax>().SingleOrDefault();

                if (fileScopedNsDeclrNode != null)
                {
                    output.Namespace = fileScopedNsDeclrNode.Name.ToString();
                    output.NamespaceIsFileScoped = true;
                    args.NsDeclrNodes = rootNodes;
                }
            }
        }

        private void AddNodes<TEnclosingType>(
            ClnblTypesCsCodeParserArgs args,
            IEnumerable<SyntaxNode> nodesNmrbl,
            ParserOutputTypeDefinition.Mtbl enclosingType)
            where TEnclosingType : ParserOutputTypeDefinition.Mtbl
        {
            var attrDecorsList = new List<ParserOutputAttributeDecoration.Mtbl>();

            foreach (var node in nodesNmrbl)
            {
                TryAddNode(
                    new ClnblTypesCsCodeParserRdnlArgs<ParserOutputTypeDefinition.Mtbl>(
                        args,
                        node,
                        enclosingType,
                        attrDecorsList,
                        enclosingType as ParserOutputClassDefinition.Mtbl));
            }
        }

        private bool TryAddNode(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs)
        {
            bool isAttrDecor = false;

            bool hasMatch = TryAddClassDef(rdnlArgs);
            hasMatch = hasMatch || TryAddInterfaceDef(rdnlArgs);
            hasMatch = hasMatch || TryAddMemberDef(rdnlArgs);

            if (!hasMatch)
            {
                isAttrDecor = TryAddAttrDecor(rdnlArgs);
                hasMatch = isAttrDecor;
            }

            if (!isAttrDecor)
            {
                rdnlArgs.AttrDecorsList.Clear();
            }

            return hasMatch;
        }

        private bool TryAddClassDef(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs)
        {
            var classDefNode = rdnlArgs.Node as ClassDeclarationSyntax;
            bool retVal = classDefNode != null;

            if (retVal)
            {
                var classDef = GetClassDef(rdnlArgs);

                if (rdnlArgs.ParentNestedClassDef != null)
                {
                    rdnlArgs.ParentNestedClassDef.NestedTypes.Add(classDef);
                }
                else
                {
                    rdnlArgs.Args.Output.ClassDefinitions.Add(classDef);
                }
            }

            return retVal;
        }

        private bool TryAddInterfaceDef(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs)
        {
            var interfaceDefNode = rdnlArgs.Node as InterfaceDeclarationSyntax;
            bool retVal = interfaceDefNode != null;

            if (retVal)
            {
                var interfaceDef = GetInterfaceDef(rdnlArgs);

                if (rdnlArgs.ParentNestedClassDef != null)
                {
                    rdnlArgs.ParentNestedClassDef.NestedTypes.Add(interfaceDef);
                }
                else
                {
                    rdnlArgs.Args.Output.InterfaceDefinitions.Add(interfaceDef);
                }
            }

            return retVal;
        }

        private bool TryAddMemberDef(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs)
        {
            var memberDefNode = rdnlArgs.Node as MemberDeclarationSyntax;
            bool retVal = memberDefNode != null;

            if (retVal)
            {
                var memberDef = GetMemberDef(rdnlArgs);

                rdnlArgs.GetEnclosingType().MemberDeclarations.Add(memberDef);
            }

            return retVal;
        }

        private bool TryAddAttrDecor(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs)
        {
            var attrNode = rdnlArgs.Node as AttributeSyntax;
            bool retVal = attrNode != null;

            if (retVal)
            {
                var attrDecor = GetAttrDecor(rdnlArgs);
                rdnlArgs.AttrDecorsList.Add(attrDecor);
            }

            return retVal;
        }

        private ParserOutputClassDefinition.Mtbl GetClassDef(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs)
        {
            throw new NotImplementedException();
        }

        private ParserOutputInterfaceDefinition.Mtbl GetInterfaceDef(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs)
        {
            throw new NotImplementedException();
        }

        private ParserOutputTypeMemberDeclaration.Mtbl GetMemberDef(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs)
        {
            throw new NotImplementedException();
        }

        private ParserOutputAttributeDecoration.Mtbl GetAttrDecor(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs)
        {
            throw new NotImplementedException();
        }

        private void AddNamespaces(ClnblTypesCsCodeParserArgs args)
        {
            var output = args.Output;

            foreach (var node in args.RootNodes.OfType<UsingDirectiveSyntax>())
            {
                output.UsingNamespaceStatements.Add(node.ToFullString());
                string @namespace = node.Name.ToString();

                if (node.StaticKeyword != default)
                {
                    args.Output.StaticallyUsedNamespaces.Add(@namespace);
                }
                else if (node.Alias != null)
                {
                    output.NamespaceAliases.Add(
                        node.Alias.Name.ToString(),
                        @namespace);
                }
                else
                {
                    output.UsedNamespaces = output.UsedNamespaces ?? new List<string>();
                    output.UsedNamespaces.Add(@namespace);
                }
            }
        }
    }

    public class ClnblTypesCsCodeParserArgs
    {
        public ClnblTypesCsCodeParserArgs(
            ClnblTypesCodeGeneratorConfig.IClnbl config,
            ClnblTypesCodeGeneratorOptions.IClnbl options,
            ClnblTypesCodeParserOutput.Mtbl output,
             IEnumerable<SyntaxNode> rootNodes)
        {
            Config = config;
            Options = options;
            Output = output;
            RootNodes = rootNodes;
        }

        public ClnblTypesCodeGeneratorConfig.IClnbl Config { get; }
        public ClnblTypesCodeGeneratorOptions.IClnbl Options { get; }
        public ClnblTypesCodeParserOutput.Mtbl Output { get; }
        public IEnumerable<SyntaxNode> RootNodes { get; }
        public IEnumerable<SyntaxNode> NsDeclrNodes { get; set; }
    }

    public interface IClnblTypesCsCodeParserRdnlArgs
    {
        ClnblTypesCsCodeParserArgs Args { get; }
        SyntaxNode Node { get; }
        List<ParserOutputAttributeDecoration.Mtbl> AttrDecorsList { get; }
        ParserOutputClassDefinition.Mtbl ParentNestedClassDef { get; }

        ParserOutputTypeDefinition.Mtbl GetEnclosingType();
    }

    public readonly struct ClnblTypesCsCodeParserRdnlArgs<TEnclosingType> : IClnblTypesCsCodeParserRdnlArgs
        where TEnclosingType : ParserOutputTypeDefinition.Mtbl
    {
        public ClnblTypesCsCodeParserRdnlArgs(
            IClnblTypesCsCodeParserRdnlArgs src,
            TEnclosingType enclosingType,
            ParserOutputClassDefinition.Mtbl parentNestedClassDef)
        {
            Args = src.Args;
            Node = src.Node;
            EnclosingType = enclosingType;
            AttrDecorsList = src.AttrDecorsList;
            ParentNestedClassDef = parentNestedClassDef;
        }

        public ClnblTypesCsCodeParserRdnlArgs(
            ClnblTypesCsCodeParserArgs args,
            SyntaxNode node,
            TEnclosingType enclosingType,
            List<ParserOutputAttributeDecoration.Mtbl> attrDecorsList,
            ParserOutputClassDefinition.Mtbl parentNestedClassDef)
        {
            Args = args;
            Node = node;
            EnclosingType = enclosingType;
            AttrDecorsList = attrDecorsList;
            ParentNestedClassDef = parentNestedClassDef;
        }

        public ClnblTypesCsCodeParserArgs Args { get; }
        public SyntaxNode Node { get; }
        public TEnclosingType EnclosingType { get; }
        public List<ParserOutputAttributeDecoration.Mtbl> AttrDecorsList { get; }
        public ParserOutputClassDefinition.Mtbl ParentNestedClassDef { get; }

        public ParserOutputTypeDefinition.Mtbl GetEnclosingType() => EnclosingType;
    }
}