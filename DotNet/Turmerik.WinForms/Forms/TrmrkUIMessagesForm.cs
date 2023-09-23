using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.LocalDevice.Core.Logging;
using Turmerik.Logging;
using Turmerik.Synchronized;
using Turmerik.Text;
using Turmerik.WinForms.Utils;

namespace Turmerik.WinForms.Forms
{
    public partial class TrmrkUIMessagesForm : Form
    {
        // private readonly Lazy<ITrmrkWinFormsActionComponentsManager> actionComponentsManager;
        private readonly IThreadSafeActionComponent threadSafeActionComponent;
        private readonly ITimeStampHelper timeStampHelper;
        private readonly IAppLoggerCreator appLoggerCreator;
        private readonly IAppEnv appEnv;

        private readonly IAppLogger logger;
        private readonly string logsDirPath;
        private readonly List<UIMessageLogCoreDateTime.Mtbl> uIMessagesList;
        private readonly DataGridViewCellStyle readMsgUICellStyle;

        private int currentMessageIdx;

        public TrmrkUIMessagesForm()
        {
            InitializeComponent();
            currentMessageIdx = -1;
        }

        public TrmrkUIMessagesForm(
            // Lazy<ITrmrkWinFormsActionComponentsManager> actionComponentsManager,
            IThreadSafeActionComponent threadSafeActionComponent,
            ITimeStampHelper timeStampHelper,
            IAppLoggerCreator appLoggerCreator,
            IAppEnv appEnv)
        {
            /* this.actionComponentsManager = actionComponentsManager ?? throw new ArgumentNullException(
                nameof(actionComponentsManager)); */

            this.threadSafeActionComponent = threadSafeActionComponent ?? throw new ArgumentNullException(
                nameof(threadSafeActionComponent));

            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(
                nameof(timeStampHelper));

            this.appLoggerCreator = appLoggerCreator ?? throw new ArgumentNullException(
                nameof(appLoggerCreator));

            this.appEnv = appEnv ?? throw new ArgumentNullException(
                nameof(appEnv));

            this.logger = this.appLoggerCreator.GetSharedAppLogger(GetType());
            this.logsDirPath = GetLogsDirPath();
            uIMessagesList = new List<UIMessageLogCoreDateTime.Mtbl>();

            InitializeComponent();

            readMsgUICellStyle = GetReadMsgUICellStyle();
        }

        public int MessagesCount => threadSafeActionComponent.Execute(
            () => uIMessagesList.Count);

        public UIMessageLogCoreDateTime.Immtbl this[int idx]
        {
            get => threadSafeActionComponent.Execute(
                () => uIMessagesList[idx].ToImmtbl());
        }

        public void AddMessage(
            UIMessageLogCoreDateTime.IClnbl logEvent,
            int idx = 0)
        {
            var mtblEvt = NormalizeLogMessage(
                logEvent.AsMtbl());

            var dgvRow = LogEventToDgvRow(mtblEvt);

            threadSafeActionComponent.Execute(() =>
            {
                uIMessagesList.Insert(idx, mtblEvt);
                dataGridViewMessages.Rows.Insert(idx, dgvRow);
            });
        }

        public void RemoveMessageAt(int idx) => threadSafeActionComponent.Execute(() =>
        {
            uIMessagesList.RemoveAt(idx);
            dataGridViewMessages.Rows.RemoveAt(idx);

            if (idx == currentMessageIdx)
            {
                ClearShownMessage();
            }
        });

        public void ShowMessage(int idx)
        {
            if (idx < 0)
            {
                ClearShownMessage();
            }
            else
            {
                var message = uIMessagesList[idx];
                ShowMessage(message);
            }
        }

        public void ClearShownMessage()
        {
            richTextBoxMessageDetailsContent.Text = string.Empty;
            richTextBoxMessageDetailsException.Text = string.Empty;
            textBoxMessageDetailsTimeStamp.Text = string.Empty;
            textBoxMessageDetailsLevel.Text = string.Empty;
        }

        public void SetAllMessagesAsRead()
        {
            var dgvRows = dataGridViewMessages.Rows;

            threadSafeActionComponent.Execute(() =>
            {
                var count = uIMessagesList.Count;

                for (int i = 0; i < count; i++)
                {
                    var message = uIMessagesList[i];

                    if (!message.HasBeenRead)
                    {
                        var row = dgvRows[i];
                        var timeStampCell = row.Cells[0] as DataGridViewTextBoxCell;
                        timeStampCell.Style = readMsgUICellStyle;
                        message.HasBeenRead = true;
                    }
                    else
                    {
                        break;
                    }
                }
            });
        }

        public void ClearAllMessages() =>
            threadSafeActionComponent.Execute(() =>
            {
                dataGridViewMessages.Rows.Clear();
                uIMessagesList.Clear();
                ClearShownMessage();
            });

        private void ShowMessage(
            UIMessageLogCoreDateTime.IClnbl message)
        {
            richTextBoxMessageDetailsContent.Text = message.RenderedMsg;
            richTextBoxMessageDetailsException.Text = message.SerializedExcp;
            textBoxMessageDetailsTimeStamp.Text = message.RenderedTimeStamp;
            textBoxMessageDetailsLevel.Text = message.SerializedLevel;
        }

        private DataGridViewRow LogEventToDgvRow(
            UIMessageLogCoreDateTime.IClnbl logEvent) => DgvRowsH.Row(
                new DataGridViewCell[]
                {
                    DgvRowsH.TextBoxCell(new DgvTextBoxCellOpts.Mtbl
                    {
                        CellValue = logEvent.RenderedTimeStamp
                    }),
                    DgvRowsH.TextBoxCell(new DgvTextBoxCellOpts.Mtbl
                    {
                        CellValue = logEvent.SerializedLevel
                    }),
                    DgvRowsH.TextBoxCell(new DgvTextBoxCellOpts.Mtbl
                    {
                        CellValue = logEvent.RenderedMsg
                    })
                });

        private string GetLogsDirPath()
        {
            var logsDirPath = appEnv.GetPath(
                AppEnvDir.Logs,
                this.logger.LogFilePath);

            logsDirPath = Path.GetDirectoryName(
                logsDirPath);

            logsDirPath = Path.GetDirectoryName(
                logsDirPath);

            return logsDirPath;
        }

        private UIMessageLogCoreDateTime.Mtbl NormalizeLogMessage(
            UIMessageLogCoreDateTime.Mtbl mtblEvt)
        {
            var excp = mtblEvt.Exception;

            if (mtblEvt.SerializedExcp == null && excp != null)
            {
                mtblEvt.SerializedExcp = excp.ToJson();
            }

            mtblEvt.SerializedLevel = mtblEvt.SerializedLevel ?? mtblEvt.Level.ToString().ToUpper();

            mtblEvt.RenderedMsg = mtblEvt.RenderedMsg ?? string.Format(
                mtblEvt.Message,
                mtblEvt.Properties?.ToArray() ?? new object[0]);

            mtblEvt.RenderedTimeStamp = mtblEvt.RenderedTimeStamp ?? timeStampHelper.TmStmp(
                mtblEvt.TimeStamp, true,
                TimeStamp.Seconds,
                false, false, false, null);

            return mtblEvt;
        }

        private DataGridViewCellStyle GetReadMsgUICellStyle() => dataGridViewMessages.DefaultCellStyle;

        #region UI Event Handlers

        private void TrmrkUIMessagesForm_FormClosing(
            object sender,
            FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            SetAllMessagesAsRead();
        }

        private void ButtonViewLogFiles_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(
                    "explorer.exe",
                    this.logsDirPath);
            }
            catch
            {
            }
        }

        private void ButtonClearAllMessages_Click(
            object sender,
            EventArgs e) => ClearAllMessages();

        private void DataGridViewMessages_CellClick(
            object sender,
            DataGridViewCellEventArgs e) => threadSafeActionComponent.Execute(() =>
            {
                if (e.RowIndex >= 0)
                {
                    currentMessageIdx = e.RowIndex;
                    ShowMessage(e.RowIndex);
                }
            });

        #endregion UI Event Handlers
    }
}
