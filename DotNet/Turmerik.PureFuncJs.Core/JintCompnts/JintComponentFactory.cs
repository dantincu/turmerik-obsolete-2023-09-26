using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public interface IJintComponentFactory
    {
        IJintComponent Create(string jsCode);
    }

    public class JintComponentFactory : IJintComponentFactory
    {
        public IJintComponent Create(string jsCode) => new JintComponent(jsCode);
    }
}
