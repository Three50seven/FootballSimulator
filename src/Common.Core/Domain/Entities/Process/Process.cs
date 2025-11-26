using Common.Core.DTOs;

namespace Common.Core.Domain
{
    public class Process : DomainEntity
    {
        protected Process() { }

        public Process(ProcessScheduleTypeOption scheduleType, int typeId, ProcessArguments args, Guid? guid = null)
            : this(scheduleType: scheduleType,
                   typeId: typeId,
                   userId: args.UserId,
                   guid: guid,
                   description: args.Description,
                   args?.ToKeyValues()!)
        {

        }

        public Process(
            ProcessScheduleTypeOption scheduleType,
            int typeId,
            int userId,
            Guid? guid = null,
            string description = null!,
            IEnumerable<KeyValuePair<string, string>> parameters = null!)
            : base (guid ?? Guid.NewGuid())
        {
            ScheduleTypeId = (int)scheduleType;
            TypeId = typeId;
            Created = new UserCommandEvent(userId);
            Description = description.SetEmptyToNull();

            if (parameters != null)
            {
                if (Parameters is List<ProcessParameter> list)
                {
                    foreach (var kvp in parameters)
                    {

                        list.Add(new ProcessParameter(this, kvp));
                    }
                }
            }
        }

        public string? JobId { get; set; }

        public string? Description { get; private set; }

        public int TypeId { get; private set; }
        public ProcessType? Type { get; private set; }

        public int ScheduleTypeId { get; private set; }
        public ProcessScheduleType? ScheduleType { get; private set; }

        public UserCommandEvent? Created { get; private set; }
        public UserCommandEventOptional Started { get; private set; } = UserCommandEventOptional.Empty;
        public DateTime? ScheduledDate { get; set; }
        public DateTime? FinishedDate { get; private set; }
        public Guid? ErrorId { get; private set; }
        public UserCommandEventOptional Viewed { get; private set; } = UserCommandEventOptional.Empty;

        public IEnumerable<ProcessParameter> Parameters { get; private set; } = new List<ProcessParameter>();
        public IEnumerable<ProcessResult> Results { get; private set; } = new List<ProcessResult>();
        public IEnumerable<ProcessRetry> Retries { get; private set; } = new List<ProcessRetry>();

        public bool IsComplete => FinishedDate.HasValue();
        public bool HasBeenViewed => Viewed != null! && Viewed.UserId != null && Viewed.Date != null;

        public TimeSpan Duration
        {
            get
            {
                if (Started == null! || !Started.Date.HasValue())
                    return TimeSpan.Zero;

                return (FinishedDate ?? DateTime.UtcNow) - (DateTime)Started.Date!;
            }
        }

        public ProcessingStatusOption Status
        {
            get
            {
                if (HasBeenViewed)
                    return ProcessingStatusOption.NotExecuting;

                if (FinishedDate.HasValue())
                    return ErrorId.HasValue() ? ProcessingStatusOption.FinishedFailed : ProcessingStatusOption.FinishedSuccess;

                return ProcessingStatusOption.Executing;
            }
        }

        public virtual void Start(string? jobId = null, int? userId = null)
        {
            if (!string.IsNullOrWhiteSpace(jobId))
                JobId = jobId;

            // set the started date/user if has not been started yet
            // (during retries, the started date should not change)
            if (Started == null! || !Started.Date.HasValue())
                Started = new UserCommandEventOptional(userId);

            // mark any latest retry as started
            var latestRetryNotStarted = Retries?.Where(r => r.StartedDate == null)
                                               ?.OrderByDescending(r => r.Attempt)
                                               ?.FirstOrDefault();

            if (latestRetryNotStarted is not null)
                latestRetryNotStarted.StartedDate = DateTime.UtcNow;
        }

        public virtual void Finish(ProcessErrorInfo errorInfo = null!)
        {
            FinishedDate = DateTime.UtcNow;

            // record off exception info as results
            if (errorInfo != null)
            {
                ErrorId = errorInfo.Guid;
                if (errorInfo.Exception != null)
                    this.AddExceptionResult(errorInfo.Exception);
            }

            FinishLatestRetry();
        }

        public virtual void View(int userId)
        {
            Viewed = new UserCommandEventOptional(userId);
        }

        protected virtual void FinishLatestRetry()
        {
            var previousRetry = Retries?.Where(r => r.FinishedDate == null)
                                       ?.OrderByDescending(r => r.Attempt)
                                       ?.FirstOrDefault();

            if (previousRetry is not null)
                previousRetry.FinishedDate = DateTime.UtcNow;
        }

        public virtual void AddRetry(int attempt, string reason = null!)
        {
            FinishLatestRetry();

            if (Retries is List<ProcessRetry> list)
                list.Add(new ProcessRetry(this, attempt, reason));
        }

        public virtual void AddResult(string key, object value)
        {
            if (!string.IsNullOrWhiteSpace(key) && value != null && Results is List<ProcessResult> list)
                list.Add(new ProcessResult(this, key, value.ToString()!));
        }

        public virtual void AddResults(IEnumerable<KeyValuePair<string, object>> results, bool clearExisting)
        {
            if (clearExisting && results is List<ProcessResult> list)
                list.Clear();

            if (results.HasItems())
            {
                foreach (var result in results)
                {
                    AddResult(result.Key, result.Value);
                }
            }
        }
    }
}
