using System.ComponentModel;

namespace Common.Core
{
    public enum SortDirectionOption
    {
        Ascending,
        Descending
    }

    public enum AuthorizationStatus
    {
        Unknown,
        Success,
        Failure,
        LockedOut,
        RequiresVerification
    }

    public enum CommandTypeOption
    {
        Added = 1,
        Updated = 2,
        Deleted = 3
    }

    public enum MessageRecipientTypeOption
    {
        To = 1,
        Cc = 2,
        Bcc = 3
    }

    public enum RepositoryIncludesDefaultOption
    {
        All = 0,
        None = 1
    }

    public enum FileDirectoryIncludeOption
    {
        None = 1,
        LevelOneSubDirectories = 2,
        AllRecursive = 3
    }

    public enum FilesIncludeOption
    {
        None = 1,
        LevelOne = 2,
        AllRecursive = 3
    }

    public enum ProcessingStatusOption
    {
        [Description("Not Executing")]
        NotExecuting = 1,
        Executing = 2,
        [Description("Finished - Succeeded")]
        FinishedSuccess = 3,
        [Description("Finished - Failed")]
        FinishedFailed = 4
    }

    public enum ProcessScheduleTypeOption
    {
        [Description("Queued Immediately")]
        QueuedImmediately = 1,
        Scheduled = 2,
        Recurring = 3
    }
}
