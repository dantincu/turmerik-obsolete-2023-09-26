using System;
using System.Collections.Generic;
using System.Text;
using Turmerik.Infrastucture;
using Turmerik.LocalDevice.Core.Env;
using Turmerik.Logging;
using Turmerik.Text;

namespace Turmerik.LocalDevice.Core.Logging
{
    public interface IAppLoggerCreatorFactory
    {
        IAppLoggerCreator Create(
            bool useAppProcessIdnfByDefault = false);
    }

    public class AppLoggerCreatorFactory : IAppLoggerCreatorFactory
    {
        private readonly IAppEnv appEnv;
        private readonly IAppProcessIdentifier appProcessIdentifier;
        private readonly ITimeStampHelper timeStampHelper;
        private readonly ITrmrkJsonFormatterFactory trmrkJsonFormatterFactory;
        private readonly IStringTemplateParser stringTemplateParser;

        public AppLoggerCreatorFactory(
            IAppEnv appEnv,
            IAppProcessIdentifier appProcessIdentifier,
            ITimeStampHelper timeStampHelper,
            ITrmrkJsonFormatterFactory trmrkJsonFormatterFactory,
            IStringTemplateParser stringTemplateParser)
        {
            this.appEnv = appEnv ?? throw new ArgumentNullException(nameof(appEnv));
            this.appProcessIdentifier = appProcessIdentifier ?? throw new ArgumentNullException(nameof(appProcessIdentifier));
            this.timeStampHelper = timeStampHelper ?? throw new ArgumentNullException(nameof(timeStampHelper));
            this.trmrkJsonFormatterFactory = trmrkJsonFormatterFactory ?? throw new ArgumentNullException(nameof(trmrkJsonFormatterFactory));
            this.stringTemplateParser = stringTemplateParser ?? throw new ArgumentNullException(nameof(stringTemplateParser));
        }

        public IAppLoggerCreator Create(
            bool useAppProcessIdnfByDefault = false) => new AppLoggerCreator(
                appEnv,
                appProcessIdentifier,
                timeStampHelper,
                trmrkJsonFormatterFactory,
                stringTemplateParser,
                useAppProcessIdnfByDefault);
    }
}
