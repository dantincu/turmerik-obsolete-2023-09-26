using Microsoft.VisualStudio.TextTemplating.VSHost;
using Microsoft.VisualStudio.TextTemplating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.MsVSTextTemplating.Components
{
    public interface ITextTemplatingService : STextTemplating, IDebugTextTemplating, ITextTemplating, ITextTemplatingComponents, ITextTemplatingSessionHost, ITextTemplatingEngineHost, ITextTemplatingOrchestrator, System.IServiceProvider, Microsoft.VisualStudio.OLE.Interop.IServiceProvider, IDisposable
    {
    }
}
