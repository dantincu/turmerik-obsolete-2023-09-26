using System;
using System.Collections.Generic;
using System.Text;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public interface IJintConsole
    {
        void Log(object obj);
        void Trace(object obj);
        void Debug(object obj);
        void Warn(object obj);
        void Info(object obj);
        void Error(object obj);
        void Fatal(object obj);

        event Action<object> OnLog;
        event Action<object> OnTrace;
        event Action<object> OnDebug;
        event Action<object> OnInfo;
        event Action<object> OnWarn;
        event Action<object> OnError;
        event Action<object> OnFatal;
    }

    public interface IJintConsoleFactory
    {
        IJintConsole Create();
    }

    public class JintConsole : IJintConsole
    {
        private Action<object> onLog;
        private Action<object> onTrace;
        private Action<object> onDebug;
        private Action<object> onInfo;
        private Action<object> onWarn;
        private Action<object> onError;
        private Action<object> onFatal;

        public event Action<object> OnLog
        {
            add => onLog += value;
            remove => onLog -= value;
        }

        public event Action<object> OnTrace
        {
            add => onTrace += value;
            remove => onTrace -= value;
        }

        public event Action<object> OnDebug
        {
            add => onDebug += value;
            remove => onDebug -= value;
        }

        public event Action<object> OnInfo
        {
            add => onInfo += value;
            remove => onInfo -= value;
        }

        public event Action<object> OnWarn
        {
            add => onWarn += value;
            remove => onWarn -= value;
        }

        public event Action<object> OnError
        {
            add => onError += value;
            remove => onError -= value;
        }

        public event Action<object> OnFatal
        {
            add => onFatal += value;
            remove => onFatal -= value;
        }

        public void Log(object obj) => onLog?.Invoke(obj);
        public void Trace(object obj) => onTrace?.Invoke(obj);
        public void Debug(object obj) => onDebug?.Invoke(obj);
        public void Warn(object obj) => onInfo?.Invoke(obj);
        public void Info(object obj) => onWarn?.Invoke(obj);
        public void Error(object obj) => onError?.Invoke(obj);
        public void Fatal(object obj) => onFatal?.Invoke(obj);
    }

    public class JintConsoleFactory : IJintConsoleFactory
    {
        public IJintConsole Create() => new JintConsole();
    }
}
