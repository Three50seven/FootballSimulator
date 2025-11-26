using System;
using System.Collections.Generic;

namespace Common.Core.Domain
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Add result metrics to the process.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="resultMetrics"></param>
        public static void AddResults(this Process process, ProcessResultMetrics resultMetrics)
        {
            AddResults(process, resultMetrics?.ToKeyValuesList());
        }

        /// <summary>
        /// Add multiple result key-value pair metrics to the process.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="results"></param>
        public static void AddResults(this Process process, IEnumerable<KeyValuePair<string, object>> results)
        {
            if (process == null || results == null)
                return;

            foreach (var result in results)
            {
                process.AddResult(result.Key, result.Value);
            }
        }

        /// <summary>
        /// Add results based on metrics object. Optionally set to clear existing result values.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="resultMetrics"></param>
        /// <param name="clearExisting"></param>
        public static void AddResults(this Process process, ProcessResultMetrics resultMetrics, bool clearExisting)
        {
            process?.AddResults(resultMetrics?.ToKeyValuesList(), clearExisting);
        }

        /// <summary>
        /// Add exception information as result metrics to the process.
        /// Type, Message, and StackTrace are recorded in the results.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="ex"></param>
        public static void AddExceptionResult(this Process process, Exception ex)
        {
            if (process == null || ex == null)
                return;

            var realException = ex.GetInnerMostException();

            process.AddResult("Exception_Type", realException.GetType().Name);
            process.AddResult("Exception_Message", realException.Message);
            process.AddResult("Exception_Stack_Trace", ex.ToString());
        }
    }
}
