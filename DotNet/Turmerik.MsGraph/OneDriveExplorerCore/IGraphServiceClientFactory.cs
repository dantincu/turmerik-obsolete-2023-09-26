﻿using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.MsGraph.OneDriveExplorerCore
{
    public interface IGraphServiceClientFactory
    {
        GraphServiceClient GetGraphServiceClient();
    }
}
