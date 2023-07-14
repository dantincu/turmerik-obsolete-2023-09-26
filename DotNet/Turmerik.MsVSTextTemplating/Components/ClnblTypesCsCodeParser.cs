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
using Turmerik.Utils;
using Turmerik.CodeAnalysis.Core.Components;

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
                    ClassDefinitions = new List<ParsedClassDefinition.Mtbl>(),
                    InterfaceDefinitions = new List<ParsedInterfaceDefinition.Mtbl>()
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

            AddNodes<ParsedTypeDefinition.Mtbl>(
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
            TEnclosingType enclosingType)
            where TEnclosingType : ParsedTypeDefinition.Mtbl
        {
            foreach (var node in nodesNmrbl)
            {
                TryAddNode(
                    new ClnblTypesCsCodeParserRdnlArgs<TEnclosingType>(
                        args,
                        node,
                        enclosingType,
                        enclosingType as ParsedClassDefinition.Mtbl));
            }
        }

        private bool TryAddNode(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs)
        {
            bool hasMatch = TryAddClassDef(rdnlArgs);
            hasMatch = hasMatch || TryAddInterfaceDef(rdnlArgs);
            hasMatch = hasMatch || TryAddMemberDef(rdnlArgs);

            return hasMatch;
        }

        private bool TryAddClassDef(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs)
        {
            var classDefNode = rdnlArgs.Node as ClassDeclarationSyntax;
            bool retVal = classDefNode != null;

            if (retVal)
            {
                var classDef = GetClassDef(
                    rdnlArgs,
                    classDefNode);

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
                var interfaceDef = GetInterfaceDef(
                    rdnlArgs,
                    interfaceDefNode);

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
                var memberDef = GetMemberDef(
                    rdnlArgs,
                    memberDefNode);

                rdnlArgs.GetEnclosingType().MemberDeclarations.Add(memberDef);
            }

            return retVal;
        }

        private void AssignTypeDefProps(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs,
            TypeDeclarationSyntax typeDeclrNode,
            ParsedTypeDefinition.Mtbl typeDef)
        {
            typeDef.Name = typeDeclrNode.Identifier.Text;// .Split('<').First()

            typeDef.Modifiers = typeDeclrNode.Modifiers.Select(
                item => (ParsedCsKeyword)Enum.Parse(typeof(ParsedCsKeyword), item.Text)).ToList();

            typeDef.GenericTypeParameters = typeDeclrNode.TypeParameterList?.WithValue(
                    typeParameterList => GetTypeParametersList(typeParameterList));

            typeDef.Attributes = typeDeclrNode.AttributeLists.SelectMany(
                attrList => attrList.Attributes).Select(
                attr => GetAttrDecor(rdnlArgs, attr)).ToList();
        }

        private ParsedClassDefinition.Mtbl GetClassDef(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs,
            ClassDeclarationSyntax classDeclrNode)
        {
            var classDef = new ParsedClassDefinition.Mtbl();
            AssignTypeDefProps(rdnlArgs, classDeclrNode, classDef);

            var childNodes = classDeclrNode.ChildNodes();

            AddNodes(
                rdnlArgs.Args,
                childNodes,
                classDef);

            throw new NotImplementedException();
        }

        private List<ParsedGenericTypeParameter.Mtbl> GetTypeParametersList(
            TypeParameterListSyntax typeParameterList) => typeParameterList.Parameters.Select(
                typeParamNode => GetTypeParameter(typeParamNode)).ToList();

        private ParsedGenericTypeParameter.Mtbl GetTypeParameter(
            TypeParameterSyntax typeParamNode) => new ParsedGenericTypeParameter.Mtbl
            {
                Name = typeParamNode.Identifier.Text,
            };

        private ParsedInterfaceDefinition.Mtbl GetInterfaceDef(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs,
            InterfaceDeclarationSyntax interfaceDeclrNode)
        {
            var interfaceDef = new ParsedInterfaceDefinition.Mtbl();
            AssignTypeDefProps(rdnlArgs, interfaceDeclrNode, interfaceDef);

            var childNodes = interfaceDeclrNode.ChildNodes();

            AddNodes(
                rdnlArgs.Args,
                childNodes,
                interfaceDef);

            throw new NotImplementedException();
        }

        private ParsedTypeMemberDeclaration.Mtbl GetMemberDef(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs,
            MemberDeclarationSyntax memberDeclrNode)
        {
            throw new NotImplementedException();
        }

        private ParsedAttributeDecoration.Mtbl GetAttrDecor(
            IClnblTypesCsCodeParserRdnlArgs rdnlArgs,
            AttributeSyntax attrNode) => new ParsedAttributeDecoration.Mtbl
            {
                AttrTypeName = attrNode.Name.ToString(),
                // Kind = ParserOutputAttributeKind.None
            };

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
        ParsedClassDefinition.Mtbl ParentNestedClassDef { get; }

        ParsedTypeDefinition.Mtbl GetEnclosingType();
    }

    public readonly struct ClnblTypesCsCodeParserRdnlArgs<TEnclosingType> : IClnblTypesCsCodeParserRdnlArgs
        where TEnclosingType : ParsedTypeDefinition.Mtbl
    {
        public ClnblTypesCsCodeParserRdnlArgs(
            IClnblTypesCsCodeParserRdnlArgs src,
            TEnclosingType enclosingType,
            ParsedClassDefinition.Mtbl parentNestedClassDef)
        {
            Args = src.Args;
            Node = src.Node;
            EnclosingType = enclosingType;
            ParentNestedClassDef = parentNestedClassDef;
        }

        public ClnblTypesCsCodeParserRdnlArgs(
            ClnblTypesCsCodeParserArgs args,
            SyntaxNode node,
            TEnclosingType enclosingType,
            ParsedClassDefinition.Mtbl parentNestedClassDef)
        {
            Args = args;
            Node = node;
            EnclosingType = enclosingType;
            ParentNestedClassDef = parentNestedClassDef;
        }

        public ClnblTypesCsCodeParserArgs Args { get; }
        public SyntaxNode Node { get; }
        public TEnclosingType EnclosingType { get; }
        public ParsedClassDefinition.Mtbl ParentNestedClassDef { get; }

        public ParsedTypeDefinition.Mtbl GetEnclosingType() => EnclosingType;
    }
}