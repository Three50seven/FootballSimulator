using Microsoft.EntityFrameworkCore;
using Common.Core;
using Common.Core.Domain;
using Common.Core.DTOs;
using Common.Core.Interfaces;
using Common.Core.Validation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Common.EntityFrameworkCore
{
    /// <summary>
    /// Scoped repository for interacting with <see cref="Process"/> records.
    /// Not implemented like standard, per-transaction-scoped implementation; DbContext is created and disposed on each call.
    /// Intended use for background app filters (Hangfire) or calls that can be made outside the scope of the transaction.
    /// </summary>
    public class ProcessEFScopedRepository<TDbContext> : IProcessRepository
        where TDbContext : DbContext
    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;

        public ProcessEFScopedRepository(IDbContextFactory<TDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        protected Process CreateProcess(
            ProcessScheduleTypeOption scheduleType,
            int typeId,
            ProcessArguments arguments,
            DateTime? scheduledDate = null)
        {
            var process = new Process(scheduleType, typeId, arguments);

            if (scheduledDate != null)
                process.ScheduledDate = scheduledDate;

            return process;
        }

        public Process GetByGuid(Guid guid)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Set<Process>().IncludeAllRelations().FirstOrDefault(p => p.Guid == guid);
        }

        public async Task<Process> GetByGuidAsync(Guid guid)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Set<Process>().IncludeAllRelations().FirstOrDefaultAsync(p => p.Guid == guid);
        }

        public Process Create(ProcessScheduleTypeOption scheduleType, int typeId, ProcessArguments arguments, DateTime? scheduledDate = null)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var process = CreateProcess(scheduleType, typeId, arguments, scheduledDate);

            context.Set<Process>().Add(process);
            context.SaveChanges();

            return process;
        }

        public async Task<Process> CreateAsync(ProcessScheduleTypeOption scheduleType, int typeId, ProcessArguments arguments, DateTime? scheduledDate = null)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var process = CreateProcess(scheduleType, typeId, arguments, scheduledDate);

            await context.Set<Process>().AddAsync(process);
            await context.SaveChangesAsync();

            return process;
        }

        public Process SetJob(Guid guid, string jobId)
        {
            Guard.IsNotNull(jobId, nameof(jobId));

            using var context = _dbContextFactory.CreateDbContext();
            var process = context.Set<Process>().FirstOrDefault(p => p.Guid == guid);
            if (process == null)
                throw new DataObjectNotFoundException(nameof(process), guid);

            process.JobId = jobId;

            context.Set<Process>().Update(process);
            context.SaveChanges();

            return process;
        }

        public Process Start(Guid guid, string jobId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var process = context.Set<Process>().Include(p => p.Retries).FirstOrDefault(p => p.Guid == guid);
            if (process == null)
                throw new DataObjectNotFoundException(nameof(process), guid);

            process.Start(jobId);

            context.Set<Process>().Update(process);
            context.SaveChanges();

            return process;
        }

        public Process Finish(Guid guid, ProcessErrorInfo errorInfo = null)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var process = context.Set<Process>().Include(p => p.Results).Include(p => p.Retries).FirstOrDefault(p => p.Guid == guid);
            if (process == null)
                throw new DataObjectNotFoundException(nameof(process), guid);

            // process is already marked completed, do not update any values.
            if (process.IsComplete)
                return process;

            process.Finish(errorInfo);

            context.Set<Process>().Update(process);
            context.SaveChanges();

            return process;
        }

        public Process ApplyResults(Guid guid, ProcessResultMetrics resultMetrics)
        {
            Guard.IsNotNull(resultMetrics, nameof(resultMetrics));

            using var context = _dbContextFactory.CreateDbContext();
            var process = context.Set<Process>().Include(p => p.Results).FirstOrDefault(p => p.Guid == guid);
            if (process == null)
                throw new DataObjectNotFoundException(nameof(process), guid);

            process.AddResults(resultMetrics);

            context.Set<Process>().Update(process);
            context.SaveChanges();

            return process;
        }

        public Process Retry(Guid guid, int attempt, string reason = null)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var process = context.Set<Process>().Include(p => p.Retries).FirstOrDefault(p => p.Guid == guid);
            if (process == null)
                throw new DataObjectNotFoundException(nameof(process), guid);

            process.AddRetry(attempt, reason);

            context.Set<Process>().Update(process);
            context.SaveChanges();

            return process;
        }
    }
}
