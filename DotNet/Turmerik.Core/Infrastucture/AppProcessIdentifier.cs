﻿using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Cloneable;

namespace Turmerik.Infrastucture
{
    public interface IAppProcessIdentifier
    {
        AppProcessIdentifierData.IClnbl Data { get; }

        string ProcessDirNameTpl { get; }
        string ProcessDirName { get; }
    }

    public class AppProcessIdentifier : IAppProcessIdentifier
    {
        public AppProcessIdentifier()
        {
            DateTime startTimeUtc = DateTime.UtcNow;
            Guid uuid = Guid.NewGuid();

            Data = new AppProcessIdentifierData.Immtbl(
                new AppProcessIdentifierData.Mtbl
                {
                    StartTimeUtc = startTimeUtc,
                    StartTimeUtcTicks = startTimeUtc.Ticks,
                    Uuid = uuid,
                    UuidStr = uuid.ToString("N")
                });

            ProcessDirNameTpl = "[{0}]-[{1}]";

            ProcessDirName = string.Format(
                ProcessDirNameTpl,
                Data.StartTimeUtcTicks,
                Data.UuidStr);
        }

        public AppProcessIdentifierData.IClnbl Data { get; }
        public string ProcessDirNameTpl { get; }
        public string ProcessDirName { get; }

    }

    public class AppProcessIdentifierData : ClnblCore<AppProcessIdentifierData.IClnbl, AppProcessIdentifierData.Immtbl, AppProcessIdentifierData.Mtbl>
    {
        public interface IClnbl : IClnblCore
        {
            DateTime StartTimeUtc { get; }
            long StartTimeUtcTicks { get; }
            Guid Uuid { get; }
            string UuidStr { get; }
        }

        public class Immtbl : ImmtblCoreBase, IClnbl
        {
            public Immtbl(IClnbl src) : base(src)
            {
                StartTimeUtc = src.StartTimeUtc;
                StartTimeUtcTicks = src.StartTimeUtcTicks;
                Uuid = src.Uuid;
                UuidStr = src.UuidStr;
            }

            public DateTime StartTimeUtc { get; }
            public long StartTimeUtcTicks { get; }
            public Guid Uuid { get; }
            public string UuidStr { get; }
        }

        public class Mtbl : MtblCoreBase, IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src) : base(src)
            {
                StartTimeUtc = src.StartTimeUtc;
                StartTimeUtcTicks = src.StartTimeUtcTicks;
                Uuid = src.Uuid;
                UuidStr = src.UuidStr;
            }

            public DateTime StartTimeUtc { get; set; }
            public long StartTimeUtcTicks { get; set; }
            public Guid Uuid { get; set; }
            public string UuidStr { get; set; }
        }
    }
}