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
using Turmerik.Collections;
using Turmerik.Utils;
using Microsoft.VisualStudio.OLE.Interop;
using Turmerik.CodeAnalysis.Core.Components;

namespace Turmerik.MsVSTextTemplating.Components
{
    public interface IClnblTypesCodeParser : IClnblTypesCodeGeneratorCore
    {
        ClnblTypesCodeParserOutput.Immtbl ParseCode(
            ClnblTypesCodeGeneratorOptions.IClnbl opts);
    }

    public class ClnblTypesCodeParser : ClnblTypesCodeGeneratorBase, IClnblTypesCodeParser
    {
        public ClnblTypesCodeParser(
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

            var resultMtbl = TraverseTree(
                new SyntaxTreeTraversalOptsCore<ClnblTypesCodeParserArgs, TreeNode, ClnblTypesCodeParserOutput.Mtbl>.Mtbl
                {
                    DefsCode = options.DefsCode,
                    OnAscend = (args, trArgs, treeNode) => OnAscend(args, trArgs, treeNode),
                    OnDescend = (args, trArgs, treeNode) => OnDescend(args, trArgs, treeNode),
                    ResultFactory = args => new ClnblTypesCodeParserOutput.Mtbl()
                },
                argsFactory: opts => GetDefaultArgs<ClnblTypesCodeParserOutput.Mtbl>(opts).WithValue(dfArgs => new ClnblTypesCodeParserArgs(
                    opts,
                    dfArgs.SyntaxTree,
                    dfArgs.RootNode,
                    GetConfig(),
                    options,
                    new ClnblTypesCodeParserOutput.Mtbl
                    {
                        SyntaxTree = dfArgs.SyntaxTree,
                        Root = dfArgs.RootNode,
                        UsingNamespaceStatements = new List<string>(),
                        NamespaceAliases = new Dictionary<string, string>(),
                        StaticallyUsedNamespaces = new List<string>(),
                        UsedNamespaces = new List<string>(),
                        ClassDefinitions = new List<ParsedClassDefinition.Mtbl>(),
                        InterfaceDefinitions = new List<ParsedInterfaceDefinition.Mtbl>()
                    })));

            var output = resultMtbl.ToImmtbl();
            return output;
        }

        private void OnDescend(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs,
            TreeNode treeNode)
        {
            var kind = treeNode.Node.Kind();

            switch (kind)
            {
                case SyntaxKind.UsingDirective:
                    HandleUsingDirective(args, trArgs);
                    break;

                case SyntaxKind.FileScopedNamespaceDeclaration:
                    HandleFileScopedNamespaceDeclaration(args, trArgs);
                    break;

                case SyntaxKind.NamespaceDeclaration:
                    HandleNamespaceDeclaration(args, trArgs);
                    break;

                case SyntaxKind.ClassDeclaration:
                    HandleClassDeclarationStart(args, trArgs);
                    break;

                case SyntaxKind.InterfaceDeclaration:
                    HandleInterfaceDeclarationStart(args, trArgs);
                    break;

                case SyntaxKind.StructDeclaration:
                    HandleStructDeclarationStart(args, trArgs);
                    break;

                case SyntaxKind.EnumDeclaration:
                    HandleEnumDeclarationStart(args, trArgs);
                    break;

                case SyntaxKind.PropertyDeclaration:
                    HandlePropertyDeclarationStart(args, trArgs);
                    break;

                case SyntaxKind.MethodDeclaration:
                    HandleMethodDeclarationStart(args, trArgs);
                    break;

                case SyntaxKind.FieldDeclaration:
                    HandleMemberDeclarationStart(args, trArgs);
                    break;

                case SyntaxKind.EventDeclaration:
                    HandleMemberDeclarationStart(args, trArgs);
                    break;

                case SyntaxKind.Attribute:
                    HandleAttributeStart(args, trArgs);
                    break;

                default:
                    break;
            }
        }

        private void OnAscend(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs,
            TreeNode treeNode)
        {
            if (args.TrState == (ClnblTypesCodeGeneratorTreeTraversalState)(-100))
            {
                switch (args.TrState)
                {
                    case ClnblTypesCodeGeneratorTreeTraversalState.File:
                        break;
                    case ClnblTypesCodeGeneratorTreeTraversalState.TypeDef:
                        HandleTypeDeclarationEnd(args, trArgs);
                        break;
                    case ClnblTypesCodeGeneratorTreeTraversalState.PropertyDef:
                        HandlePropertyDeclarationEnd(args, trArgs);
                        break;
                    case ClnblTypesCodeGeneratorTreeTraversalState.MethodDef:
                        HandleMethodDeclarationEnd(args, trArgs);
                        break;
                    case ClnblTypesCodeGeneratorTreeTraversalState.MemberDef:
                        HandleMemberDeclarationEnd(args, trArgs);
                        break;
                    case ClnblTypesCodeGeneratorTreeTraversalState.AttrDecrt:
                        HandleAttributeEnd(args, trArgs);
                        break;
                    default:
                        throw new InvalidOperationException("Invalid tree traversal state");
                }
            }
        }

        private void PushNode<TNode>(
            ClnblTypesCodeParserArgs args,
            TNode nextTypeDef,
            TreeTraversalComponent<TreeNode>.Args trArgs)
            where TNode : ParsedTypeDefinition.Mtbl
        {
            var currentTypeDef = args.CurrentTypeDef;

            nextTypeDef.NestedParentClass = args.CurrentTypeDef.SafeCast<ParsedClassDefinition.Mtbl>(
                () => throw new InvalidOperationException("Only classes can have nested types"));

            nextTypeDef.Attributes = args.CurrentAttrDecrtsList;

            args.CurrentAttrDecrt = null;
            args.CurrentAttrDecrtsList = new List<ParsedAttributeDecoration.Mtbl>();

            args.CurrentTypeDef = nextTypeDef;
        }

        private void PopNode(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            var currentTypeDef = args.CurrentTypeDef;
            var parentNestedClass = currentTypeDef.NestedParentClass;

            bool isRootLevel = parentNestedClass == null;

            if (isRootLevel)
            {
                if (currentTypeDef is ParsedClassDefinition.Mtbl classDef)
                {
                    args.ParserOutput.ClassDefinitions.Add(classDef);
                }
                else if (currentTypeDef is ParsedInterfaceDefinition.Mtbl interfaceDef)
                {
                    args.ParserOutput.InterfaceDefinitions.Add(interfaceDef);
                }
            }
            else
            {
                parentNestedClass.NestedTypes.Add(currentTypeDef);
            }

            args.CurrentTypeDef = args.CurrentTypeDef.NestedParentClass;

            if (isRootLevel)
            {
                args.TrState = ClnblTypesCodeGeneratorTreeTraversalState.File;
            }
            else
            {
                args.TrState = ClnblTypesCodeGeneratorTreeTraversalState.TypeDef;
            }
        }

        private void HandleAttributeStart(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            var node = trArgs.CurrentTreeNode.Data.Node as AttributeSyntax;

            args.CurrentAttrDecrt = new ParsedAttributeDecoration.Mtbl
            {
                AttrTypeName = node.Name.ToString()
            };
        }

        private void HandleAttributeEnd(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            args.CurrentAttrDecrtsList.Add(args.CurrentAttrDecrt);
            args.CurrentAttrDecrt = null;
            args.TrState = ClnblTypesCodeGeneratorTreeTraversalState.TypeDef;
        }

        private void HandleMemberDeclarationStart(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            args.TrState = ClnblTypesCodeGeneratorTreeTraversalState.MemberDef;
        }

        private void HandleMemberDeclarationEnd(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            args.TrState = ClnblTypesCodeGeneratorTreeTraversalState.TypeDef;
        }

        private void HandlePropertyDeclarationStart(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            args.TrState = ClnblTypesCodeGeneratorTreeTraversalState.MemberDef;
        }

        private void HandlePropertyDeclarationEnd(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            args.TrState = ClnblTypesCodeGeneratorTreeTraversalState.TypeDef;
        }

        private void HandleMethodDeclarationStart(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            args.TrState = ClnblTypesCodeGeneratorTreeTraversalState.MemberDef;
        }

        private void HandleMethodDeclarationEnd(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            args.TrState = ClnblTypesCodeGeneratorTreeTraversalState.TypeDef;
        }

        private void HandleClassDeclarationStart(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            var node = trArgs.CurrentTreeNode.Data.Node as ClassDeclarationSyntax;

            PushNode(args, new ParsedClassDefinition.Mtbl
            {
                Name = node.Identifier.Text
            }, trArgs);
        }

        private void HandleInterfaceDeclarationStart(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            var node = trArgs.CurrentTreeNode.Data.Node as InterfaceDeclarationSyntax;

            PushNode(args, new ParsedInterfaceDefinition.Mtbl
            {
                Name = node.Identifier.Text
            }, trArgs);
        }

        private void HandleStructDeclarationStart(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            var node = trArgs.CurrentTreeNode.Data.Node as StructDeclarationSyntax;

            PushNode(args, new ParsedTypeDefinition.Mtbl
            {
                Name = node.Identifier.Text
            }, trArgs);
        }

        private void HandleEnumDeclarationStart(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            var node = trArgs.CurrentTreeNode.Data.Node as EnumDeclarationSyntax;

            PushNode(args, new ParsedTypeDefinition.Mtbl
            {
                Name = node.Identifier.Text
            }, trArgs);
        }

        private void HandleTypeDeclarationEnd(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            PopNode(args, trArgs);
        }

        private void HandleUsingDirective(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            AssertNoNamespaceDeclaration(args, trArgs);
            var node = trArgs.CurrentTreeNode.Data.Node as UsingDirectiveSyntax;
            var output = args.ParserOutput;

            output.UsingNamespaceStatements.Add(node.ToFullString());
            string @namespace = node.Name.ToString();

            if (node.StaticKeyword != default)
            {
                output.StaticallyUsedNamespaces.Add(@namespace);
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

        private void HandleNamespaceDeclaration(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            AssertNoNamespaceDeclaration(args, trArgs);
            var nsNode = trArgs.CurrentTreeNode.Data.Node as NamespaceDeclarationSyntax;
            var output = args.ParserOutput;

            output.Namespace = nsNode.Name.ToString();
        }

        private void HandleFileScopedNamespaceDeclaration(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            AssertNoNamespaceDeclaration(args, trArgs);
            var fsNsNode = trArgs.CurrentTreeNode.Data.Node as FileScopedNamespaceDeclarationSyntax;
            var output = args.ParserOutput;

            output.Namespace = fsNsNode.Name.ToString();
            output.NamespaceIsFileScoped = true;
        }

        private void AssertNoNamespaceDeclaration(
            ClnblTypesCodeParserArgs args,
            TreeTraversalComponent<TreeNode>.Args trArgs)
        {
            if (args.ParserOutput.Namespace != null)
            {
                throw new InvalidOperationException(
                    "Source code cannot contain multiple namespace declarations");
            }
        }
    }
}
