using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.NotesMakeFsDirPairs.ConsoleApp.Components
{
    public interface IProgramComponent
    {
        void Run(string[] args);
    }

    public class ProgramComponent : IProgramComponent
    {
        public void Run(string[] args)
        {

        }
    }

}