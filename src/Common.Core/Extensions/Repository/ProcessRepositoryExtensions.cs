using Common.Core.Domain;
using Common.Core.DTOs;
using Common.Core.Interfaces;
using Common.Core.Validation;
using System;
using System.Threading.Tasks;

namespace Common.Core
{
    public static class ProcessRepositoryExtensions
    {
        /// <summary>
        /// Lookup process and return <see cref="Process.IsComplete"/>.
        /// </summary>
        /// <param name="processRepository"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool IsFinished(this IProcessRepository processRepository, Guid guid)
        {
            return processRepository.GetByGuid(guid)?.IsComplete ?? false;
        }

        /// <summary>
        /// Lookup process and return <see cref="Process.IsComplete"/>.
        /// </summary>
        /// <param name="processRepository"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static async Task<bool> IsFinishedAsync(this IProcessRepository processRepository, Guid guid)
        {
            var process = await processRepository.GetByGuidAsync(guid);
            return process?.IsComplete ?? false;
        }

        /// <summary>
        /// Direct call into <see cref="IProcessRepository.Finish(Guid, ProcessErrorInfo)"/> omitting error object.
        /// </summary>
        /// <param name="processRepository"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Process Succeeded(this IProcessRepository processRepository, Guid guid)
        {
            return processRepository.Finish(guid);
        }

        /// <summary>
        /// Direct call to <see cref="IProcessRepository.Finish(Guid, ProcessErrorInfo)"/> including error object 
        /// built from provided exception <paramref name="ex"/>.
        /// </summary>
        /// <param name="processRepository"></param>
        /// <param name="guid"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Process Failed(this IProcessRepository processRepository, Guid guid, Exception ex)
        {
            Guard.IsNotNull(ex, nameof(ex));
            return Failed(processRepository, guid, new ProcessErrorInfo(Guid.NewGuid(), ex));
        }

        /// <summary>
        /// Direct call to <see cref="IProcessRepository.Finish(Guid, ProcessErrorInfo)"/> including error object.
        /// </summary>
        /// <param name="processRepository"></param>
        /// <param name="guid"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public static Process Failed(this IProcessRepository processRepository, Guid guid, ProcessErrorInfo errorInfo)
        {
            Guard.IsNotNull(errorInfo, nameof(errorInfo));
            return processRepository.Finish(guid, errorInfo);
        }
    }
}
