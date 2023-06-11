using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Turmerik.Synchronized
{
    public interface IMutexCreator
    {
        Mutex Create(
            string mutexName,
            bool initiallyOwned = false,
            bool createGlobalMutex = false);
    }

    public class MutexCreator : IMutexCreator
    {
        public const string GLOBAL_MUTEX_PREFIX = "Global";

        public Mutex Create(
            string mutexName,
            bool initiallyOwned = false,
            bool createGlobalMutex = false)
        {
            mutexName = mutexName?.Replace('\\', '/') ?? string.Empty;

            if (createGlobalMutex && !string.IsNullOrWhiteSpace(mutexName))
            {
                mutexName = string.Join(
                    "\\",
                    GLOBAL_MUTEX_PREFIX,
                    mutexName);
            }

            Mutex mutex = new Mutex(initiallyOwned, mutexName);
            return mutex;
        }
    }
}
