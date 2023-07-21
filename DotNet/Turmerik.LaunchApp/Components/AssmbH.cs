using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.LaunchApp.Components
{
    public class AssmbH
    {
        public static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
    }
}
