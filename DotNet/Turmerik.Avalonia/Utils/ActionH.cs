using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Avalonia.Utils
{
    public static class ActionH
    {
        /* public static async Task PerformActionAsync(
            string message,
            Func<Task> action,
            Func<Exception, string> errorHandler)
        {
            OutputText = message;
            OutputTextForeground = initialOutputTextForeground;

            try
            {
                await action();
            }
            catch (Exception exc)
            {
                string outputText = errorHandler(exc);
                OutputText = outputText;
                OutputTextForeground = errorOutputTextForeground;
            }
        }

        public static async Task<T> PerformActionAsync<T>(
            string message,
            Func<Task<T>> action,
            Func<Exception, Tuple<T, string>> errorHandler)
        {
            T retVal = default;

            await PerformActionAsync(
                message,
                async () =>
                {
                    retVal = await action();
                },
                exc =>
                {
                    var tuple = errorHandler(exc);
                    retVal = tuple.Item1;

                    return tuple.Item2;
                });

            return retVal;
        }*/
    }
}
