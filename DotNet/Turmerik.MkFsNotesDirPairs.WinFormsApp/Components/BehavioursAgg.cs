﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.PureFuncJs.Core.JintCompnts;

namespace Turmerik.MkFsNotesDirPairs.WinFormsApp.Components
{
    public interface IBehavioursAgg
    {
    }

    public class BehavioursAgg : BehavioursAggBase, IBehavioursAgg
    {
        public BehavioursAgg(
            IJintComponent<IBehavioursAgg> component,
            ReadOnlyDictionary<string, ReadOnlyDictionary<string, string>> exportedMemberNames) : base(component)
        {
        }
    }
}
