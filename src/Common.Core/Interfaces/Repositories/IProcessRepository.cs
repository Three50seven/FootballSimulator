using Common.Core.Domain;
using Common.Core.DTOs;
using System;
using System.Threading.Tasks;

namespace Common.Core.Interfaces
{
    public interface IProcessRepository
    {
        Process GetByGuid(Guid guid);
        Task<Process> GetByGuidAsync(Guid guid);
        Process Create(ProcessScheduleTypeOption scheduleType, int typeId, ProcessArguments arguments, DateTime? scheduledDate = null);
        Task<Process> CreateAsync(ProcessScheduleTypeOption scheduleType, int typeId, ProcessArguments arguments, DateTime? scheduledDate = null);
        Process SetJob(Guid guid, string jobId);
        Process Start(Guid guid, string jobId);
        Process Finish(Guid guid, ProcessErrorInfo errorInfo = null);
        Process Retry(Guid guid, int attempt, string reason = null);
        Process ApplyResults(Guid guid, ProcessResultMetrics resultMetrics);
    }
}
