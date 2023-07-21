using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Turmerik.LaunchApp.Components
{
    public class ConsoleAppManager
    {
        private readonly string appName;
        private readonly Process process = new Process();
        private readonly object theLock = new object();
        private SynchronizationContext context;
        private string pendingWriteData;

        public ConsoleAppManager(
            string appFilePath,
            string workingDirPath = null)
        {
            this.appName = appFilePath;
            this.process.StartInfo.FileName = this.appName;

            if (workingDirPath != null)
            {
                this.process.StartInfo.WorkingDirectory = workingDirPath;
            }

            this.process.StartInfo.RedirectStandardError = true;
            this.process.StartInfo.StandardErrorEncoding = Encoding.UTF8;

            this.process.StartInfo.RedirectStandardInput = true;
            this.process.StartInfo.RedirectStandardOutput = true;
            this.process.EnableRaisingEvents = true;
            this.process.StartInfo.CreateNoWindow = true;

            this.process.StartInfo.UseShellExecute = false;

            this.process.StartInfo.StandardOutputEncoding = Encoding.UTF8;

            this.process.Exited += this.ProcessOnExited;
        }

        public event EventHandler<string> ErrorTextReceived;
        public event EventHandler ProcessExited;
        public event EventHandler<string> StandartTextReceived;

        public int ExitCode
        {
            get { return this.process.ExitCode; }
        }

        public bool Running
        {
            get; private set;
        }

        public void ExecuteAsync(params string[] args)
        {
            if (this.Running)
            {
                throw new InvalidOperationException(
                    "Process is still Running. Please wait for the process to complete.");
            }

            string arguments = string.Join(" ", args);

            this.process.StartInfo.Arguments = arguments;

            this.context = SynchronizationContext.Current;

            this.Running = true;

            this.ReadOutputAsync();
            this.WriteInputTask();
            this.ReadOutputErrorAsync();

            this.process.Start();
            this.process.WaitForExit();
        }

        public void Write(string data)
        {
            if (data == null)
            {
                return;
            }

            lock (this.theLock)
            {
                this.pendingWriteData = data;
            }
        }

        public void WriteLine(string data)
        {
            this.Write(data + Environment.NewLine);
        }

        protected virtual void OnErrorTextReceived(string e)
        {
            EventHandler<string> handler = this.ErrorTextReceived;

            if (handler != null)
            {
                if (this.context != null)
                {
                    this.context.Post(delegate { handler(this, e); }, null);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        protected virtual void OnProcessExited()
        {
            EventHandler handler = this.ProcessExited;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnStandartTextReceived(string e)
        {
            EventHandler<string> handler = this.StandartTextReceived;

            if (handler != null)
            {
                if (this.context != null)
                {
                    this.context.Post(delegate { handler(this, e); }, null);
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        private void ProcessOnExited(object sender, EventArgs eventArgs)
        {
            this.Running = false;
            this.OnProcessExited();
        }

        private async Task ReadOutputAsync()
        {
            var standart = new StringBuilder();
            var buff = new char[1024];
            int length;

            while (this.Running)
            {
                standart.Clear();

                length = await this.process.StandardOutput.ReadAsync(buff, 0, buff.Length);
                standart.Append(buff.Take(length).ToArray());
                this.OnStandartTextReceived(standart.ToString());

                if (length > 0)
                {

                }

                Thread.Sleep(1);
            }
        }

        private async Task ReadOutputErrorAsync()
        {
            var sb = new StringBuilder();

            do
            {
                sb.Clear();
                var buff = new char[1024];
                int length = await this.process.StandardError.ReadAsync(buff, 0, buff.Length);
                sb.Append(buff.Take(length).ToArray());
                this.OnErrorTextReceived(sb.ToString());
                Thread.Sleep(1);
            }
            while (this.Running);
        }

        private async Task WriteInputTask()
        {
            while (this.Running)
            {
                Thread.Sleep(1);

                if (this.pendingWriteData != null)
                {
                    await this.process.StandardInput.WriteLineAsync(this.pendingWriteData);
                    await this.process.StandardInput.FlushAsync();

                    lock (this.theLock)
                    {
                        this.pendingWriteData = null;
                    }
                }
            }
        }
    }
}
