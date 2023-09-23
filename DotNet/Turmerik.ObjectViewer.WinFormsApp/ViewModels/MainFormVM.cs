using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.Logging;
using Turmerik.Ux.MvvmH;
using Turmerik.WinForms.ViewModels;

namespace Turmerik.ObjectViewer.WinFormsApp.ViewModels
{
    public interface IMainFormVM : IViewModelCore<MainFormState>
    {
    }

    public class MainFormVM : ViewModelBase<MainFormState>, IMainFormVM
    {
        public const string SCRIPT_ENTRY_POINT_CLASS_NAME = "Program";
        public const string SCRIPT_ENTRY_POINT_METHOD_NAME = "Main";

        public static readonly ReadOnlyDictionary<string, Type> ScriptEntryPointMethodArgTypes = new Dictionary<string, Type>
        {
            { "args", typeof(string[]) }
        }.RdnlD();

        public static readonly string ScriptEntryPointMethodSignatureCode;
        public static readonly string DefaultScriptCode;

        static MainFormVM()
        {
            ScriptEntryPointMethodSignatureCode = string.Join(
                ", ", ScriptEntryPointMethodArgTypes.Select(
                    kvp => string.Join(" ", kvp.Key, kvp.Value.FullName)));

            DefaultScriptCode = string.Join(
                Environment.NewLine,
                $"public static class {SCRIPT_ENTRY_POINT_CLASS_NAME}",
                "{",
                $"   public static void {SCRIPT_ENTRY_POINT_METHOD_NAME}({ScriptEntryPointMethodSignatureCode})",
                "    {",
                "    }",
                "}");
        }

        public MainFormVM(
            IAppLoggerCreator appLoggerFactory/* ,
            IWinFormsActionComponentFactory actionComponentFactory*/) : base(
                appLoggerFactory/* ,
                actionComponentFactory*/)
        {
        }
    }

    public class MainFormState
    {
    }
}
